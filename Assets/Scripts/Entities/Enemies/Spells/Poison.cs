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
    private bool firstCollision = false;

    private void Awake()
    {
        if (TryGetComponent(out SphereCollider sCol)) firstCollision = !Physics.CheckSphere(sCol.center, sCol.radius);
        else if (TryGetComponent(out BoxCollider bCol)) firstCollision = !Physics.CheckBox(bCol.center, bCol.size / 2);
        else if (TryGetComponent(out CapsuleCollider cCol))
            firstCollision = !Physics.CheckCapsule(cCol.center + Vector3.up * cCol.height / 2, cCol.center - Vector3.up * cCol.height / 2, cCol.radius);
        else if (TryGetComponent(out Collider _)) Debug.LogWarning("Your collider sucks");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCombat_Jerzy combat))
        {
            combat.Poison(this);
        }
        if (destroyOnContact)
        {
            if (!firstCollision) firstCollision = true;
            else Destroy(gameObject);
        };
    }
}
