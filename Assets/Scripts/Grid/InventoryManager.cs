using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    CellManager _cellManager;
    GameObject _itemPrompt;

    public void OpenItemPrompt()
    {
        _itemPrompt.SetActive(true);
    }

    public void CloseItemPrompt()
    {
        _itemPrompt.GetComponent<GridPrompt_Animation>().ClosePrompt();
    }
}
