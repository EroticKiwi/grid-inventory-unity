using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    SelectionCellGroup currentCellGroup;
    SelectionCellGroup previousCellGroup;

    SelectionCellGroup backupCellGroup;
    

    bool isSelecting = false;

    public SelectionGO selectionGO;
    public GameObject selectionGO_prefab;

    public CheckCollider checkColliderObj;
    public GameObject checkCollider_prefab;

    // DEBUG
    public GameObject doubleGrid;
    public Grid_Item medspray;
    public GameObject prefab;
    public Grid_Item firstCellItemDebug;
    public Grid_Item secondCellItemDebug;

    [Tooltip("When pressing ESC while an item is selected, chooses wheter the highlighted item grid should be the old one or the one the cursor is currently on.")]
    public bool onCancel_highlightOldItem = true;
    
    private void Awake()
    {
        InstantiateCellGroups();
        InstantiateCheckCollider();
    }

    void InstantiateCellGroups()
    {
        currentCellGroup = new SelectionCellGroup();
        previousCellGroup = new SelectionCellGroup();
        backupCellGroup = new SelectionCellGroup();
    }

    void InstantiateCheckCollider()
    {
        GameObject tempGO = Instantiate(checkCollider_prefab, GameObject.FindWithTag("InventoryGrid").transform);
        RectTransform rectTransform = tempGO.GetComponent<RectTransform>();
        checkColliderObj = new CheckCollider(rectTransform);
    }

    public void SetFirstCell(GridCell firstCell)
    {
        currentCellGroup.AssignCell(firstCell);
        currentCellGroup.AssignCellGroup(null);

        //CreateDebugCellGroup();
        FillCell();
        currentCellGroup.FocusCell(true);
        ArtificialGrid.Instance.CreateCellGroup(medspray.cellsOccupied, true, medspray);
    }

    void FillCell() // Debug
    {
        ArtificialGrid.Instance.FillCell(firstCellItemDebug, secondCellItemDebug);
    }

    public void NextCell(Vector2 direction)
    {
        GameObject hit = CheckForCell(direction);

        if (hit == null && !isSelecting)
        {
            Debug.Log("null");
            return;
        }

        // Debug.Log(hit.transform.name);

        AssignPreviousCells();
        DisablePreviousCells();
        if (hit != null)
        {
            AssignCurrentCells(hit);
        }

        // DebugLog_CurrentCellGroup();
    }

    void AssignPreviousCells()
    {
        previousCellGroup.AssignCell(currentCellGroup.GetCell());
        previousCellGroup.AssignCellGroup(currentCellGroup.GetCellGroup());
        previousCellGroup.EnableIcon();
    }

    void DisablePreviousCells()
    {
        if (previousCellGroup.GetCell() != null)
        {
            previousCellGroup.UnFocusCell();
            return;
        }

        previousCellGroup.UnFocusCellGroup();
    }

    void AssignCurrentCells(GameObject hit)
    {
        bool color = true;
        GridCell cell = hit.GetComponent<GridCell>();

        if (isSelecting)
        {
            color = false;
        }

        if (cell != null && cell.isPartOfCellGroup)
        {
            currentCellGroup.AssignCell(null);
            currentCellGroup.AssignCellGroup(cell.owner);
            currentCellGroup.FocusCellGroup(color);
            if (isSelecting)
            {
                currentCellGroup.DisableIcon();
                //selectionGO.SetPosition(); // Position to occupy
            }
            return;
        }

        currentCellGroup.AssignCell(cell);
        currentCellGroup.AssignCellGroup(null);
        currentCellGroup.FocusCell(color);

        if (isSelecting)
        {
            currentCellGroup.DisableIcon();
            selectionGO.SetPosition(currentCellGroup.GetCellTransform().position);
        }
    }

    public void SelectCells_Input()
    {
        if (!isSelecting) // Is NOW selecting
        {
            GridCell cell = currentCellGroup.GetCell();
            if (cell != null && currentCellGroup.isEmpty)
            {
                return;
            }

            if (cell != null)
            {
                SelectCell_Single();
            }
            else
            {
                SelectCell_Multiple();
            }

            isSelecting = true;
            return;
        }

        if (currentCellGroup.isEmpty)
        {
            PlaceSelection();
            return;
        }

        SwapCells();
        //isSelecting = false;
    }

    public void Cancel_Input()
    {
        if (!isSelecting)
        {
            return;
        }

        if (!selectionGO.isSelectingMultiCell)
        {
            backupCellGroup.FillCell(selectionGO.storedItem);
            selectionGO.Empty();

            currentCellGroup.EnableIcon();

            if (onCancel_highlightOldItem)
            {
                currentCellGroup.UnFocusCell();
                currentCellGroup.AssignCell(backupCellGroup.GetCell());
            }

            currentCellGroup.FocusCell(true);
        }

        isSelecting = false;
    }

    void SelectCell_Single()
    {
        if (selectionGO == null)
        {
            GameObject go = Instantiate(selectionGO_prefab, GameObject.FindWithTag("InventoryGrid").transform);
            selectionGO = new SelectionGO(go);
        }

        currentCellGroup.UnFocusCell();
        selectionGO.SetItem(currentCellGroup.SelectCell());
        selectionGO.SetPosition(currentCellGroup.GetCellTransform().position);
        BackupCellGroup();
        currentCellGroup.EmptyCells();
    }

    void SelectCell_Multiple()
    {
        if (selectionGO == null)
        {
            int amount = currentCellGroup.GetItem().cellsOccupied;
            List<GameObject> gos = new List<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                gos.Add(ArtificialGrid.Instance.InstantiateSelectionGO());
            }
            selectionGO = new SelectionGO(gos);
        }

        currentCellGroup.UnFocusCellGroup_KeepLayer();
        Grid_Item item = currentCellGroup.GetItem();
        selectionGO.SetSelection(item, currentCellGroup.GetCellGroupSize(), currentCellGroup.GetCellGroupPositions(), currentCellGroup.GetCellGroupGO());

        checkColliderObj.SetInitialCheckCollider(currentCellGroup.GetCellGroupSize(), currentCellGroup.GetCellGroupPosition());

        currentCellGroup.DisableIcon();
    }

    void PlaceSelection()
    {
        currentCellGroup.FillCell(selectionGO.storedItem);
        currentCellGroup.FocusCell(true);
        selectionGO.Empty();
        isSelecting = false;
    }

    void SwapCells()
    {
        Grid_Item tempItem = currentCellGroup.GetItem();
        currentCellGroup.FillCell(selectionGO.storedItem);
        currentCellGroup.DisableIcon();
        selectionGO.SetItem(tempItem);
    }

    void BackupCellGroup()
    {
        GridCell cell = currentCellGroup.GetCell();
        if (cell != null && cell.isPartOfCellGroup)
        {
            backupCellGroup.AssignCell(null);
            backupCellGroup.AssignCellGroup(cell.owner);
            backupCellGroup.FocusCellGroup(false);
            return;
        }

        backupCellGroup.AssignCell(cell);
        backupCellGroup.AssignCellGroup(null);
        backupCellGroup.FocusCell(false);
    }

    GameObject CheckForCell(Vector2 direction)
    {
        Vector2 position = Vector2.zero;
        if (!currentCellGroup.IsSingleCell())
        {

            if (isSelecting)
            {
                GameObject go;
                go = CheckForCellGroup(direction);
                return go;
            }

            CellGroup cellGroup = currentCellGroup.GetCellGroup();
            position = cellGroup.cells[0].transform.position;
        }
        else
        {
            position = currentCellGroup.GetCell().transform.position;
        }

        RaycastHit2D hit = Physics2D.Raycast(position, direction);

        if (hit.transform == null)
        {
            return null;
        }

        return hit.transform.gameObject;
    }

    GameObject CheckForCellGroup(Vector2 direction)
    {
        // 1 - Prendi l'oggetto Check Collider;
        // 2 - Sposta nella direzione in cui si vuole andare di una cella o riga;
        checkColliderObj.SetPosition(currentCellGroup.GetCellGroupPosition(), direction); // Problema, capisci bene come funzionano le coordinate
        
        // 3 - Controlla che il numero di celle coperte sia uguale al numero di celle necessarie da coprire, altrimenti return NULL;
        if (checkColliderObj.GetNumberOfColliders() < selectionGO.GetNumberOfOccupiedCells())
        {
            return null;
        }

        // 4 - Sposta cellGO di cellGroup nella posizione del Check Collider
        GameObject cellGO = selectionGO.GetCellGO();
        RectTransform rect = cellGO.GetComponent<RectTransform>();
        selectionGO.SetCellGOPosition(checkColliderObj.GetPosition(), rect);

        // 5 - Sposta i cursor[] attivi di selectedGO nelle posizioni delle celle appena coperte
        GameObject[] coveredCells = checkColliderObj.CheckCells();
        selectionGO.SetPosition(checkColliderObj.CheckCellsPosition());

        // 6 - Ritorna la prima cella occupata dall'immagine
        return null;
    }

    void DebugLog_CurrentCellGroup()
    {
        Debug.Log(currentCellGroup.GetCell());
        Debug.Log(currentCellGroup.GetCellGroup());
    }
}
