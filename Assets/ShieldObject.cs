using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Defencevalues
{
    public float physicalDef;
    public float fireDef;
    public float iceDef;

    public Defencevalues()
    {
        physicalDef = 0;
        fireDef = 0;
        iceDef = 0;
    }
    public Defencevalues(params float[] values)
    {
        physicalDef = values[0];
        fireDef = values[1];
        iceDef = values[2];
    }
}


[CreateAssetMenu(fileName = "New Shield Object", menuName = "Inventory System/Items/Shield")]
public class ShieldObject : ItemObject_Sal
{ 
    public Defencevalues defValues = new Defencevalues();

    public void Awake()
    {
        type = ItemType.Shield;
    }

    public override void Use()
    {
        Debug.Log("Shield used/Equipped");
    }
}
