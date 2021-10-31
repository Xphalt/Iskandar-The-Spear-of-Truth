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
            const string value1 = "{damage}";
            const string value2 = "{spiritualDamage}";
            const string value3 = "{speedBoost}";
            string newDesc1 = desc.Replace(value1, damage.ToString());
            string newDesc2 = newDesc1.Replace(value2, spiritualDamage.ToString());
            string newDesc3 = newDesc2.Replace(value3, speedBoost.ToString());
            return newDesc3;
        }
    }

    public void Awake()
    {
        type = ItemType.Weapon; 
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
