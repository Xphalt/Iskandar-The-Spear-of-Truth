using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float damage;
    private float projectileLifetime;
    public float lifetimeMultiplier=2;
    private float lifetime;

    private void Update()
    {
        lifetime += Time.deltaTime;

        if (lifetime >= projectileLifetime)
            Destroy(gameObject);
    }

    public void SetDamageFromParent(float dmg, float projlife)
    {
        projectileLifetime = projlife * lifetimeMultiplier;
        damage = dmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerStats>().TakeDamage(damage,false);
            Destroy(gameObject);
        }
    }
}
