using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandAid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //stop bleed the fucking player
        if (other.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.bleeding = false;
            gameObject.SetActive(false);
        }
    }
}
