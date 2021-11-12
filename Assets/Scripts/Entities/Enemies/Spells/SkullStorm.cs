using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullStorm : MonoBehaviour
{
    /// <summary>
    /// Daniel, let's set the variables of this projectile directly in the Phase2 script when we spawn them in,
    /// the more I wrote I found that we didn't really need the ProjectileScript since it only held the lifetime
    /// stuff, which we wont need for this one.
    /// -Bernardo
    /// </summary>
    private float seekTimer, skullDmg, skullSpeed, seekDelay;
    private bool seeking = false;
    private Transform playerPos;
    private Rigidbody myRigid;
    private Vector3 wanderDir;

    public void Start()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (!seeking)
        {
            seekTimer += Time.deltaTime;
            if (seekTimer >= seekDelay)
            {
                seeking = true;
                seekTimer = 0;
            }

            Wander();
        }
        else
            Seek();
    }

    private void Wander()
    {
        //move to random direction given by boss
        transform.rotation = Quaternion.LookRotation(wanderDir);
        myRigid.velocity = wanderDir;
    }

    private void Seek()
    {
        //move to player position given by boss
        Vector3.MoveTowards(transform.position,playerPos.position,skullSpeed);
    }

    public void SetVariables(Transform _target, Vector3 _direction, float _damage, float _speed, float _seekDelay)
    {
        playerPos = _target;
        wanderDir = _direction;
        skullDmg = _damage;
        skullSpeed = _speed;
        seekDelay = _seekDelay;
    }
}
