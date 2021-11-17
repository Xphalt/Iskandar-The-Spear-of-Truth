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

    public void Summon(Vector3 pos, float dmg, float speed)
    {
        transform.position = pos;
        damage = dmg;
        movementSpeed = speed;

        transform.SetParent(null);
        gameObject.SetActive(true);

        Vector2 direction = ExtraFunctions.RandomVector2(speed);
        myRigid.velocity = new Vector3(direction.x, 0, direction.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement_Jerzy move) && other.TryGetComponent(out PlayerStats stats))
        {
            if (!stats.desertProtection)
            {
                move.KnockBack(transform.position, knockbackForce, knockbackDuration, verticalVel:knockbackForce / 2);
                stats.TakeDamage(damage);
            }
        }
    }
}
