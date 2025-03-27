using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SelectionGO
{
    public List<GameObject> cursors;
    public List<Image> margins;
    public List<Image> icon;
    public Grid_Item storedItem;
    public Vector2 cellGroupSize;

    public SelectionGO(List<GameObject> gos)
    {
        cursors = gos;
        GetImagesMultiple();
    }

    public SelectionGO(GameObject go)
    {
        cursors = new List<GameObject>();
        cursors.Add(go);
        GetImagesSingle();
    }

    public void SetItem(Grid_Item item)
    {
        DisableAllCursors();
        storedItem = item;
        icon[0].sprite = storedItem.itemSprite;
        icon[0].gameObject.SetActive(true);
        cursors[0].SetActive(true);
    }

    public void SetItems(Grid_Item item, Vector2 size)
    {
        if (cursors.Count < item.cellsOccupied) // Non funziona correttamente
        {
            int difference = item.cellsOccupied - cursors.Count;
            for (int i = cursors.Count; i < difference; i++)
            {
                cursors.Add(ArtificialGrid.Instance.InstantiateSelectionGO());
                margins.Add(cursors[i].transform.Find("margins").GetComponent<Image>());
                icon.Add(cursors[i].transform.Find("icon").GetComponent<Image>());
            }

            //GetImagesMultiple();
        }

        for (int i = 0; i < cursors.Count; i++)
        {
            if (cursors[i].activeSelf)
            {
                icon[i].sprite = item.itemSprite;
            }
        }
        storedItem = item;

        SetSize(size);
        Debug.Log("size: " + size);
    }

    void SetSize(Vector2 size)
    {
        cellGroupSize = size;
    }

    public Vector2 GetSize()
    {
        return cellGroupSize;
    }

    public void SetPosition(Vector2 position)
    {
        cursors[0].transform.position = position;
    }

    public void SetPosition(List<Vector2> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            cursors[i].transform.position = positions[i];
        }
    }

    public void DisableIcons()
    {
        foreach (Image element in icon)
        {
            element.gameObject.SetActive(false);
        }
    }

    void GetImagesSingle()
    {
        margins = new List<Image>();
        icon = new List<Image>();
        margins.Add(cursors[0].transform.Find("margins").GetComponent<Image>());
        icon.Add(cursors[0].transform.Find("icon").GetComponent<Image>());
    }

    void GetImagesMultiple()
    {
        margins = new List<Image>();
        icon = new List<Image>();
        for (int i = 0; i < cursors.Count; i++)
        {
            margins.Add(cursors[i].transform.Find("margins").GetComponent<Image>());
            icon.Add(cursors[i].transform.Find("icon").GetComponent<Image>());
        }
    }

    public void Empty()
    {
        for (int i = 0; i < cursors.Count; i++)
        {
            cursors[i].SetActive(false);
            icon[i].sprite = null;
        }
        storedItem = null;
    }

    void DisableAllCursors()
    {
        foreach (GameObject go in cursors)
        {
            go.SetActive(false);
        }
    }
}
