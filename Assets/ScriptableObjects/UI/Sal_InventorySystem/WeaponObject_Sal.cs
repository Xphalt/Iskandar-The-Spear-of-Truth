using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum WeaponType
{
    Mele,
    Throwable
}

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

    public WeaponType Type;

    public string Desc; 

    public void Awake()
    {
        type = ItemType.Weapon;
        Desc = string.Concat("<color=red><b>Damage value</color></b>: ", damage,
            "\n<color=red><b>Spiritual Damage value</color></b>: ", spiritualDamage);
    }

    public override void UseBefore()
    {
        Debug.Log("Sword Used");
    }
    public override void UseAfter()
    {
        OnUseAfter.Invoke();
    }
}
