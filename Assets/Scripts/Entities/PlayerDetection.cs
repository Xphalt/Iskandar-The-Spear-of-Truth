using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public List<Transform> visibleTargets = new List<Transform>();

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Range(0, 360)]
    public float viewAngle;

    public float findDelay;

    public void FindVisibleTargets(float viewRadius)
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        Transform targetTransform;

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distance, obstacleMask))
                {
                    //found you
                    visibleTargets.Add(target);
                    targetTransform = target.transform;
                }
            }
        }

    }

    private void MeleeAttack()
    {
        if (MeleeRangeCheck())
        {
            stats.DealDamage(curTarget.GetComponent<StatsInterface>(), attackDamages[(int)AttackTypes.Melee]);
            attackUsed = true;
            curAttack = AttackTypes.Melee;
            MyRigid.velocity = Vector2.zero;
        }
    }
}
