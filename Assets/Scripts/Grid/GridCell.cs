using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    public Grid_Item item;
    public Image margins;
    public Image icon;
    public bool occupied = false;
    public bool isPartOfCellGroup = false;
    public CellGroup owner;

    private void Awake()
    {
        icon = transform.Find("icon").GetComponent<Image>();
        margins = transform.Find("margins").GetComponent<Image>();

        if (item == null)
        {
            return;
        }

        owner = new CellGroup();
        FillCell(item);
    }

    public void EmptyCell()
    {
        item = null;
        icon.sprite = null;
        icon.color = Color.black;
        occupied = false;
    }

    public void FillCell(Grid_Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.itemSprite;
        icon.color = Color.white;
        occupied = true;
    }

    public void AddToCellGroup(CellGroup cellGroup)
    {
        isPartOfCellGroup = true;
        owner = cellGroup;
    }

    public void RemoveFromCellGroup()
    {
        isPartOfCellGroup = false;
        owner = null;
    }

    public void Focus()
    {
        margins.color = Color.green;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void UnFocus()
    {
        margins.color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
