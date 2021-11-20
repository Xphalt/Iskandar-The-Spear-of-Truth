using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemyBase
{

    [SerializeField] private float ResurrectionChancePercentage;
    private bool Resurrect = false;

    [Header ("For Aggroing Skeletons In Vacinity")]
    public float skeletonAgroRadius;
    [Range(0, 360)]
    public float agroSupportAngle;
    public LayerMask skeletonMask;
    public LayerMask obstacleMask;
    [HideInInspector]
    public List<Transform> SkeletonsInAgroRange = new List<Transform>();



    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (detector.GetCurTarget() != null)
        {
            FindSkeletonsInRange();
            if (skeletonAgroRadius != 0)
            { 
                for (int i = 0; i < SkeletonsInAgroRange.Count; i++)
                {
                    //disabled this line due to the changes in PlayerDetection, must add the SetCurrentTarget later again -Bernardo
                    //SkeletonsInAgroRange[i].gameObject.GetComponent<PlayerDetection>().SetCurrentTarget(detector.GetCurTarget());
                    SkeletonsInAgroRange[i].gameObject.GetComponent<Skeleton>().SetStateChasing();
                }
            }
        }

        if (gameObject.GetComponent<EnemyStats>().health <= 0)
        {
            //die
            ResurrectChance();
            if (Resurrect == true)
            {
                //comeback to life
            }
        }
    }

    void FindSkeletonsInRange()
    {
        SkeletonsInAgroRange.Clear();
        Collider[] skeletonInViewRadius = Physics.OverlapSphere(transform.position, skeletonAgroRadius, skeletonMask);

        for (int i = 0; i < skeletonInViewRadius.Length; i++)
        {
            Transform skeleton = skeletonInViewRadius[i].transform;
            Vector3 dirToTarget = (skeleton.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < agroSupportAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, skeleton.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    SkeletonsInAgroRange.Add(skeleton);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    protected override void MeleeAttack()
    {
        if (detector.MeleeRangeCheck(attackRanges[(int)AttackTypes.Melee], detector.GetCurTarget()))
        {
            attackUsed = true;
            curAttack = AttackTypes.Melee;
            MyRigid.velocity = Vector3.zero;
        }
    }

    public void ResurrectChance()
    {
        float percentChance = Random.Range(0, 100);
        if (percentChance > ResurrectionChancePercentage)
        {
            Debug.Log("Resurrect");
            Resurrect = true;
        }
    }

    public void SetStateChasing()
    {
        curState = EnemyStates.Aggro;
    }

}