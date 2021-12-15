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
    protected float curAttackDmg = 0;

    protected bool MeleeAvailable => availableAttacks[(int)AttackTypes.Melee] &&
        (attackTimers[(int)AttackTypes.Melee] >= attackCooldowns[(int)AttackTypes.Melee]) && 
        detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee], detector.GetCurTarget());
    protected bool ChargeAvailable => availableAttacks[(int)AttackTypes.Charge] && 
        (detector.GetCurTarget().position - transform.position).magnitude <= attackRanges[(int)AttackTypes.Charge] &&
        (attackTimers[(int)AttackTypes.Charge] >= attackCooldowns[(int)AttackTypes.Charge]);
    protected bool ShootAvailable => availableAttacks[(int)AttackTypes.Shoot] &&
        (detector.GetCurTarget().position - transform.position).magnitude <= attackRanges[(int)AttackTypes.Shoot] && 
        (attackTimers[(int)AttackTypes.Shoot] >= attackCooldowns[(int)AttackTypes.Shoot]);
    
    protected bool attackEnded = true;
    #endregion

    public enum EnemyStates
    {
        Patrolling,
        Aggro,
        Attacking
    };
    internal EnemyStates curState = EnemyStates.Patrolling;
    protected bool charging = false;
    protected bool attackUsed = false;
    protected bool targeting = false;

    protected bool CanAttack => !attackUsed && attackEnded && detector.GetCurTarget() != null && curState == EnemyStates.Aggro;

    public float chargeSpeed, chargeDistanceMult = 1, chargeRaycast, chargeSelfDamage;
    public LayerMask chargeLayerMask;
    protected float chargeDistance, chargeDuration, chargeTimer;
    protected Vector3 chargeStart, chargeDirection;

    public GameObject projectileObj;
    public Transform shootPoint;
    public float projectileSpeed;
    protected BoxCollider hitCollider;

    public float minChaseRadius;


    public bool PatrolAvailable => agent.enabled && ListOfNodes.Length > 0;

    public bool isBoss = false;


    public override void Start()
    {
        base.Start();
        //defaultToZero = false;
        stats = GetComponent<EnemyStats>();
        for (int at = 0; at < attackTimers.Length; at++) attackTimers[at] = attackCooldowns[at];
        curState = EnemyStates.Patrolling;
        detector = GetComponent<PlayerDetection>();
        hitCollider = GetComponent<BoxCollider>();

        StartCoroutine("FindTargetsWithDelay", findDelay);
        for (int t = 0; t < attackTimers.Length; t++) attackTimers[t] = attackCooldowns[t];        
    }

    public override void Update()
    {
        base.Update();

        if (stats.isDead)
        {
            if (!isDead)
            {
                agent.enabled = false;
                MyRigid.velocity = Vector3.zero;
                _myAnimator.SetTrigger("Dead");
                _myCapsuleCol.enabled = false;
                AttackEnd();
                isDead = true;
                MyRigid.velocity = Vector3.zero;
            }
        }

        else
        {
            agent.enabled = curState != EnemyStates.Attacking;

            if (!charging)
            {
                switch (curState)
                {
                    case EnemyStates.Patrolling:
                        agent.speed = (PatrolAvailable && !isPaused) ? patrolSpeed : 0;
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
                            agent.destination = transform.position + (transform.position - detector.GetCurTarget().position).normalized;
                            agent.speed = (transform.position - detector.GetCurTarget().position).magnitude < detector.detectionRadius ? aggroedMoveSpeed : 0;
                        }

                        break;
                    case EnemyStates.Attacking:
                        if (targeting) transform.LookAt(detector.GetCurTarget().position);
                        agent.speed = 0;
                        break;
                    default:
                        //Debug.Log("Error in EnemyBase script, Current State switch statement");
                        break;
                }
            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, chargeRaycast, chargeLayerMask)
                    || transform.position.GetDistance(chargeStart) > chargeDistance || chargeTimer > chargeDuration)
                {
                    EndCharge();
                }
                else
                {
                    MyRigid.velocity = chargeDirection * chargeSpeed;
                    chargeTimer += Time.deltaTime;
                }
            }

            attackUsed = false;
            Attack();
            AttackCooldown();

            SetMovementAnim();
        }
    }

    public void SetMovementAnim()
    {
        switch (curState)
        {
            case EnemyStates.Patrolling:
                _myAnimator.SetBool("IsAggroed", false);
                break;
            case EnemyStates.Aggro:
                _myAnimator.SetBool("IsPatrolling", false);
                _myAnimator.SetBool("IsAggroed", true);
                break;
            case EnemyStates.Attacking:
                _myAnimator.SetBool("IsPatrolling", false);
                _myAnimator.SetBool("IsAggroed", true);
                break;
            default:
                break;
        }
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

    public virtual void AttackEnd()
    {
        curState = (detector && detector.GetCurTarget()) ? EnemyStates.Aggro : EnemyStates.Patrolling;
        attackEnded = true;
        SetWeaponActive(0);
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
        if (MyRigid) MyRigid.velocity = Vector3.zero;
        AttackEnd();
    }


    //will be called by other scripts
    public virtual void Attack()
    {
        if (CanAttack)
        {
            if (MeleeAvailable)
            {
                MeleeAttack();
            }

            if (!attackUsed && ChargeAvailable)
            {
                ChargeAttack();
            }

            if (!attackUsed && ShootAvailable)
            {
                ShootAttack();
            }

            if (attackUsed)
            {
                //change state to Attacking
                curState = EnemyStates.Attacking;
                curAttackDmg = attackDamages[(int)curAttack];
                //reset cooldown so Enemy can attack again
                attackTimers[(int)curAttack] = 0;
                attackEnded = false;
            }
        }
    }

    public virtual void ChargeAttack()
    {
        chargeStart = transform.position;
        chargeDirection = detector.GetCurTarget().position - transform.position;
        chargeDistance = chargeDirection.magnitude * chargeDistanceMult;
        chargeDuration = chargeDistance / chargeSpeed;
        chargeTimer = 0;
        chargeDirection.Normalize();
        MyRigid.velocity = chargeDirection * chargeSpeed;

        attackUsed = true;
        curAttack = AttackTypes.Charge;
        charging = true;
    }

    protected virtual void MeleeAttack()
    {
        _myAnimator.SetTrigger("Hit");

        attackUsed = true;
        curAttack = AttackTypes.Melee;
        MyRigid.velocity = Vector3.zero;
    }

    protected virtual void ShootAttack()
    {
         _myAnimator.SetTrigger("Shoot");

        attackUsed = true;
        curAttack = AttackTypes.Shoot;
        MyRigid.velocity = Vector3.zero;
        targeting = true;
    }

    public void FireShot()
    {
        transform.LookAt(detector.GetCurTarget().position, Vector3.up);
        Vector3 projectileVelocity = (detector.GetCurTarget().position - transform.position).normalized * projectileSpeed;
        GameObject projectile = Instantiate(projectileObj, shootPoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = projectileVelocity;
        projectile.GetComponent<ProjectileScript>().SetDamageFromParent(attackDamages[(int)AttackTypes.Shoot]);
        projectile.transform.LookAt(detector.GetCurTarget());
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), projectile.GetComponent<Collider>());
        attackUsed = true;
        curAttack = AttackTypes.Shoot;
        MyRigid.velocity = Vector3.zero;
        targeting = false;
    }

    protected Vector3 CalculateVelocity(Vector3 startPos, Vector3 target, float speed)
    {
        float time = 1 - speed;
        if (time == 0)
        {
            time += 0.10f;
        }

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
        if (hitCollider) hitCollider.enabled = active > 0;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(charging && detector)
        {
            if (detector.IsTarget(collision.collider.transform))
            {
                stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), curAttackDmg);
            }
            else { stats.TakeDamage(chargeSelfDamage); stats.PlayAudio(); }

            EndCharge();
        }
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (detector && detector.IsTarget(collider.transform))
        {
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(), curAttackDmg);
            hitCollider.enabled = false;
        }
    }

    protected virtual void OnDisable()
    {
        if (charging)
            EndCharge();

        if (_myAnimator)
        {
            _myAnimator.SetBool("IsAggroed", false);
            _myAnimator.SetBool("IsPatrolling", false);
            //_myAnimator.Play("Idle");
        }
        if (agent)
        {
            agent.speed = 0;
            agent.enabled = false;
        }
    }

    //Morgan Save Edit
    public virtual bool getIsDead()
    {
        return (stats.isDead);
    }
}
