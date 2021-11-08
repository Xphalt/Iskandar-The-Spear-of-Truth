using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dryad : EnemyBase
{
    public enum DryadAttacks
    {
        SummonSpell,
        AttackTypesCount
    };

    public int numTrapsSpawned;
    public int numPlantsSpawned;
    public float minSpawnRadius;
    public float maxSpawnRadius;
    public GameObject trapPrefab;
    public GameObject plantPrefab;

    [NamedArrayAttribute(new string[] { "SummonSpell" })]
    public float[] dryadCooldowns = new float[(int)DryadAttacks.AttackTypesCount];
    private float[] dryadTimers = new float[(int)DryadAttacks.AttackTypesCount];
    protected DryadAttacks dryadAttack = DryadAttacks.AttackTypesCount;

    protected bool SummonAvailable => (dryadTimers[(int)DryadAttacks.SummonSpell] >= dryadCooldowns[(int)DryadAttacks.SummonSpell]);

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        base.Attack();

        if (!attackUsed && SummonAvailable)
        {
            SummonSpellCast();
            attackUsed = true;
        }

        if (attackUsed)
        {
            //change state to Attacking
            curState = EnemyStates.Attacking;
            //reset cooldown so Enemy can attack again
            dryadTimers[(int)dryadAttack] = 0;
            attackEnded = false;
        }
    }

    private void SummonSpellCast()
    {
        dryadAttack = DryadAttacks.SummonSpell;

        Vector3 spawnPos = transform.position;
        for (int t = 0; t < numTrapsSpawned; t++)
        {
            spawnPos.x = transform.position.x + Random.Range(minSpawnRadius, maxSpawnRadius);
            spawnPos.z = transform.position.z + Random.Range(minSpawnRadius, maxSpawnRadius);

            GameObject newTrap = Instantiate(trapPrefab);
            newTrap.transform.position = spawnPos;
        }

        for(int p = 0; p < numPlantsSpawned; p++)
        {
            spawnPos.x = transform.position.x + Random.Range(minSpawnRadius, maxSpawnRadius);
            spawnPos.z = transform.position.z + Random.Range(minSpawnRadius, maxSpawnRadius);

            GameObject newPlant = Instantiate(plantPrefab);
            newPlant.transform.position = spawnPos;
        }

        //_myAnimator.SetTrigger();

    }
}
