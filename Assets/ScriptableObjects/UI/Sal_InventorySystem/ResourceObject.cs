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

    public override void UseCurrent()
    {
        //Setting values for the delegate Use
        UseFunctions.Instance.GemsValue = gems;
        OnUseCurrent.Invoke();
    }
    public override void UseAfter()
    {
        //Setting values for the delegate Use
        UseFunctions.Instance.GemsValue = gems;
        OnUseAfter.Invoke();
    }
}
