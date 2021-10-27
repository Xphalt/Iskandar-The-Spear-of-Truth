using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Object", menuName = "Inventory System/Items/Item")]
public class ItemOBJECT : ItemObject_Sal
{
    public float HealingValue;

    public string Desc;

    public void Awake()
    {
        type = ItemType.Armor;
        //Desc = string.Concat(
        //    "<b><color=red>Physical Defence</color></b>: ", defValues.physicalDef,
        //    "\n<b><color=red>Fire Defence</color></b>: ", defValues.fireDef,
        //    "\n<b><color=red>Ice Defence</color></b>: ", defValues.iceDef);
    }

    public override void UseBefore()
    {
        Debug.Log("Item used/Equipped");
    }
    public override void UseAfter()
    {
        OnUseAfter.Invoke(useParameters);
    }
} 