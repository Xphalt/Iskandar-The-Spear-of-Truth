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

    public float detectionRadius;


    public Transform FindVisibleTargets()
    {
        visibleTargets.Clear();
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
                    visibleTargets.Add(target);
                    return target;
                }
            }
        }

        return null;
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool MeleeRangeCheck(float attackrange, Transform target)
    {
        foreach (RaycastHit targetScan in Physics.RaycastAll(transform.position, transform.forward, attackrange))
        {
            if (targetScan.collider.transform == target) return true;
        }

        return false;
    }


}
