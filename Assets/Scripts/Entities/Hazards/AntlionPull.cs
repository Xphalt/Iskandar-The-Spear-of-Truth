using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntlionPull : MonoBehaviour
{
    [Tooltip("Set to 0 to use Antlion Trap force value")]
    public float force = 0;
    private Rigidbody playerRigidBody;

    void Start()
    {
        if (force == 0) force = GetComponentInParent<AntlionTrap>().force;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement_Jerzy move) && other.TryGetComponent(out PlayerStats stats))
        {
            if (!move.respawning && !move.gettingConsumed && !move.knockedBack && !move.falling && !stats.desertProtection)
            {
                if (playerRigidBody is null) playerRigidBody = other.gameObject.GetComponent<Rigidbody>();
                playerRigidBody.AddForce((transform.position - other.gameObject.transform.position).normalized * force);
            }
        }
    }
}
