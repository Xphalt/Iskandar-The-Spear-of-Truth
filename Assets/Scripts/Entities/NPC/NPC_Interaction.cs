using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Interaction : Patrol
{
    public float ViewRadius;

    [SerializeField] private LayerMask _targetMask;

    public override void Update()
    {
        base.Update();

        Collider[] playerIsInReach = Physics.OverlapSphere(transform.position, ViewRadius, _targetMask);

        Patrolling = playerIsInReach.Length == 0;
    }
}
