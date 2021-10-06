using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Rigidbody rigi;
    private GameObject player;

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //every frame, set location of camera to player's 

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == ("CameraWall"))
        {
            rigi.velocity = Vector3.zero;
        }
        else
        {
            Physics.IgnoreCollision(collision.transform.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
