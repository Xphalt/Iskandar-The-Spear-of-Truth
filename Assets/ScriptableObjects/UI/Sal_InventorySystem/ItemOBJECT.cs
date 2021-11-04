using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Object", menuName = "Inventory System/Items/Item")]
public class ItemOBJECT : ItemObject_Sal
{
    public float healingValue;

    public float damage;
    public float explosionRadius;
    public float timeBeforeDetonation;

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
        OnUseCurrent.Invoke();
    }
    public override void UseAfter()
    {
        OnUseAfter.Invoke();
    }
} 