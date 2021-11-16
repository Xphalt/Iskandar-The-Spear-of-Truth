using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinalBurst explosion = transform.GetComponentInChildren<FinalBurst>(true);
            if (explosion) explosion.Burst();
        }
    }
}
