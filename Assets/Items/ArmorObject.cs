using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
public class ArmorObject : ItemObject
{
    public float defence;

    public void Awake()
    {
        type = ItemTyoe.Armor;
    }
}
