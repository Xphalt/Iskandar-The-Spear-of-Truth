using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventCheck : MonoBehaviour
{
    public Image img;
    //public Renderer rend;
    public void Start()
    {
        GameEvents.current.onDisableUI += DisableUI;
        GameEvents.current.onEnableUI += EnableUI;

       img = this.GetComponent<Image>();
    }

    public void DisableUI()
    {
        if (this.img != null)
        {
            img.enabled = false;
        }
        gameObject.SetActive(false);
    }

    private void EnableUI()
    {
        if (this.img != null)
        {
            img.enabled = true;
        }
        gameObject.SetActive(true);
    }
}
