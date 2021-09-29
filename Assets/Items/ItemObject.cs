using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTyoe
{
    Weapon,
    Armor
}

public abstract class ItemObject : ScriptableObject
{
    public GameObject prefabDisplay;
    public ItemTyoe type;
    public string description;
}
