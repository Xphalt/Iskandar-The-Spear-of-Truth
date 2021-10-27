using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Armor,
    Weapon, 
    Accessories,
    Items,
    Resource,
    Material
}

public delegate void Use(params GameObject[] p_obj);
public abstract class ItemObject_Sal : ScriptableObject
{
    public int BuyValue, SellValue;

    public Sprite uiDisplay;
    public Mesh model;
    public ItemType type;
    [TextArea(10,15)] public string description;
    public bool stackable;
    public Item data = new Item();

    public Use OnUseBefore;
    public Use OnUseAfter;
    public GameObject[] useParameters; 

    public abstract void UseBefore();  
    public abstract void UseAfter();  
}

[System.Serializable]
public class Item
{
    public string name;
    public int id = 1;

    public Item()
    {
        name = "";
        id = -1;
    }

    public Item(ItemObject_Sal item)
    {
        name = item.name;
        id = item.data.id;
    }
}