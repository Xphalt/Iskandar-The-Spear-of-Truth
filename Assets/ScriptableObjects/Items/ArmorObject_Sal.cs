using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
public class ArmorObject_Sal : ItemObject_Sal
{
    public float defence;

    public void Awake()
    {
        type = ItemTyoe.Armor;
    }

    public override void Use()
    {
        Debug.Log("Armor used/Equipped");
    }
}
