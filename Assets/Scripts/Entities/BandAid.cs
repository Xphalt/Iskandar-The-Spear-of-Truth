using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BandAid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //stop bleed the fucking player
        if (other.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.bleeding = false;
            playerStats.playerBleedEffect.SetActive(false);
            playerStats.bleedHealthColour.gameObject.GetComponent<Image>().color = new Color32(156, 8, 8, 255);
            gameObject.SetActive(false);
        }
    }
}
