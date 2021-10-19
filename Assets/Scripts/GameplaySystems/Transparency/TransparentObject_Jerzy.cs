using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject_Jerzy : MonoBehaviour
{
    float transparencyAmount = 1f;
    private MeshRenderer meshRend;

    public float TRANSPARENCY_SPEED = 0.03f;
    public float MIN_TRANSPARENCY_PER_LAYER = 0.5f;
    private const float MAX_TRANSPARENCY = 1;

    private float timeSinceLastInWay = 0f;

    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
        timeSinceLastInWay += Time.deltaTime;
        meshRend.material.color = new Color(meshRend.material.color.r, meshRend.material.color.g, meshRend.material.color.b, transparencyAmount);

        if(transparencyAmount < MAX_TRANSPARENCY && timeSinceLastInWay >= 0.1f)
            transparencyAmount += TRANSPARENCY_SPEED;

        if(transparencyAmount >= MAX_TRANSPARENCY)
        {
            meshRend.material.SetOverrideTag("RenderType", "");
            meshRend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            meshRend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            meshRend.material.SetInt("_ZWrite", 1);
            meshRend.material.DisableKeyword("_ALPHATEST_ON");
            meshRend.material.DisableKeyword("_ALPHABLEND_ON");
            meshRend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            meshRend.material.renderQueue = -1;
        }
        else
        {
            meshRend.material.SetOverrideTag("RenderType", "Transparent");
            meshRend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            meshRend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            meshRend.material.SetInt("_ZWrite", 0);
            meshRend.material.DisableKeyword("_ALPHATEST_ON");
            meshRend.material.EnableKeyword("_ALPHABLEND_ON");
            meshRend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            meshRend.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
    }

    public void MakeTransparent(float objectInList)
    {
        if(transparencyAmount > (Mathf.Pow(MIN_TRANSPARENCY_PER_LAYER, objectInList)))
        {
            transparencyAmount -= TRANSPARENCY_SPEED * 2;
        }
        timeSinceLastInWay = 0;
    }


}
