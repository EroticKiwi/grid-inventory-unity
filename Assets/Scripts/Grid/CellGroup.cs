using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellGroup // Ogni volta che abbiamo un elemento che occupa più celle, creiamo un CellGroup, e gli diamo le coordinate nella griglia delle celle occupate
{
    public string name;
    public List<Tuple<int, int>> coordinates; // (0,1) - (0,2)
    public List<GridCell> cells;
    public GameObject cellGO;
    public Image margins;
    public Image icon;

    public CellGroup() { }

    public CellGroup(string name, List<Tuple<int, int>> coordinates, List<GridCell> cells, GameObject cellGO) {
        this.name = name;
        SetCells(coordinates, cells);
        SetCellImage(cellGO);
    }

    void SetCells(List<Tuple<int, int>> coordinates, List<GridCell> cells)
    {
        for(int i = 0; i < coordinates.Count; i++)
        {
            AddCell(cells[i], coordinates[i]);
        }
    }

    void SetCellImage(GameObject cellGO)
    {
        this.cellGO = cellGO;
        margins = cellGO.transform.Find("margins").GetComponent<Image>();
        icon = cellGO.transform.Find("icon").GetComponent<Image>();
    }

    public void AddCell(GridCell cell, Tuple<int, int> cellCoords)
    {
        if (cells == null)
        {
            cells = new List<GridCell>();
        }

        if (coordinates == null)
        {
            coordinates = new List<Tuple<int, int>>();
        }

        cell.AddToCellGroup(this);
        cells.Add(cell);
        coordinates.Add(cellCoords);
    }

    public void IgnoreRaycast()
    {
        foreach (GridCell cell in cells)
        {
            cell.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    public void StopIgnoringRaycast()
    {
        foreach (GridCell cell in cells)
        {
            cell.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void Focus()
    {
        margins.color = Color.green;
        cellGO.layer = LayerMask.NameToLayer("Ignore Raycast");
        foreach (GridCell cell in cells)
        {
            cell.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    public void UnFocus()
    {
        margins.color = Color.white;
        cellGO.layer = LayerMask.NameToLayer("Default");
        foreach (GridCell cell in cells)
        {
            cell.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}

public class SelectionCellGroup
{
    GridCell cell;
    CellGroup cellGroup;

    public void AssignCell(GridCell cell)
    {
        this.cell = cell;
    }

    public void FocusCell()
    {
        cell.Focus();
    }

    public void UnFocusCell()
    {
        cell.UnFocus();
    }

    public GridCell GetCell()
    {
        return cell;
    }

    public void AssignCellGroup(CellGroup cellGroup)
    {
        this.cellGroup = cellGroup;
    }

    public void FocusCellGroup()
    {
        cellGroup.Focus();
    }

    public void UnFocusCellGroup()
    {
        cellGroup.UnFocus();
    }

    public CellGroup GetCellGroup()
    {
        return cellGroup;
    }
}