using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warchief : EnemyBase
{
    public enum WarchiefAttacks
    {
        Warcry,
        Flurry,
        Lunge,
        Parry,
        AttackTypesCount
    };
    
    public int flurryChance;

    public float meleeRange, minJumpDistance, axeDamage, AOEDamageInner, AOEDamageOuter, AOERadiusInner, AOERadiusOuter, blockDuration;
    public Transform axeTransform;

    public List<GameObject> orcPrefabs;
    public List<Orc> minions;

    public float minSpawnRadius = 5, maxSpawnRadius = 10, buffPercent = 10, buffDuration = 5;
    public int numOrcsSpawned = 3;
    
    private bool AOEActive = false, blocking = false;
    private float blockTimer = 0;

    [NamedArrayAttribute(new string[] { "Warcry", "Flurry", "Lunge", "Parry" })]
    public float[] warChiefCooldowns = new float[(int)WarchiefAttacks.AttackTypesCount];

    private float[] warChiefTimers = new float[(int)WarchiefAttacks.AttackTypesCount];

    protected WarchiefAttacks warChiefAttack = WarchiefAttacks.AttackTypesCount;

    protected bool WarcryAvailable => (warChiefTimers[(int)WarchiefAttacks.Warcry] >= warChiefCooldowns[(int)WarchiefAttacks.Warcry]);
    protected bool FlurryAvailable => (warChiefTimers[(int)WarchiefAttacks.Flurry] >= warChiefCooldowns[(int)WarchiefAttacks.Flurry]);
    protected bool LungeAvailable => (warChiefTimers[(int)WarchiefAttacks.Lunge] >= warChiefCooldowns[(int)WarchiefAttacks.Lunge]);
    protected bool ParryAvailable => (warChiefTimers[(int)WarchiefAttacks.Parry] >= warChiefCooldowns[(int)WarchiefAttacks.Parry]);

    public override void Start()
    {
        base.Start();
        _myAnimator.SetBool("IsAggroed", true);

        chargeDistance = minJumpDistance;
        chargeDuration = chargeDistance / chargeSpeed;
        for (int t = 0; t < warChiefTimers.Length; t++) warChiefTimers[t] = warChiefCooldowns[t];
        isBoss = true;
    }

    public override void Update()
    {
        base.Update();
        CheckAOECollision();
    }

    public override void Attack()
    {
        if (blocking)
        {
            blockTimer += Time.deltaTime;
            if (detector.GetCurTarget()) transform.rotation = Quaternion.LookRotation(detector.GetCurTarget().position - transform.position);
            if (blockTimer > blockDuration) EndBlock();
        }
        else if (CanAttack)
        {
            attackUsed = false;

            if (!attackUsed && ParryAvailable && stats.health < stats.MAX_HEALTH / 2)
            {
                Block();
                attackUsed = true;
            }

            if (!attackUsed && FlurryAvailable && transform.GetDistance(detector.GetCurTarget()) < meleeRange)
            {
                int rand = Random.Range(0, 101);
                if (rand > flurryChance)
                    FlurryAttack();
                else
                    MeleeAttack();
                attackUsed = true;
            }

            if (!attackUsed && LungeAvailable && transform.GetDistance(detector.GetCurTarget()) > minJumpDistance)
            {
                LungeAttack();
                attackUsed = true;
            }

            if (!attackUsed && WarcryAvailable)
            {
                WarcryAttack();
                attackUsed = true;
            }

            if (attackUsed)
            {
                //change state to Attacking
                curState = EnemyStates.Attacking;
                //reset cooldown so Enemy can attack again
                warChiefTimers[(int)warChiefAttack] = 0;
                attackEnded = false;
            }
            //_myAnimator.SetBool("IsAggroed", !attackUsed);
        }
    }

    private void EndBlock()
    {
        blocking = false;
        stats.vulnerable = true;
        _myAnimator.SetBool("IsBlocking", false);
    }

    protected override void AttackCooldown()
    {
        for (int a = 0; a < warChiefCooldowns.Length; a++)
        {
            warChiefTimers[a] += Time.deltaTime;
        }
    }

    private void WarcryAttack()
    {
        warChiefAttack = WarchiefAttacks.Warcry;
        for (int o = 0; o < numOrcsSpawned; o++)
        {
            GameObject newOrc = Instantiate(orcPrefabs[Random.Range(0, orcPrefabs.Count)]);
            newOrc.transform.position = transform.RandomRadiusPoint(minSpawnRadius, maxSpawnRadius);
            if (newOrc.TryGetComponent(out NavMeshAgent newAgent))
            {
                NavMesh.SamplePosition(newOrc.transform.position, out NavMeshHit hit, 100.0f, NavMesh.AllAreas);
                newAgent.Warp(hit.position);
            }

            minions.Add(newOrc.GetComponent<Orc>());
        }

        foreach (Orc o in minions)
            o.Buff(buffPercent, buffDuration);

        _myAnimator.SetTrigger("Warcry");
    }

    protected override void MeleeAttack()
    {
        _myAnimator.SetTrigger("Slash");
        MyRigid.velocity = Vector3.zero;
        warChiefAttack = WarchiefAttacks.Flurry;
    }

    private void FlurryAttack()
    {
        _myAnimator.SetTrigger("Flurry");
        MyRigid.velocity = Vector3.zero;
        warChiefAttack = WarchiefAttacks.Flurry;
    }

    private void LungeAttack()
    {
        chargeStart = transform.position;
        chargeDirection = (detector.GetCurTarget().position - transform.position).normalized;
        chargeTimer = 0;
        MyRigid.velocity = chargeDirection * chargeSpeed;
        charging = true;

        _myAnimator.SetTrigger("Lunge");
        warChiefAttack = WarchiefAttacks.Lunge;
    }

    private void Block()
    {
        blocking = true;
        blockTimer = 0;
        stats.vulnerable = false;
        _myAnimator.SetBool("IsBlocking", true);
        _myAnimator.SetTrigger("Taunt");
        warChiefAttack = WarchiefAttacks.Parry;
    }

    private void CheckAOECollision()
    {
        if (!AOEActive) return;

        Collider[] objectsHit = Physics.OverlapSphere(axeTransform.position, AOERadiusOuter);
        foreach (Collider hit in objectsHit)
        {
            if (hit.TryGetComponent(out PlayerStats player))
            {
                player.TakeDamage(AOEDamageOuter);
                if (hit.ClosestPoint(axeTransform.position).GetDistance(axeTransform.position) < AOERadiusInner)
                    player.TakeDamage(AOEDamageInner);

                AOEActive = false;
            }
        }
    }

    public void SetAOE(int active)
    {
        AOEActive = active > 0;
    }

    protected override void EndCharge()
    {
        charging = false;
        MyRigid.velocity = Vector3.zero;
    }

    protected override void OnTriggerEnter(Collider other)
    {        
        if (detector.IsTarget(other.transform))
        {
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), axeDamage);
            hitCollider.enabled = false;
        }

        if (other.CompareTag("playerSword") && blocking)
        {
            EndBlock();
            MeleeAttack();
        }
    }
}
