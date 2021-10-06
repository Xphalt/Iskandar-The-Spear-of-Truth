using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Interaction : Patrol
{
    public float viewRadius;

    [SerializeField]
    LayerMask targetMask;

    public override void Update()
    {
        base.Update();

        Collider[] playerIsInReach = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        //Patrolling = playerIsInReach.Length == 0;
    }
}
