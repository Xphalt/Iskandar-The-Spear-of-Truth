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

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
public class ArmorObject_Sal : ItemObject_Sal
{
    public Defencevalues defValues = new Defencevalues();

    [TextArea(5, 10)] public string desc;
    public string Desc
    {
        get
        {
            LocalisationTableReference physicalDefenceString;
            physicalDefenceString.tableReference = "InventoryStrings";
            physicalDefenceString.entryReference = "Armour_PhysicalDefence";

            const string value1 = "{defValues.physicalDef}";
            const string value2 = "Physical defence";
            string desc1 = desc.Replace(value1, defValues.physicalDef.ToString());
            string desc2 = desc1.Replace(value2, physicalDefenceString.GetLocalisedString());
            return desc2;
        }
    }

    public void Awake()
    {
        objType = ObjectType.Armor; 
    }

    public override void UseAfter()
    {
        
    }

    public override void UseCurrent()
    {
        Debug.Log("Armor used/Equipped");
    } 
}
