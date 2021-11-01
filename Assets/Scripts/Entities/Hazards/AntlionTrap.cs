using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntlionTrap : MonoBehaviour
{
    public float force;
    public float trapRadius;
    public float damage;
    public float consumeDuration;
    public float consumeMoveAmt;
    public GameObject respawnObject;
    public GameObject pullObject;
    private Vector3 respawnPosition;

    void Start()
    {
        pullObject.GetComponent<SphereCollider>().radius = trapRadius;
        respawnPosition = respawnObject.transform.position;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement_Jerzy>().GetConsumed(respawnPosition,0,damage,consumeDuration, consumeMoveAmt);

        }
    }
}
