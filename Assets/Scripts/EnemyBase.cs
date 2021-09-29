//Bernardo Mendes
//AI Programming Dept.
//Latest Rev: 29/09/2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    Transform curTarget;
    public float moveSpeed;

    public enum EnemyStates
    {
        Patrolling,
        Chasing,
        Attacking
    };
    EnemyStates curState;
    private float attackTimer;
    public static float attackInterval;
    private bool canAttack;

    public float findDelay;
    
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();



    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", findDelay);
        attackTimer = 0;
        canAttack = true;
        curState = EnemyStates.Patrolling;
    }

    private void Update()
    {
        switch (curState)
        {
            case EnemyStates.Patrolling:
                //move a bit to a random direction, based off of "Link's Awakening" enemy behaviour

                break;
            case EnemyStates.Chasing:
                //seek player position and go to it
                transform.position = Vector3.MoveTowards(transform.position, curTarget.position, moveSpeed);
                break;
            case EnemyStates.Attacking:
                //
                break;
            default: Debug.Log("Error in EnemyBase script, Current State switch statement");
                break;
        }

        //attack timer
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
            canAttack = true;
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
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
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
   


    //will be called by other scripts
    public void Attack ()
    {
        if(canAttack)
        {
            //change state to Attacking
            curState = EnemyStates.Attacking;
            //reset cooldown so Enemy can attack again
            canAttack = false;
            attackTimer = 0;
        }
    }

}
