//Bernardo Mendes
//AI Programming Dept.
//Latest Rev: 06/10/2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]

public class EnemyBase : Patrol
{
    protected Transform curTarget;
    public float chaseSpeed;
    private CharacterStats stats;

    public enum AttackTypes
    { 
        Melee,
        Charge,
        AttackTypesCount
    };

    #region Attack Info
    [NamedArrayAttribute(new string[] { "Melee", "Charge" })]
    public bool[] availableAttacks = new bool[(int)AttackTypes.AttackTypesCount];

    [NamedArrayAttribute(new string[] { "Melee", "Charge" })]
    public float[] attackDamages = new float[(int)AttackTypes.AttackTypesCount];

    [NamedArrayAttribute(new string[] { "Melee", "Charge" })]
    public float[] attackRanges = new float[(int)AttackTypes.AttackTypesCount];

    [NamedArrayAttribute(new string[] { "Melee", "Charge" })]
    public float[] attackDurations = new float[(int)AttackTypes.AttackTypesCount];

    [NamedArrayAttribute(new string[] { "Melee", "Charge" })]
    public float[] attackCooldowns = new float[(int)AttackTypes.AttackTypesCount];

    private float[] attackTimers = new float[(int)AttackTypes.AttackTypesCount];

    private float curAttackTimer = 0;

    private AttackTypes curAttack = AttackTypes.AttackTypesCount;

    protected bool MeleeAvailable => availableAttacks[(int)AttackTypes.Melee] && 
        (attackTimers[(int)AttackTypes.Melee] >= attackCooldowns[(int)AttackTypes.Melee]);
    protected bool ChargeAvailable => availableAttacks[(int)AttackTypes.Charge] && 
        (attackTimers[(int)AttackTypes.Charge] >= attackCooldowns[(int)AttackTypes.Charge]);
    protected bool AttackEnded => (curAttack != AttackTypes.AttackTypesCount) ? attackTimers[(int)curAttack] > attackDurations[(int)curAttack] : true;
    #endregion

    public enum EnemyStates
    {
        Patrolling,
        Chasing,
        Attacking
    };
    EnemyStates curState;
    protected bool charging = false;
    private bool attackUsed = false;

    public float chargeSpeed;
    private float chargeDistance;
    private Vector3 chargePoint;

    public float findDelay;
    
    public float chaseRadius, minChaseRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public override void Start()
    {
        base.Start();
        //defaultToZero = false;
        stats = GetComponent<CharacterStats>();
        StartCoroutine("FindTargetsWithDelay", findDelay);
        for (int at = 0; at < attackTimers.Length; at++) attackTimers[at] = attackCooldowns[at];
        curState = EnemyStates.Patrolling;
    }

    public override void Update()
    {
        base.Update();

        agent.enabled = curState != EnemyStates.Attacking;

        if (!charging)
        {
            switch (curState)
            {
                case EnemyStates.Patrolling:
                    agent.speed = patrolSpeed;
                    agent.stoppingDistance = 0;

                    break;
                case EnemyStates.Chasing:
                    //seek player position and go to it
                    transform.rotation = Quaternion.LookRotation(curTarget.position - transform.position);
                    agent.destination = curTarget.transform.position;
                    agent.speed = agent.remainingDistance > minChaseRadius ? chaseSpeed : 0;
                    agent.stoppingDistance = minChaseRadius;
                    
                    break;
                case EnemyStates.Attacking:
                    agent.speed = 0;
                    break;
                default:
                    //Debug.Log("Error in EnemyBase script, Current State switch statement");
                    break;
            }
        }

        Attack();
        AttackEnd();
        AttackCooldown();
    }

    private void AttackEnd()
    {
        //attack timer
        if (!AttackEnded && curAttack != AttackTypes.AttackTypesCount)
        {
            curAttackTimer += Time.deltaTime;
            if (curAttackTimer >= attackDurations[(int)curAttack])
            {
                if (charging) EndCharge();
                curState = EnemyStates.Chasing;
            }
        }
    }

    private void AttackCooldown()
    {
        for (int a = 0; a < attackCooldowns.Length; a++)
        {
            attackTimers[a] += Time.deltaTime;
        }
    }

    private void EndCharge()
    {
        charging = false;
        MyRigid.velocity = Vector3.zero;
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            if (curState != EnemyStates.Attacking) FindVisibleTargets(chaseRadius);
        }
    }

    void FindVisibleTargets(float viewRadius)
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for(int i = 0; i< targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToTarget, distance, obstacleMask))
                {
                    //found you
                    visibleTargets.Add(target);
                    curTarget = target.transform;
                    curState = EnemyStates.Chasing;
                }
            }
        }

        if (curState == EnemyStates.Chasing && visibleTargets.Count == 0) curState = EnemyStates.Patrolling;
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool MeleeRangeCheck()
    {
        foreach (RaycastHit targetScan in Physics.RaycastAll(transform.position, transform.forward, attackRanges[(int)AttackTypes.Melee]))
        {
            if (targetScan.collider.transform == curTarget) return true;
        }

        return false;
    }

    //will be called by other scripts
    public virtual void Attack ()
    {
        if(AttackEnded)
        {
            attackUsed = false;

            if (MeleeAvailable)
            {
                MeleeAttack();
            }

            if (!attackUsed && ChargeAvailable && curState == EnemyStates.Chasing)
            {
                ChargeAttack();
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

    private void ChargeAttack()
    {
        if ((curTarget.position - transform.position).magnitude <= attackRanges[(int)AttackTypes.Charge])
        {
            MyRigid.velocity = (curTarget.position - transform.position).normalized * chargeSpeed;
            chargeDistance = (curTarget.position - transform.position).magnitude;
            attackDurations[(int)AttackTypes.Charge] = chargeDistance / chargeSpeed;

            attackUsed = true;
            curAttack = AttackTypes.Charge;
            charging = true;
        }
    }

    private void MeleeAttack()
    {
        if (MeleeRangeCheck())
        {
            stats.DealDamage(curTarget.gameObject, attackDamages[(int)AttackTypes.Melee]);
            attackUsed = true;
            curAttack = AttackTypes.Melee;
            MyRigid.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform == curTarget && charging) stats.DealDamage(curTarget.gameObject, attackDamages[(int)AttackTypes.Charge]);
    }
}
