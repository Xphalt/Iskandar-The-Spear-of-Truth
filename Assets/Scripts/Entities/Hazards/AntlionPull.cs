using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntlionPull : MonoBehaviour
{
    private float force;
    private Rigidbody playerRigidBody;

    void Start()
    {
        force = GetComponentInParent<AntlionTrap>().force;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement_Jerzy move) && other.TryGetComponent(out PlayerStats stats))
        {
            if (!move.respawning && !move.gettingConsumed && !stats.desertProtection)
            {
                if (playerRigidBody is null)
                {
                    playerRigidBody = other.gameObject.GetComponent<Rigidbody>();
                }
                playerRigidBody.AddForce((transform.position - other.gameObject.transform.position).normalized * force);
            }
        }
    }
}
