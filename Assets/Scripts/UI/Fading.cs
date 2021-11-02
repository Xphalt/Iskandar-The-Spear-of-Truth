// Script made by Jerzy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    enum FadingStates
    {
        In,
        Out
    };

    private Image panel;
    int opacity = 0;
    float targetOpacity;
    public int opacityChange;
    FadingStates targetState;
    bool fading = false;

    public float opacityChangeDelay;
    private float timeSinceOpacityChanged;

    void Start()
    {
        panel = GetComponent<Image>();
    }

    void Update()
    {
        timeSinceOpacityChanged += Time.deltaTime;

        if(fading)
        {
            if(timeSinceOpacityChanged > opacityChangeDelay)
            {
                switch (targetState)
                {
                    case FadingStates.In:
                        if (opacity > targetOpacity)
                        {
                            opacity -= opacityChange;
                            timeSinceOpacityChanged = 0;
                        }
                        else
                            fading = false;

                        break;


                    case FadingStates.Out:
                        if (opacity < targetOpacity)
                        {
                            opacity += opacityChange;
                            timeSinceOpacityChanged = 0;
                        }
                        else
                            fading = false;

                        break;
                }
            }
            if (opacity > 255) opacity = 255;
            if (opacity < 0) opacity = 0;
            panel.color = new Color32(0, 0, 0, (byte)opacity);
        }
    }

    public void FadeIn()
    {
        fading = true;
        targetState = FadingStates.In;
        targetOpacity = 0;
    }

    public void FadeOut()
    {
        fading = true;
        targetState = FadingStates.Out;
        targetOpacity = 255;
    }


}
