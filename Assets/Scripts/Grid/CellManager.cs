using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    SelectionCellGroup currentCellGroup;
    SelectionCellGroup previousCellGroup;

    SelectionCellGroup backupCellGroup;
    

    bool isSelecting = false;

    public SelectionGO selectionGO_one;
    public GameObject selectionGO_oneprefab;

    public SelectionGO selectionGO_two;

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
        currentCellGroup.FocusCell(true);

        CreateDebugCellGroup();
        FillCell();
    }

    void FillCell() // Debug
    {
        ArtificialGrid.Instance.FillCell(firstCellItemDebug, secondCellItemDebug);
    }

    void CreateDebugCellGroup() // Debug
    {
        List<Tuple<int, int>> tuple = new List<Tuple<int, int>>();

        tuple.Add(Tuple.Create(0, 1));
        tuple.Add(Tuple.Create(0, 2));

        ArtificialGrid.Instance.CreateCellGroup(tuple, "medspray", doubleGrid, medspray);
    }

    public void NextCell(Vector2 direction)
    {
        RaycastHit2D hit = CheckForCell(direction);
        if (hit.collider == null)
        {
            return;
        }

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
            return;
        }

        currentCellGroup.AssignCell(cell);
        currentCellGroup.AssignCellGroup(null);
        currentCellGroup.FocusCell(color);
        if (isSelecting)
        {
            currentCellGroup.DisableIcon();
            selectionGO_one.SetPosition(currentCellGroup.GetCellTransform().position);
        }
    }

    public void SelectCells_Input()
    {
        if (!isSelecting) // Is NOW selecting
        {
            if (currentCellGroup.isEmpty)
            {
                //Debug.Log("isEmpty!");
                return;
            }

            if (currentCellGroup.GetCellGroup() == null)
            {
                SelectCell_One();
            }
            else
            {
                //SelectCell_Two();
                //selectionGO.SetItem(currentCellGroup.GetCellGroup().GetItem());
            }
            isSelecting = true;
            return;
        }

        if (currentCellGroup.isEmpty)
        {
            currentCellGroup.FillCell(selectionGO_one.storedItem);
            currentCellGroup.FocusCell(true);
            selectionGO_one.Empty();
            isSelecting = false;
            return;
        }

        SwapCells();
        //isSelecting = false;
    }

    void SwapCells()
    {
        Grid_Item tempItem = currentCellGroup.GetItem();
        currentCellGroup.FillCell(selectionGO_one.storedItem);
        currentCellGroup.DisableIcon();
        selectionGO_one.SetItem(tempItem);
    }

    void SelectCell_One()
    {
        if (selectionGO_one == null)
        {
            GameObject go = Instantiate(selectionGO_oneprefab, GameObject.FindWithTag("InventoryGrid").transform);
            selectionGO_one = new SelectionGO(go);
        }

        currentCellGroup.UnFocusCell();
        selectionGO_one.SetItem(currentCellGroup.SelectCell());
        selectionGO_one.SetPosition(currentCellGroup.GetCellTransform().position);
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
        } else
        {
            position = currentCellGroup.GetCell().transform.position;
        }

        RaycastHit2D hit = Physics2D.Raycast(position, direction);
        return hit;
    }

    void DebugLog_CurrentCellGroup()
    {
        Debug.Log(currentCellGroup.GetCell());
        Debug.Log(currentCellGroup.GetCellGroup());
    }
}
