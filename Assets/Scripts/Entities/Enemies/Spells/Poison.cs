using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float damage;
    public float poisonDamage;
    public float poisonDelay;
    public float amountOfPoisonTicks;

    public bool destroyOnContact = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCombat_Jerzy combat))
        {
            combat.Poison(this);
        }
        if (destroyOnContact) Destroy(gameObject);
    }
}
