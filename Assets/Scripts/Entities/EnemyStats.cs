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

    
    /*______________________________Damage_Flash_Variables_______________________________*/
    public SkinnedMeshRenderer MeshRenderer;
    private Color Origin;
    public float FlashTime;
    /*___________________________________________________________________________________*/

    EntityDrop drops;

    private void Start()
    {
        health = MAX_HEALTH;
        drops = GetComponent<EntityDrop>();
        MeshRenderer = GetComponent<SkinnedMeshRenderer>();
        FlashTime = 0.5f;
        Origin = MeshRenderer.material.color;
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

    /*______________________________Damage_Flash_________________________________________*/
    private IEnumerator EDamageFlash()
    {
        MeshRenderer.material.color = Color.HSVToRGB(0.08f,1,1);
        yield return new WaitForSeconds(FlashTime);
        MeshRenderer.material.color = Origin;
    }
    /*___________________________________________________________________________________*/


    public override void TakeDamage(float amount, bool scriptedKill = false)
    {
        if (!vulnerable) return;
        health -= amount;

         StartCoroutine(EDamageFlash());

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
