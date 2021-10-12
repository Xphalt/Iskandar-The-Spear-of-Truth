using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
public class ArmorObject_Sal : ItemObject_Sal
{
    public Defencevalues defValues = new Defencevalues();

    public void Awake()
    {
        type = ItemType.Chest;
    }

    public override void Use()
    {
        Debug.Log("Armor used/Equipped");
    }
}
