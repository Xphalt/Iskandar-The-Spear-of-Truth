using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Accessories
{
    BraceletOfScouting,
    Goggles,
    RingOfVitality,
    NecklaceOfTheLifeStealers
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
            "<b><color=red>Healing value</color></b>: ", HealingValue,
            "\nRegeneration Rate: ", regenerationInterval, "sec");
    }
     
    public override void UseBefore()
    {
        Debug.Log("Accessory Used");

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

