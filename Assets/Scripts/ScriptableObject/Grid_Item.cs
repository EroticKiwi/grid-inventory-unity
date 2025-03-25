using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item_ScriptableObject")]
public class Grid_Item : ScriptableObject
{
    public int cellsOccupied = 1;
    public bool inLine = true;
    public Sprite itemSprite;
    public AudioClip gridSFX;
}
