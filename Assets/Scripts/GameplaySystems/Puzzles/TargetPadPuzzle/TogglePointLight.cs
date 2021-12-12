using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePointLight : MonoBehaviour
{
    private Light pointLight;

    public List<Light> _attachedLights;

    void Awake()
    {
        pointLight = GetComponent<Light>();
    }

  
    void Update()
    {
        if (gameObject.tag == "TargetLightGroup_1")
        {

        }
        if (gameObject.tag == "TargetLightGroup_2")
        {

        }
    }
    private void SetLight(bool value)
    {
        foreach (Light pointLight in _attachedLights)
            pointLight.enabled = value;
    }
}
