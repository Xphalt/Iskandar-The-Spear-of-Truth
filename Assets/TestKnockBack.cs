using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKnockBack : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        collision.gameObject.GetComponent<PlayerMovement_Jerzy>().KnockBack(transform.position,70,0.3f);
    }
}
