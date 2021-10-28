using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Accessories
{
    BracersOfScouting,
    Goggles,
    RingOfVitality,
    BracersOfTheLifeStealers
}
 

[CreateAssetMenu(fileName = "New Accessory Object", menuName = "Inventory System/Items/Accessory")]
public class AccessoryObject : ItemObject_Sal
{  
    public float HealingValue;
    public float regenerationInterval;
    public bool isPassive;


    public string Desc;
    public Accessories accessory; 

    public void Awake()
    {
        type = ItemType.Accessories; 

        Desc = string.Concat(
            "<b><color=red>Healing value</color></b>: ", HealingValue );
    }
     
    public override void UseBefore()
    {
        Debug.Log("Default item Used");

        //Setting values for the delegate Use
        UseFunctions.Instance.SetVariables(regenerationInterval, HealingValue);

        //Use
        OnUseBefore.Invoke();
    }
    public override void UseAfter()
    {
        //Setting values for the delegate Use
        UseFunctions.Instance.SetVariables(regenerationInterval, HealingValue);
        OnUseAfter.Invoke();
    }
}

