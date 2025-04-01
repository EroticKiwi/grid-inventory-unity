using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SelectionGO
{
    public List<GameObject> cursors;
    public List<Image> margins;
    public List<Image> icon;
    public Grid_Item storedItem;
    public Vector2 cellGroupSize;
    public int occupiedCells;

    public SelectionGO(List<GameObject> gos)
    {
        cursors = gos;
        GetImagesMultiple();
        occupiedCells = 1;
    }

    public SelectionGO(GameObject go)
    {
        cursors = new List<GameObject>();
        cursors.Add(go);
        GetImagesSingle();
    }

    public void SetItem(Grid_Item item, int occupiedCells)
    {
        DisableAllCursors();
        storedItem = item;
        icon[0].sprite = storedItem.itemSprite;
        icon[0].gameObject.SetActive(true);
        cursors[0].SetActive(true);
        this.occupiedCells = occupiedCells;
    }

    public void SetSelection(Grid_Item item, Vector2 size, List<Vector2> positions)
    {
        CheckIfEnoughCursors(positions.Count);
        SetItems(item, size);
        SetPosition(positions);
    }

    public void SetItems(Grid_Item item, Vector2 size)
    {
        for (int i = 0; i < cursors.Count; i++)
        {
            icon[i].sprite = item.itemSprite;
            cursors[i].SetActive(true);
        }
        storedItem = item;

        SetSize(size);
        DisableIcons();
    }

    public int GetNumberOfOccupiedCells()
    {
        return occupiedCells;
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

    void CheckIfEnoughCursors(int neededCursors)
    {
        if (cursors.Count < neededCursors)
        {
            int count = cursors.Count;
            while (count < neededCursors)
            {
                GameObject go = ArtificialGrid.Instance.InstantiateSelectionGO();
                cursors.Add(go);
                margins.Add(go.transform.Find("margins").GetComponent<Image>());
                icon.Add(go.transform.Find("icon").GetComponent<Image>());
                count++;
            }
        }

        // LogAllIcons();
    }

    void LogAllIcons()
    {
        Debug.Log("icon count = " + icon.Count);
        for (int i = 0; i < icon.Count; i++)
        {
            Debug.Log(icon[i]);
            if (i > 0 && icon[i] == icon[i-1])
            {
                Debug.Log("le icone sono uguali!");
            }
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
