using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Defencevalues
{
    public float physicalDef; 
    public bool poisonProtection;
    public bool desertProtection;
    public bool snowProtection;

    public Defencevalues()
    {
        physicalDef = 0; 
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
            "\n<b><color=red>Poison Protection</color></b>: ", defValues.poisonProtection.ToString(),
            "\n<b><color=red>Desert Protection</color></b>: ", defValues.desertProtection.ToString(),
            "\n<b><color=red>Snow Protection</color></b>: ", defValues.snowProtection.ToString());
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
