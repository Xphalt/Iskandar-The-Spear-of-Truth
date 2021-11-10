using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private Transform curTarget = null;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Range(0, 360)]
    public float viewAngle;

    public float detectionRadius;
    public void FindVisibleTargets()
    {
        curTarget = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);

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
                    curTarget = target;
                }
            }
        }
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool MeleeRangeCheck(float attackrange, Transform target = null)
    {
        if (!target) target = curTarget;
        foreach (RaycastHit targetScan in Physics.RaycastAll(transform.position, transform.forward, attackrange, targetMask))
        {
            if (targetScan.collider.transform == target) return true;
        }

        return false;
    }

    public Transform GetCurTarget ()
    {
        return curTarget;
    }

    public bool IsTarget(Transform check)
    {
        if (!curTarget) return false;
        return curTarget == check;
    }
}
