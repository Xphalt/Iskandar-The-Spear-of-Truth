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

        SetMovementAnim();
    }

    public override void Attack()
    {
        base.Attack();
        
        if (CanAttack)
        {
            if (SummonAvailable)
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
    }

    protected override void AttackCooldown()
    {
        base.AttackCooldown();
        for (int a = 0; a < dryadCooldowns.Length; a++)
        {
            dryadTimers[a] += Time.deltaTime;
        }
    }

    private void SummonSpellCast()
    {
        dryadAttack = DryadAttacks.SummonSpell;

        _myAnimator.SetTrigger("CastSpell");
    }

    public void SpawnTraps()
    {
        Vector3 spawnPos = transform.position;
        for (int t = 0; t < numTrapsSpawned; t++)
        {
            GameObject newTrap = Instantiate(trapPrefab);
            newTrap.transform.position = transform.RandomRadiusPoint(minSpawnRadius, maxSpawnRadius);
        }

        for (int p = 0; p < numPlantsSpawned; p++)
        {
            GameObject newPlant = Instantiate(plantPrefab);
            newPlant.transform.position = transform.RandomRadiusPoint(minSpawnRadius, maxSpawnRadius);
        }
    }
}
