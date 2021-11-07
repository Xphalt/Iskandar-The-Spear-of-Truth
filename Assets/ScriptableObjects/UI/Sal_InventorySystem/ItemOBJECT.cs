using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Object", menuName = "Inventory System/Items/Item")]
public class ItemOBJECT : ItemObject_Sal
{
    public float healingValue; 

    public string Desc;

    public void Awake()
    {
        type = ItemType.Item;
        //Desc = string.Concat(
        //    "<b><color=red>Physical Defence</color></b>: ", defValues.physicalDef,
        //    "\n<b><color=red>Fire Defence</color></b>: ", defValues.fireDef,
        //    "\n<b><color=red>Ice Defence</color></b>: ", defValues.iceDef);
    }

    public override void UseCurrent()
    {
        //Setting values for the delegate Use
        UseFunctions.Instance.HealingValue = healingValue;

        OnUseCurrent.Invoke();
    }
    public override void UseAfter()
    {
        //Setting values for the delegate Use
        UseFunctions.Instance.HealingValue = healingValue;

        OnUseAfter.Invoke();
    }
} 