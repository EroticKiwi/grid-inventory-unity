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

    public CheckColliderObject checkColliderObj;

    // DEBUG
    public GameObject doubleGrid;
    public Grid_Item medspray;
    public GameObject prefab;
    public Grid_Item firstCellItemDebug;
    public Grid_Item secondCellItemDebug;

    private void Awake()
    {
        InstantiateCellGroups();
    }

    void InstantiateCellGroups()
    {
        currentCellGroup = new SelectionCellGroup();
        previousCellGroup = new SelectionCellGroup();
        backupCellGroup = new SelectionCellGroup();
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
        RaycastHit2D hit = CheckForCell(direction);
        if (hit.collider == null)
        {
            Debug.Log("null");
            return;
        }

        Debug.Log(hit.transform.name);

        AssignPreviousCells();
        DisablePreviousCells();
        AssignCurrentCells(hit.transform.gameObject);
        
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
            currentCellGroup.FillCell(selectionGO.storedItem);
            currentCellGroup.FocusCell(true);
            selectionGO.Empty();
            isSelecting = false;
            return;
        }

        SwapCells();
        //isSelecting = false;
    }

    void SelectCell_Multiple()
    {
        if (selectionGO == null)
        {
            int amount = currentCellGroup.GetItem().cellsOccupied;
            List<GameObject> gos = new List<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                gos.Add(Instantiate(selectionGO_prefab, GameObject.FindWithTag("InventoryGrid").transform));
            }
            selectionGO = new SelectionGO(gos);
        }

        currentCellGroup.UnFocusCellGroup_KeepLayer();
        Grid_Item item = currentCellGroup.GetItem();
        selectionGO.SetItems(item);
        selectionGO.DisableIcons();
        currentCellGroup.DisableIcon();
        selectionGO.SetPosition(currentCellGroup.GetCellGroupPositions());
    }

    void SwapCells()
    {
        Grid_Item tempItem = currentCellGroup.GetItem();
        currentCellGroup.FillCell(selectionGO.storedItem);
        currentCellGroup.DisableIcon();
        selectionGO.SetItem(tempItem);
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

    RaycastHit2D CheckForCell(Vector2 direction)
    {
        Vector2 position = Vector2.zero;
        CellGroup cellGroup = currentCellGroup.GetCellGroup();
        if (cellGroup != null)
        {
            position = cellGroup.cells[0].transform.position;
        }
        else
        {
            //position = currentCellGroup.GetCell().transform.position;
            Raycast2D hit;
            hit.collider = CheckForCellGroup();
            return hit;
        }

        RaycastHit2D hit = Physics2D.Raycast(position, direction);
        return hit;
    }

    Collider2D CheckForCellGroup()
    {
        // 1 - Prendi l'oggetto Check Collider;
        // 2 - Modifica la grandezza del Check Collider per matcharla a quella del go della cellGroup;
        // 3 - Sposta nella direzione in cui si vuole andare di una cella o riga;
        // 4 - Controlla che il numero di celle coperte sia uguale al numero di celle necessarie da coprire, altrimenti return NULL;
        // 5 - Sposta cellGO di cellGroup nella posizione del Check Collider
        // 6 - Sposta i cursor[] attivi di selectedGO nelle posizioni delle celle appena coperte
    }

    void DebugLog_CurrentCellGroup()
    {
        Debug.Log(currentCellGroup.GetCell());
        Debug.Log(currentCellGroup.GetCellGroup());
    }
}
