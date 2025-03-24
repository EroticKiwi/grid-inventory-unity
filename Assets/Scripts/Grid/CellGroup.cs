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
    public Grid_Item item;

    public CellGroup() { }

    public CellGroup(string name, List<Tuple<int, int>> coordinates, List<GridCell> cells, GameObject cellGO, Grid_Item item) {
        this.name = name;
        SetCells(coordinates, cells);
        InitializeCellGroupImage(cellGO);
        this.item = item;
    }

    void SetCells(List<Tuple<int, int>> coordinates, List<GridCell> cells)
    {
        for(int i = 0; i < coordinates.Count; i++)
        {
            AddCell(cells[i], coordinates[i]);
        }
    }

    void InitializeCellGroupImage(GameObject cellGO)
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

    public void Focus(bool color)
    {
        if (color)
        {
            margins.color = Color.green;
        }
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

    public Grid_Item GetItem()
    {
        return item;
    }

    public void FillCells(Grid_Item item)
    {
        foreach (GridCell cell in cells)
        {
            cell.FillCell(item);
        }
        SetImage(item.itemSprite);
    }

    public void EmptyCells()
    {
        foreach (GridCell cell in cells)
        {
            cell.EmptyCell();
        }
        SetImage(null);
        item = null;
    }

    void SetImage(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}

public class SelectionCellGroup
{
    public bool isEmpty;
    GridCell cell;
    CellGroup cellGroup;

    public void AssignCell(GridCell cell)
    {
        this.cell = cell;
    }

    public void DisableIcon()
    {
        cell.DisableIcon();
    }

    public void ActivateIcon()
    {
        cell.ActivateIcon();
    }

    public Grid_Item SelectCell()
    {
        return cell.SelectCell();
    }

    public void FocusCell(bool color)
    {
        cell.Focus(color);
        CheckIsEmpty();
    }

    public void UnFocusCell()
    {
        cell.UnFocus();
    }

    public GridCell GetCell()
    {
        return cell;
    }

    public Transform GetCellTransform()
    {
        return cell.transform;
    }

    public void AssignCellGroup(CellGroup cellGroup)
    {
        this.cellGroup = cellGroup;
    }

    public void FocusCellGroup(bool color)
    {
        cellGroup.Focus(color);
    }

    public void UnFocusCellGroup()
    {
        cellGroup.UnFocus();
    }

    public CellGroup GetCellGroup()
    {
        return cellGroup;
    }

    public Transform GetCellGroupTransform() // Returns transform of the first cell of the cellgroup
    {
        return cellGroup.cells[0].transform;
    }

    void CheckIsEmpty()
    {
        if (cellGroup == null)
        {
            if (cell.item == null)
            {
                isEmpty = true;
            }
            else
            {
                isEmpty = false;
            }
            return;
        }

        if (cellGroup.item == null)
        {
            isEmpty = true;
        }
        else
        {
            isEmpty = false;
        }
    }

    public Grid_Item GetItem()
    {
        if (cellGroup == null)
        {
            return cell.GetItem();
        }

        return cellGroup.GetItem();
    }

    public void FillCell(Grid_Item item)
    {
        if (cellGroup == null)
        {
            cell.FillCell(item);
            return;
        }

        cellGroup.FillCells(item);
    }

    public void EmptyCells()
    {
        isEmpty = true;
        if (IsSingleCell())
        {
            cell.EmptyCell();
            return;
        }

        cellGroup.EmptyCells();
    }

    bool IsSingleCell()
    {
        if (cell == null)
        {
            return false;
        }

        return true;
    }
}

public class SelectionGO
{
    public GameObject go;
    public Image margins;
    public Image icon;
    public Grid_Item storedItem;

    public SelectionGO(GameObject go)
    {
        this.go = go;
        GetImages();
    }

    public void SetItem(Grid_Item item)
    {
        storedItem = item;
        icon.sprite = storedItem.itemSprite;
        go.SetActive(true);
    }

    public void SetPosition(Vector2 position)
    {
        go.transform.position = position;
    }

    void GetImages()
    {
        margins = go.transform.Find("margins").GetComponent<Image>();
        icon = go.transform.Find("icon").GetComponent<Image>();
    }

    public void Empty()
    {
        go.SetActive(false);
        icon.sprite = null;
        storedItem = null;
    }
}