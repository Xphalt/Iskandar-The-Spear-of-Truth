using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : StatsInterface
{
    public static int EnemiesKilled = 0;

    public bool vulnerable = true;
    [HideInInspector] public float deathTimer = 0.0f;

    public float despawnTime = 4;
    public bool isDead = false;
    public bool IsDead() { return isDead; }

    EntityDrop drops;

    private void Start()
    {
        health = MAX_HEALTH;
        drops = GetComponent<EntityDrop>();
    }

    private void Update()
    {
        if(health <= 0)
        {
            isDead = true;
            //timer til gameobj disable
            deathTimer += Time.deltaTime;
            if (deathTimer >= despawnTime)
            {
                ++EnemiesKilled;
                if (drops) drops.SpawnLoot();
                gameObject.SetActive(false);

                FinalBurst explosion = transform.GetComponentInChildren<FinalBurst>(true);
                if (explosion) explosion.Burst();
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
        if (target)
        {
            target.TakeDamage(amount, scriptedKill);
        }
    }
}
