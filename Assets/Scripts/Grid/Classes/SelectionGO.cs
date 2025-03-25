using UnityEngine;
using UnityEngine.UI;

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
