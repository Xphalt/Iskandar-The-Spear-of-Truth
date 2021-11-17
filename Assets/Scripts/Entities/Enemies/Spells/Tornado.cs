using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Tornado : MonoBehaviour
{
    public float knockbackForce, knockbackDuration, raycastDistance;
    private float damage, movementSpeed;

    private Rigidbody myRigid;

    private void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, myRigid.velocity, raycastDistance))
        {
            Vector3 newVelocity = Vector3.Cross(myRigid.velocity, Vector3.up).normalized * movementSpeed;
            if (Random.Range(0, 2) == 1) newVelocity *= -1;

            myRigid.velocity = newVelocity;
        }
    }

    public void Summon(float _damage, float _speed)
    {
        damage = _damage;
        movementSpeed = _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement_Jerzy move))
        {
            move.KnockBack(transform.position, knockbackForce, knockbackDuration);
            move.Launch(knockbackForce);
            move.GetComponent<PlayerStats>().TakeDamage(damage);
        }
    }
}
