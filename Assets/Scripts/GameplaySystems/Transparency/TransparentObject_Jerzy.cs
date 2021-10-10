using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject_Jerzy : MonoBehaviour
{
    float transparencyAmount = 1f;
    private MeshRenderer meshRend;

    private const float TRANSPARENCY_SPEED = 0.03f;
    private const float MIN_TRANSPARENCY_PER_LAYER = 0.5f;
    private const float MAX_TRANSPARENCY = 1;

    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {    
        meshRend.material.color = new Color(meshRend.material.color.r, meshRend.material.color.g, meshRend.material.color.b, transparencyAmount);

        if(transparencyAmount < MAX_TRANSPARENCY)
            transparencyAmount += TRANSPARENCY_SPEED;
    }

    public void MakeTransparent(float objectInList)
    {
        if(transparencyAmount > (Mathf.Pow(MIN_TRANSPARENCY_PER_LAYER, objectInList)))
            transparencyAmount -= TRANSPARENCY_SPEED*2;
    }

}
