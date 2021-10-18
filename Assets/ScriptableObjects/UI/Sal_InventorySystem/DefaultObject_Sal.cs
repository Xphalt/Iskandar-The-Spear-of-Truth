using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
public class DefaultObject_Sal : ItemObject_Sal
{
    public float HealingValue;

    public void Awake()
    {
        type = ItemType.Default;
    }

    public override void Use()
    {
        Debug.Log("Default item Used");
    }
}