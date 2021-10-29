using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public Sprite image;

    private void OnMouseEnter()
    {
        ToolTipManager.instance.SetAndShowToolTip();
    }

    private void OnMouseExit()
    {
        ToolTipManager.instance.HideToolTip();
    }
}
