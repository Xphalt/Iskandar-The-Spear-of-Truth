using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
public class DefaultObject_Sal : ItemObject_Sal
{
    public float HealingValue;

    public string Desc;

    public void Awake()
    {
        type = ItemType.Default;

        Desc = string.Concat(
            "<b><color=red>Healing value</color></b>: ", HealingValue );
    }

    public override void Use()
    {
        Debug.Log("Default item Used");
    }
}