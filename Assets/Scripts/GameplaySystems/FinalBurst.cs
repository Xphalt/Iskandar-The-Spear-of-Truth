using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FinalBurst : MonoBehaviour
{
    public void Burst()
    {
        GameObject parent = transform.parent.gameObject;

        ParticleSystem explosion = GetComponent<ParticleSystem>();
        transform.SetParent(null);
        gameObject.SetActive(true);
        parent.SetActive(false);

        Destroy(gameObject, explosion.main.duration);
    }
}
