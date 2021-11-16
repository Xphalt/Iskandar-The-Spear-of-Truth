using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FinalBurst : MonoBehaviour
{
    public GameObject transformInto;
    public bool transformFirst = false;

    public void Burst()
    {
        GameObject parent = transform.parent.gameObject;

        ParticleSystem explosion = GetComponent<ParticleSystem>();
        transform.SetParent(null);
        gameObject.SetActive(true);
        parent.SetActive(false);

        if (transformFirst) Transformation();

        Destroy(gameObject, explosion.main.duration);
    }

    private void Transformation()
    {
        if (transformInto)
        {
            transformInto.transform.position = transform.position;
            transformInto.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (!transformFirst) Transformation();
    }
}
