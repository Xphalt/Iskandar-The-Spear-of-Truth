using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrap : MonoBehaviour
{
    public float rootDuration;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement_Jerzy>().Root(rootDuration);
        }
    }
}
