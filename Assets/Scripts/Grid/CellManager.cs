using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    SelectionCellGroup currentCellGroup;
    SelectionCellGroup previousCellGroup;

    bool isSelecting = false;

    public GameObject doubleGrid;
    public Grid_Item medspray;
    public GameObject prefab;

    private void Awake()
    {
        InstantiateCellGroups();
    }

    void InstantiateCellGroups()
    {
        currentCellGroup = new SelectionCellGroup();
        previousCellGroup = new SelectionCellGroup();
    }

    public void SetFirstCell(GridCell firstCell)
    {
        currentCellGroup.AssignCell(firstCell);
        currentCellGroup.FocusCell();

        CreateDebugCellGroup();
    }

    void CreateDebugCellGroup() // Debug
    {
        List<Tuple<int, int>> tuple = new List<Tuple<int, int>>();

        tuple.Add(Tuple.Create(0, 1));
        tuple.Add(Tuple.Create(0, 2));

        ArtificialGrid.Instance.CreateCellGroup(tuple, "medspray", doubleGrid);
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
        GridCell cell = hit.GetComponent<GridCell>();
        
        if (cell != null && cell.isPartOfCellGroup)
        {
            currentCellGroup.AssignCell(null);
            currentCellGroup.AssignCellGroup(cell.owner);
            currentCellGroup.FocusCellGroup();
            return;
        }

        currentCellGroup.AssignCell(cell);
        currentCellGroup.AssignCellGroup(null);
        currentCellGroup.FocusCell();
    }

    void SelectCells()
    {
        
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
