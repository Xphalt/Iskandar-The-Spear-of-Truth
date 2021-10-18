using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]

public class EnemyBase : Patrol
{
    public float chaseSpeed;
    private EnemyStats stats;
    PlayerDetection detector;
    public float findDelay;

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

    
    public float minChaseRadius;


    [HideInInspector]

    public override void Start()
    {
        base.Start();
        StartCoroutine("FindTargetsWithDelay", findDelay);
        //defaultToZero = false;
        stats = GetComponent<EnemyStats>();
        for (int at = 0; at < attackTimers.Length; at++) attackTimers[at] = attackCooldowns[at];
        curState = EnemyStates.Patrolling;
        detector = GetComponent<PlayerDetection>();
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
                    transform.rotation = Quaternion.LookRotation(detector.GetCurTarget().position - transform.position);
                    agent.destination = detector.GetCurTarget().transform.position;
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

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (curState != EnemyStates.Attacking)
            {
                detector.FindVisibleTargets();
                if (detector.GetCurTarget() == null) curState = EnemyStates.Patrolling;
                else curState = EnemyStates.Chasing;
            }
        }
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

    //public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    //{
    //    if(!angleIsGlobal)
    //        angleInDegrees += transform.eulerAngles.y;

    //    return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    //}


    //will be called by other scripts
    public virtual void Attack ()
    {
        if(AttackEnded && detector.GetCurTarget() != null)
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
        if ((detector.GetCurTarget().position - transform.position).magnitude <= attackRanges[(int)AttackTypes.Charge])
        {
            MyRigid.velocity = (detector.GetCurTarget().position - transform.position).normalized * chargeSpeed;
            chargeDistance = (detector.GetCurTarget().position - transform.position).magnitude;
            attackDurations[(int)AttackTypes.Charge] = chargeDistance / chargeSpeed;

            attackUsed = true;
            curAttack = AttackTypes.Charge;
            charging = true;
        }
    }

    private void MeleeAttack()
    {
        if (detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee], detector.GetCurTarget()))
        {
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Melee]);
            attackUsed = true;
            curAttack = AttackTypes.Melee;
            MyRigid.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform == detector.GetCurTarget() && charging) stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Charge]);
    }
}
