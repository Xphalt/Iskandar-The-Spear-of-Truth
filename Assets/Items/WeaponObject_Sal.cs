using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Mele,
    Throwable
}

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject_Sal : ItemObject_Sal
{
    public float damage;
    public float swingSpeed;
    public float attackCooldown;

    public float throwSpeed;
    public float throwDmgTickRate;
    public float returnSped;

    public WeaponType Type;

    public void Awake()
    {
        type = ItemTyoe.Weapon;
    }

    public override void Use()
    {
        Debug.Log("Sword Used");
    }
}
