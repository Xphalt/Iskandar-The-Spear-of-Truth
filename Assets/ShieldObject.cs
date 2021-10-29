using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Defencevalues
{
    public float physicalDef;
    public float fireDef;
    public float iceDef;
    public bool poisonProtection;
    public bool desertProtection;
    public bool snowProtection;

    public Defencevalues()
    {
        physicalDef = 0;
        fireDef = 0;
        iceDef = 0;
        poisonProtection = false;
        desertProtection = false;
        snowProtection = false;
    } 
}


[CreateAssetMenu(fileName = "New Shield Object", menuName = "Inventory System/Items/Shield")]
public class ShieldObject : ItemObject_Sal
{ 
    public Defencevalues defValues = new Defencevalues();

    public string Desc;

    public void Awake()
    {
        type = ItemType.Weapon;
        Desc = string.Concat(
            "<b><color=red>Physical Defence</color></b>: ", defValues.physicalDef,
            "\n<b><color=red>Fire Defence</color></b>: ", defValues.fireDef,
            "\n<b><color=red>Ice Defence</color></b>: ", defValues.iceDef);
    }

    public override void UseBefore()
    {
        Debug.Log("Shield used/Equipped");
    }
    public override void UseAfter()
    {
        OnUseAfter.Invoke();
    }
}
