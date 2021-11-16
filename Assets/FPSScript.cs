using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSScript : MonoBehaviour
{
    private float fps;
    public Text text;

    private void Start()
    {
        Application.targetFrameRate = 100;
        InvokeRepeating(nameof(GetFPS), 1, 1);
    }

    private void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        text.text = fps + " FPS";
    }
}
