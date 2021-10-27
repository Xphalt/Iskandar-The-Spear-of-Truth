using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Object", menuName = "Inventory System/Items/Resource")]
public class ResourceObject : ItemObject_Sal
{
    public int gems;
    public string Desc;

    public void Awake()
    {
        type = ItemType.Resource;
        //Desc = string.Concat(
        //    "<b><color=red>Physical Defence</color></b>: ", defValues.physicalDef,
        //    "\n<b><color=red>Fire Defence</color></b>: ", defValues.fireDef,
        //    "\n<b><color=red>Ice Defence</color></b>: ", defValues.iceDef);
    }

    public override void Use(params GameObject[] p_obj)
    {
        Debug.Log("Resource used");
    }
}
