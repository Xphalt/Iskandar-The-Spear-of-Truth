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

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
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
    }

    private void SummonSpellCast()
    {
        dryadAttack = DryadAttacks.SummonSpell;
        Vector3 spawnPos = transform.position;
        for (int t = 0; t < numTrapsSpawned; t++)
        {
            spawnPos.x = transform.position.x + Random.Range(minSpawnRadius, maxSpawnRadius);
            spawnPos.z = transform.position.z + Random.Range(minSpawnRadius, maxSpawnRadius);

            GameObject newOrc = Instantiate(trapPrefab);
            newOrc.transform.position = spawnPos;
        }

        for(int p = 0; p < numPlantsSpawned; p++)
        {
            spawnPos.x = transform.position.x + Random.Range(minSpawnRadius, maxSpawnRadius);
            spawnPos.z = transform.position.z + Random.Range(minSpawnRadius, maxSpawnRadius);

            GameObject newOrc = Instantiate(plantPrefab);
            newOrc.transform.position = spawnPos;
        }

        //_myAnimator.SetTrigger();

    }
}
