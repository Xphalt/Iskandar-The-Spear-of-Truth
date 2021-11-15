using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<StatsInterface>(out _))
        {
            FinalBurst explosion = transform.GetComponentInChildren<FinalBurst>(true);
            if (explosion) explosion.Burst();
        }
    }
}
