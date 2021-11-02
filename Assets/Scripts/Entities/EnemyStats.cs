using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : StatsInterface
{
    public bool vulnerable = true;
    private float deathTimer = 0.0f;
    public float despawnTime;
    private bool isDead = false;
    EntityDrop drops;

    private void Start()
    {
        drops = GetComponent<EntityDrop>();
    }

    private void Update()
    {
        if(isDead)
        {
            //timer til gameobj disable
            deathTimer += Time.deltaTime;
            if (deathTimer >= despawnTime)
            {
                drops.SpawnLoot();
                gameObject.SetActive(false);
            }
        }
    }


    public override void TakeDamage(float amount, bool scriptedKill = false)
    {
        if (!vulnerable) return;
        health -= amount;

        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            isDead = true;
        }
    }

    public override void DealDamage(StatsInterface target, float amount, bool scriptedKill = false)
    {
        target.TakeDamage(amount, scriptedKill);
    }
}
