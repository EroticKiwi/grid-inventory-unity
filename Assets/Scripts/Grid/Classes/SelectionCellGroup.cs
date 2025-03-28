using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

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
        if (IsSingleCell())
        {
            cell.DisableIcon();
            return;
        }

        cellGroup.DisableIcon();
    }

    public void EnableIcon()
    {
        if (IsSingleCell())
        {
            cell.EnableIcon();
            return;
        }
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

    public  void UnFocusCellGroup_KeepLayer()
    {
        cellGroup.UnFocus_KeepLayer();
    }

    public CellGroup GetCellGroup()
    {
        return cellGroup;
    }

    public Transform GetCellGroupTransform() // Returns transform of the first cell of the cellgroup
    {
        return cellGroup.cells[0].transform;
    }

    public List<Vector2> GetCellGroupPositions()
    {
        return cellGroup.GetCellsPositions();
    }

    public Vector2 GetCellGroupPosition()
    {
        //Debug.Log(cellGroup.GetCellGroupPosition());
        return cellGroup.GetCellGroupPosition();
    }

    public Vector2 GetCellGroupSize()
    {
        return cellGroup.GetCellGroupSize();
    }

    public Grid_Item SelectCells()
    {
        DisableIcon();
        return cellGroup.item;
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

    public Vector2 GetCellSize()
    {
        return cellGroup.GetCellGroupSize();
    }

    public void FillCell(Grid_Item item)
    {
        isEmpty = false;

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
