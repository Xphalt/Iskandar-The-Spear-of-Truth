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
    public float healingValue;
    public float regenerationInterval;
    public bool isPassive;


    [TextArea(5, 10)] public string desc;
    public string Desc
    {
        get 
        {
            const string value1 = "{healingValue}";
            const string value2 = "{regenerationInterval}";
            string desc1  = desc.Replace(value1, healingValue.ToString());
            string desc2  = desc1.Replace (value2, regenerationInterval.ToString());
            return desc2;
        }
    }
    public Accessories accessory; 

    void Awake()
    {
        type = ItemType.Accessory; 
    } 
     
    public override void UseCurrent()
    {
        Debug.Log("Accessory Used");

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

