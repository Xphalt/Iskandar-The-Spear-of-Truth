using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject_Sal : ItemObject_Sal
{
    [Space(5)] [Header("Damage/Boost Values")]
    public float damage;
    public float spiritualDamage;
    public float speedBoost;

    [Space(5)] [Header("Swing/Throw valuees")]
    public float swingSpeed;
    public float attackCooldown;
    public float throwSpeed;
    public float throwDmgTickRate;
    public float returnSped;

    [TextArea(5, 10)] public string desc;
    public string Desc
    {
        get
        {
            LocalisationTableReference damageString;
            damageString.tableReference = "InventoryStrings";
            damageString.entryReference = "Weapon_DamageValue";
            LocalisationTableReference spiritualDamageString;
            spiritualDamageString.tableReference = "InventoryStrings";
            spiritualDamageString.entryReference = "Weapon_SpiritualDamageValue";
            LocalisationTableReference speedBoostString;
            speedBoostString.tableReference = "InventoryStrings";
            speedBoostString.entryReference = "Weapon_SpeedBoost";

            const string value1 = "{damage}";
            const string value2 = "{spiritualDamage}";
            const string value3 = "{speedBoost}";
            const string value4 = "Damage value";
            const string value5 = "Spiritual Damage value";
            const string value6 = "Speed Boost";
            string newDesc1 = desc.Replace(value1, damage.ToString());
            string newDesc2 = newDesc1.Replace(value2, spiritualDamage.ToString());
            string newDesc3 = newDesc2.Replace(value3, speedBoost.ToString());
            string newDesc4 = newDesc3.Replace(value4, damageString.GetLocalisedString());
            string newDesc5 = newDesc4.Replace(value5, spiritualDamageString.GetLocalisedString());
            string newDesc6 = newDesc5.Replace(value6, speedBoostString.GetLocalisedString());
            return newDesc6;
        }
    }

    public void Awake()
    {
        objType = ObjectType.Weapon; 
    }

    public override void UseCurrent()
    {
        Debug.Log("Sword Used");
    }
    public override void UseAfter()
    {
        OnUseAfter.Invoke();
    }
}
