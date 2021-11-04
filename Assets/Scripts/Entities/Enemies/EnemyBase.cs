using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]

public class EnemyBase : Patrol
{
    protected bool isDead = false;
    public bool isChaser;
    public float aggroedMoveSpeed;
    protected EnemyStats stats;
    protected PlayerDetection detector;
    public float findDelay;

    public enum AttackTypes
    {
        Melee,
        Charge,
        Shoot,
        AttackTypesCount
    };

    #region Attack Info
    [NamedArrayAttribute(new string[] { "Melee", "Charge", "Shoot" })]
    public bool[] availableAttacks = new bool[(int)AttackTypes.AttackTypesCount];

    [NamedArrayAttribute(new string[] { "Melee", "Charge", "Shoot" })]
    public float[] attackDamages = new float[(int)AttackTypes.AttackTypesCount];

    [NamedArrayAttribute(new string[] { "Melee", "Charge", "Shoot" })]
    public float[] attackRanges = new float[(int)AttackTypes.AttackTypesCount];

    [NamedArrayAttribute(new string[] { "Melee", "Charge", "Shoot" })]
    public float[] attackCooldowns = new float[(int)AttackTypes.AttackTypesCount];

    protected float[] attackTimers = new float[(int)AttackTypes.AttackTypesCount];

    protected AttackTypes curAttack = AttackTypes.AttackTypesCount;

    protected bool MeleeAvailable => availableAttacks[(int)AttackTypes.Melee] &&
        (attackTimers[(int)AttackTypes.Melee] >= attackCooldowns[(int)AttackTypes.Melee]);
    protected bool ChargeAvailable => availableAttacks[(int)AttackTypes.Charge] &&
        (attackTimers[(int)AttackTypes.Charge] >= attackCooldowns[(int)AttackTypes.Charge]);
    protected bool ShootAvailable => availableAttacks[(int)AttackTypes.Shoot] &&
        (attackTimers[(int)AttackTypes.Shoot] >= attackCooldowns[(int)AttackTypes.Shoot]);
    
    protected bool attackEnded = true;
    #endregion

    public enum EnemyStates
    {
        Patrolling,
        Aggro,
        Attacking
    };
    protected EnemyStates curState = EnemyStates.Patrolling;
    protected bool charging = false;
    protected bool attackUsed = false;

    public float chargeSpeed, chargeDistanceMult = 1;
    protected float chargeDistance;
    protected Vector3 chargeStart, chargeDirection;

    public GameObject projectileObj;
    public Transform shootPoint;
    protected BoxCollider hitCollider;

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
        hitCollider = GetComponent<BoxCollider>();
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
                case EnemyStates.Aggro:
                    //seek player position and go to it OR if not chaser, keep distance from player
                    transform.rotation = Quaternion.LookRotation(detector.GetCurTarget().position - transform.position);
                    if (isChaser)
                    {
                        agent.destination = detector.GetCurTarget().position;
                        agent.speed = agent.remainingDistance > minChaseRadius ? aggroedMoveSpeed : 0;
                        agent.stoppingDistance = minChaseRadius;
                    }
                    else
                    {
                        agent.destination = transform.position + (transform.position - detector.GetCurTarget().transform.position).normalized;
                        agent.speed = (transform.position - detector.GetCurTarget().transform.position).magnitude < detector.detectionRadius ? aggroedMoveSpeed : 0;
                        Debug.Log((transform.position - detector.GetCurTarget().transform.position).magnitude);
                        Debug.Log((transform.position * detector.detectionRadius).magnitude);
                    }
                    

                    break;
                case EnemyStates.Attacking:
                    agent.speed = 0;
                    break;
                default:
                    //Debug.Log("Error in EnemyBase script, Current State switch statement");
                    break;
            }
        }
        else
        {
            MyRigid.velocity = chargeDirection * chargeSpeed;
            if (transform.position.GetDistance(chargeStart) > chargeDistance) EndCharge();
        }

        Attack();
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
                else curState = EnemyStates.Aggro;
            }
        }
    }

    public void AttackEnd()
    {
        curState = detector.GetCurTarget() ? EnemyStates.Aggro : EnemyStates.Patrolling;
        attackEnded = true;
    }

    protected virtual void AttackCooldown()
    {
        for (int a = 0; a < attackCooldowns.Length; a++)
        {
            attackTimers[a] += Time.deltaTime;
        }
    }

    protected virtual void EndCharge()
    {
        charging = false;
        MyRigid.velocity = Vector3.zero;
        AttackEnd();
    }


    //will be called by other scripts
    public virtual void Attack()
    {
        if (attackEnded && detector.GetCurTarget() != null)
        {
            attackUsed = false;

            if (MeleeAvailable)
            {
                MeleeAttack();
            }

            if (!attackUsed && ChargeAvailable && curState == EnemyStates.Aggro)
            {
                ChargeAttack();
            }

            if (!attackUsed && ShootAvailable && curState == EnemyStates.Aggro)
            {
                ShootAttack();
            }

            if (attackUsed)
            {
                //change state to Attacking
                curState = EnemyStates.Attacking;
                //reset cooldown so Enemy can attack again
                attackTimers[(int)curAttack] = 0;
                attackEnded = false;
            }
        }
    }

    protected virtual void ChargeAttack()
    {
        if ((detector.GetCurTarget().position - transform.position).magnitude <= attackRanges[(int)AttackTypes.Charge])
        {
            chargeStart = transform.position;
            chargeDirection = detector.GetCurTarget().position - transform.position;
            chargeDistance = chargeDirection.magnitude * chargeDistanceMult;
            chargeDirection.Normalize();
            MyRigid.velocity = chargeDirection * chargeSpeed;

            attackUsed = true;
            curAttack = AttackTypes.Charge;
            charging = true;
        }
    }

    protected virtual void MeleeAttack()
    {
        if (detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee], detector.GetCurTarget()))
        {
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Melee]);
            attackUsed = true;
            curAttack = AttackTypes.Melee;
            MyRigid.velocity = Vector3.zero;
        }
    }

    protected void ShootAttack()
    {
        if ((detector.GetCurTarget().position - transform.position).magnitude <= attackRanges[(int)AttackTypes.Shoot])
        {
            transform.LookAt(detector.GetCurTarget().position, Vector3.up);
            Vector3 projectileVelocity = CalculateVelocity(detector.GetCurTarget().position, shootPoint.position);
            GameObject projectile = Instantiate(projectileObj, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = projectileVelocity;
            projectile.GetComponent<ProjectileScript>().SetDamageFromParent(attackDamages[(int)AttackTypes.Shoot]);
            attackUsed = true;
            curAttack = AttackTypes.Shoot;
            MyRigid.velocity = Vector3.zero;

        }
    }

    protected Vector3 CalculateVelocity(Vector3 startPos, Vector3 target, float time = 1)
    {
        //find distance x and y first
        Vector3 distance = startPos - target;

        //find distance on x and z axis
        Vector3 distance_x_z = distance;
        distance_x_z.y = 0;

        //creating a float for the vertical height
        float projectileHeight = distance.y;
        float DistanceOnX_Z = distance_x_z.magnitude;

        //calculating initial x velocity
        //velocityX = x / t
        float velocityX_Z = DistanceOnX_Z / time;

        //calculating initial y velocity
        //velocityY = (y/t) + 1/2 * g * t
        float velocityY = projectileHeight / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distance_x_z.normalized;
        result *= velocityX_Z;
        result.y = velocityY;

        return result;
    }

    public void SetWeaponActive(int active)
    {
        hitCollider.enabled = active > 0;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (detector.IsTarget(collision.collider.transform) && charging)
        {
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Charge]);
        }
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (detector.IsTarget(collider.transform))
        {
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Melee]);
            hitCollider.enabled = false;
        }
    }
}
