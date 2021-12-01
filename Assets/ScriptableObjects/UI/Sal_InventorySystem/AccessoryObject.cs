using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccessoryType
{
    BraceletOfScouting,
    Goggles,
    RingOfVitality,
    NecklaceOfTheLifeStealers
}
 

[CreateAssetMenu(fileName = "New Accessory Object", menuName = "Inventory System/Items/Accessory")]
public class AccessoryObject : ItemObject_Sal
{ 
    public float healingValue;
    public float regenerationInterval;
    public bool isPassive;


    [TextArea(5, 10)] public string desc;
    public string Desc
    {
        get 
        {
            LocalisationTableReference healingString;
            healingString.tableReference = "InventoryStrings";
            healingString.entryReference = "Accessory_HealingValue";
            LocalisationTableReference regenerationIntervalString;
            regenerationIntervalString.tableReference = "InventoryStrings";
            regenerationIntervalString.entryReference = "Accessory_RegenerationRate";

            const string value1 = "{healingValue}";
            const string value2 = "{regenerationInterval}";
            const string value3 = "Healing value";
            const string value4 = "Regeneration rate";
            string desc1  = desc.Replace(value1, healingValue.ToString());
            string desc2  = desc1.Replace (value2, regenerationInterval.ToString());
            string desc3  = desc2.Replace(value3, healingString.GetLocalisedString());
            string desc4  = desc3.Replace(value4, regenerationIntervalString.GetLocalisedString());
            return desc4;
        }
    }
    public AccessoryType accessory; 

    void Awake()
    {
        objType = ObjectType.Accessory; 
    } 
     
    public override void UseCurrent()
    { 
        //Setting values for the delegate Use
        UseFunctions.Instance.RegenerationInterval = regenerationInterval; 
        UseFunctions.Instance.HealingValue = healingValue; 

        OnUseCurrent.Invoke();
    }
    public override void UseAfter()
    {
        //Setting values for the delegate Use
        UseFunctions.Instance.RegenerationInterval = regenerationInterval;
        UseFunctions.Instance.HealingValue = healingValue;

        OnUseAfter.Invoke();
    }
}

