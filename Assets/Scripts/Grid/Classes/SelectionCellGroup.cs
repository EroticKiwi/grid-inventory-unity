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
        cell.DisableIcon();
    }

    public void EnableIcon()
    {
        cell.EnableIcon();
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
