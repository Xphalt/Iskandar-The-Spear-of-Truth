using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Helmet,
    Chest,
    Weapon,
    Shield,
    Boots,
    Default
}

public abstract class ItemObject_Sal : ScriptableObject
{
    public int id;
    public Sprite uiDisplay;
    public Mesh model;
    public ItemType type;
    [TextArea(10,15)]
    public string description;

    public abstract void Use();
}

[System.Serializable]
public class Item
{
    public string name;
    public int id;

    public Item()
    {
        name = "";
        id = -1;
    }

    public Item(ItemObject_Sal item)
    {
        name = item.name;
        id = item.id;
    }
}