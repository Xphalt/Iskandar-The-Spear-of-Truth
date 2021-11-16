using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfExplosive : MonoBehaviour
{
    public float Countdown;
    public float explosionDamage;
    public float explsionRadius;
    public float knockbackSpeed;
    public float duration;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Countdown -= Time.deltaTime;
        if (Countdown <= 0)
        {
            if ((player.transform.position - transform.position).magnitude <= explsionRadius)
            {
                player.GetComponent<PlayerMovement_Jerzy>().KnockBack(transform.position, knockbackSpeed, duration);
                player.GetComponent<PlayerStats>().TakeDamage(explosionDamage, false);
            }
            Destroy(gameObject);
        }
    }
}
