using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellGroup // Ogni volta che abbiamo un elemento che occupa più celle, creiamo un CellGroup, e gli diamo le coordinate nella griglia delle celle occupate
{
    public string name;
    public List<Tuple<int, int>> coordinates; // (0,1) - (0,2)
    public List<GridCell> cells;
    public GameObject cellGO; // Image on the canvas
    public Image margins;
    public Image icon;
    public Grid_Item item;
    public Vector2 cellGO_size;
    public CanvasGroup canvasGroup;

    public CellGroup() { }

    public CellGroup(string name, List<Tuple<int, int>> coordinates, List<GridCell> cells, GameObject cellGO, Grid_Item item) {
        this.name = name;
        this.item = item;
        SetCells(coordinates, cells);
        InitializeCellGroupImage(cellGO);
        FillCells(item);
        GetSize();
    }

    void GetSize()
    {
        RectTransform rect = cellGO.GetComponent<RectTransform>();
        cellGO_size = new Vector2(rect.rect.width, rect.rect.height);
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
        canvasGroup = cellGO.GetComponent<CanvasGroup>();
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

    public void UnFocus_KeepLayer()
    {
        margins.color = Color.white;
    }

    public Grid_Item GetItem()
    {
        return item;
    }

    public Grid_Item SelectCells()
    {
        foreach(GridCell cell in cells)
        {
            cell.DisableIcon();
        }
        return item;
    }

    public void FillCells(Grid_Item item)
    {
        foreach (GridCell cell in cells)
        {
            cell.FillCell_NoIcon(item);
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

    public void DisableIcon()
    {
        foreach (GridCell cell in cells)
        {
            cell.DisableIcon();
        }
        //icon.gameObject.SetActive(false);
    }

    public void EnableIcon()
    {
        foreach (GridCell cell in cells)
        {
            cell.EnableIcon();
        }
        icon.gameObject.SetActive(true);
    }

    public void BecomeTransparent()
    {
        canvasGroup.alpha = 0.2f;
    }

    public void StopTransparent()
    {
        canvasGroup.alpha = 1f;
    }

    public void SetImage(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public Vector2 GetCellGroupPosition()
    {
        return cellGO.transform.localPosition; // Local Position perchè cellGO è comunque soggetto ai suoi parent
    }

    public List<Vector2> GetCellsPositions()
    {
        List<Vector2> positions = new List<Vector2>();
        foreach (GridCell cell in cells)
        {
            positions.Add(cell.transform.position);
        }
        return positions;
    }

    public Vector2 GetCellGroupSize()
    {
        return cellGO_size;
    }

    public GameObject GetCellGO()
    {
        return cellGO;
    }

    public int GetNumberOfOccupiedCells()
    {
        return cells.Count;
    }
}