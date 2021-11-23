using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float damage;
    protected float projectileLifetime;
    public float lifetimeMultiplier=2;
    protected float lifetime;

    public virtual void Update()
    {
        lifetime += Time.deltaTime;

        if (lifetime >= projectileLifetime)
            Destroy(gameObject);
    }

    public virtual void SetDamageFromParent(float dmg, float projlife = 1)
    {
        projectileLifetime = projlife * lifetimeMultiplier;
        damage = dmg;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerStats stats))
        {
            stats.TakeDamage(damage,false);
        }
        Destroy(gameObject);
    }
}
