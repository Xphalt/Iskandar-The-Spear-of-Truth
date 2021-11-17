// Script made by Jerzy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomSpitter : MonoBehaviour
{
    public float shootDelay;
    public float venomSpeed;
    public GameObject VenomBulletPrefab;
    private float timeSinceLastShot;
    private Animator anim;

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && timeSinceLastShot >= shootDelay)
        {
            anim.Play("VenomSpitter");
            timeSinceLastShot = 0;
            GameObject venomShot = Instantiate(VenomBulletPrefab, transform.position, Quaternion.identity);
            Vector3 playerPos = new Vector3(other.gameObject.transform.position.x,transform.position.y,other.gameObject.transform.position.z);
            //playerPos.y = transform.y;
            Vector3 direction = (playerPos - transform.position).normalized;
            venomShot.GetComponent<VenomShot>().SetDirection(direction,venomSpeed,playerPos);
        }
    }
}
