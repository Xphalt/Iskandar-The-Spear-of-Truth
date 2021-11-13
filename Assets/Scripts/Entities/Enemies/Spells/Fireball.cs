using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExtraFunctions;

public class Fireball : MonoBehaviour
{
    private Vector3 start, end, centre, rotVec;
    private Rigidbody myRigid;

    private float timer = 0, duration, damage, radius, knockback, knockbackDuration;
    private bool moving = false;

    public Transform indicator;

    public bool destroyOnEnd = true;
    public float arcSize = 5, rotSpeed = 360;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        rotVec = RandomVector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                if (destroyOnEnd) Impact(true);
                else moving = false;
            }
            else
            {
                myRigid.MovePosition(Vector3.Slerp(start - centre, end - centre, timer / duration) + centre);
                transform.Rotate(rotSpeed * Time.deltaTime * rotVec);
            }
        }
    }

    public void Launch(Vector3 _start, Vector3 _end, float _speed, float _damage, float _radius, float _knockback = 0, float _kbDuration = 0)
    {
        start = _start;
        end = _end;
        centre = Vector3.Lerp(start, end, 0.5f) - Vector3.up * arcSize;
        damage = _damage;
        radius = _radius;
        knockback = _knockback;
        knockbackDuration = _kbDuration;
        duration = (end - start).magnitude / _speed;

        indicator.position = end;
        indicator.SetParent(null);
        indicator.localScale = new Vector3(radius * 2, indicator.localScale.y, radius * 2);
        indicator.gameObject.SetActive(true);

        moving = true;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats player))
        {
            player.TakeDamage(damage);
            player.GetComponent<PlayerMovement_Jerzy>().KnockBack(transform.position, knockback, knockbackDuration);
            Impact();
        }
    }

    private void Impact(bool dealDamage = false)
    {
        if (dealDamage)
            foreach (Collider c in Physics.OverlapSphere(transform.position, radius)) OnTriggerEnter(c);
        
        float destroyIn = 0;
        ParticleSystem explosion = transform.GetComponentInChildren<ParticleSystem>(true);
        if (transform.GetComponentInChildren<ParticleSystem>(true))
        {
            explosion.gameObject.SetActive(true);
            explosion.transform.SetParent(null);
            gameObject.SetActive(false);
            destroyIn = explosion.main.duration;
        }

        indicator.gameObject.SetActive(false);
        Destroy(gameObject, destroyIn);
    }
}
