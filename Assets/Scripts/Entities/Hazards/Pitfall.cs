// Script made by Jerzy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour
{
    public float damage;
    public float respawnDuration;
    public GameObject respawnObject;
    private Vector3 respawnPosition;

    void Start()
    {
        respawnPosition = respawnObject.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement_Jerzy>().Respawn(respawnPosition,respawnDuration,damage);
        }
    }

}
