// Script made by Jerzy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomSpitter : MonoBehaviour
{
    public float shootDelay;
    public float venomSpeed;
    public float range;
    public GameObject VenomBulletPrefab;
    private float timeSinceLastShot;
    private Animator anim;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= shootDelay)
        {
            foreach (Collider hit in Physics.OverlapSphere(transform.position, range))
            {
                if (hit.CompareTag("Player"))
                {
                    anim.Play("VenomSpitter");
                    timeSinceLastShot = 0;
                    GameObject venomShot = Instantiate(VenomBulletPrefab, transform.position, Quaternion.identity);
                    Vector3 playerPos = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                    //playerPos.y = transform.y;
                    Vector3 direction = (playerPos - transform.position).normalized;
                    venomShot.GetComponent<VenomShot>().SetDirection(direction, venomSpeed, playerPos);
                    Physics.IgnoreCollision(GetComponent<Collider>(), venomShot.GetComponent<Collider>());

                    break;
                }
            }
        }
    }
}
