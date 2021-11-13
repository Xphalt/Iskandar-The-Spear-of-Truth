using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullStorm : MonoBehaviour
{
    /// <summary>
    /// Daniel, let's set the variables of this projectile directly in the Phase2 script when we spawn them in,
    /// the more I wrote I found that we didn't really need the ProjectileScript since it only held the lifetime
    /// stuff, which we wont need for this one.
    /// -Bernardo xx
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
        else Seek();
        transform.rotation = Quaternion.LookRotation(myRigid.velocity);
    }

    private void Wander()
    {
        //move to random direction given by boss
        myRigid.velocity = wanderDir * skullSpeed;
    }

    private void Seek()
    {
        //move to player position given by boss
        myRigid.velocity = (playerPos.position - transform.position).normalized * skullSpeed;
    }

    public void SetVariables(Transform _target, float _damage, float _speed, float _seekDelay, Vector3 _direction = new Vector3())
    {
        playerPos = _target;
        skullDmg = _damage;
        skullSpeed = _speed;
        seekDelay = _seekDelay;
        wanderDir = _direction == new Vector3() ? transform.forward : _direction;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats player))
        {
            player.TakeDamage(skullDmg);
            Destroy(gameObject);
        }
    }
}
