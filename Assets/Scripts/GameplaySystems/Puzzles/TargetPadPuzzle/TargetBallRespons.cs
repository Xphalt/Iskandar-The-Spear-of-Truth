using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBallRespons : MonoBehaviour
{
    public GameObject targetLight, targetPad;
    private TargetPad_Jack targetPadScript;
    private bool hasTriggered;
    void Awake()
    {
        targetPadScript = targetPad.GetComponent<TargetPad_Jack>();
    }
    void Update()
    {
        hasTriggered = targetPadScript.hasTriggered;

        if (hasTriggered)
        {
            targetLight.SetActive(true);
        }
        else targetLight.SetActive(false);
    }
}
