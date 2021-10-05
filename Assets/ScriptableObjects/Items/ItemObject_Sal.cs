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
    public GameObject prefabDisplay;
    public ItemType type;
    public string description;

    public abstract void Use();
}
