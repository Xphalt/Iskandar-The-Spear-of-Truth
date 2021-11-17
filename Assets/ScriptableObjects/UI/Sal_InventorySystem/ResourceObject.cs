using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    ConduitOre1,
    ConduitOre2,
    ConduitOre3,
    Gems,
    RevivalGem
}

[CreateAssetMenu(fileName = "New Resource Object", menuName = "Inventory System/Items/Resource")]
public class ResourceObject : ItemObject_Sal
{
    public ResourceType resourceType;
    public int gems;
    [Range(1, 100)] public float healPercentage;
    [TextArea(5, 10)] public string desc;


    public void Awake()
    {
        objType = ObjectType.Resource;
        //Desc = string.Concat(
        //    "<b><color=red>Physical Defence</color></b>: ", defValues.physicalDef,
        //    "\n<b><color=red>Fire Defence</color></b>: ", defValues.fireDef,
        //    "\n<b><color=red>Ice Defence</color></b>: ", defValues.iceDef);
    }

    public override void UseCurrent()
    {
        //Setting values for the delegate Use
        UseFunctions.Instance.GemsValue = gems;
        UseFunctions.Instance.HealingValue = healPercentage;

        OnUseCurrent.Invoke();
    }
    public override void UseAfter()
    {
        //Setting values for the delegate Use
        UseFunctions.Instance.GemsValue = gems;
        UseFunctions.Instance.HealingValue = healPercentage;

        OnUseAfter.Invoke();
    }
}
