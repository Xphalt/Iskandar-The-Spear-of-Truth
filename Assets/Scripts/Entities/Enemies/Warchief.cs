using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warchief : EnemyBase
{
    public int flurryChance;

    public float axeDamage, AOEDamage, axeRadius, AOERadius, blockDuration;
    public Transform axeTransform;

    public List<GameObject> orcPrefabs;
    public List<Orc> minions;

    public float minSpawnRadius = 5, maxSpawnRadius = 10, buffPercent = 10, buffDuration = 5;
    public int numOrcsSpawned = 3;
    
    private bool AOEActive = false, blocking = false;
    private float blockTimer = 0;

    public enum WarchiefAttacks
    {
        Warcry,
        Flurry,
        Lunge,
        Parry,
        AttackTypesCount
    };

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
}

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        CheckAOECollision();
    }

    public override void Attack()
    {
        if (AttackEnded && detector.GetCurTarget() != null)
        {
            attackUsed = false;

            /*if(WarcryAvailable)
            {
                WarcryAttack();
            }*/

            if (MeleeAvailable)
            {
                int rand = Random.Range(0, 101);
                if (rand > flurryChance)
                    FlurryAttack();
                else
                    MeleeAttack();
            }

            /*if(LungeAvailable && Distance check from Player)
            {
               //LungeAttack();
            }*/

            if(/*ParryAvailable &&*/ stats.health < stats.health/2)
            {
                //ParryAttack()
            }

            if (attackUsed)
            {
                //change state to Attacking
                curState = EnemyStates.Attacking;
                //reset cooldown so Enemy can attack again
                attackTimers[(int)curAttack] = 0;
                curAttackTimer = 0;
            }
        }
    }

    private void WarcryAttack()
    {
        Vector3 spawnPos = transform.position;
        for (int o = 0; o < numOrcsSpawned; o++)
        {
            spawnPos.x = transform.position.x + Random.Range(minSpawnRadius, maxSpawnRadius);
            spawnPos.z = transform.position.z + Random.Range(minSpawnRadius, maxSpawnRadius);

            GameObject newOrc = Instantiate(orcPrefabs[Random.Range(0, orcPrefabs.Count)]);
            newOrc.transform.position = spawnPos;

            minions.Add(newOrc.GetComponent<Orc>());
        }

        foreach (Orc o in minions)
            o.Buff(buffPercent, buffDuration);
    }

    private void FlurryAttack()
    {
        hitCollider.enabled = false;
    }

    private void LungeAttack()
    {

    }

    private void ParryAttack()
    {

    }

    private void CheckAOECollision()
    {
        if (AOEActive)
        {
            Collider[] objectsHit = Physics.OverlapSphere(axeTransform.position, AOERadius);
            foreach (Collider hit in objectsHit)
            {
                if (hit.TryGetComponent(out PlayerStats player))
                {
                    player.TakeDamage(AOEDamage);
                    if (hit.ClosestPoint(axeTransform.position).GetDistance(axeTransform.position) < axeRadius)
                        player.TakeDamage(axeDamage);

                    AOEActive = false;
                }
            }
        }
    }

    public void SetAOE(int active)
    {
        AOEActive = active > 0 ? true : false;
    }
}
