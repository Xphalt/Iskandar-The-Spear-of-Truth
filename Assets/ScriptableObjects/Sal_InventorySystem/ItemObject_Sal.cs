using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Shield,
    Armor
}

public abstract class ItemObject_Sal : ScriptableObject
{
    public int id;
    public GameObject uiDisplay;
    public ItemType type;
    [TextArea(10,15)]
    public string description;

    public abstract void Use();
}
