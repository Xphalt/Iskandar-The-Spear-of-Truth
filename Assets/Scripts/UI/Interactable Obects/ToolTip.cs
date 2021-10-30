using System.Collections.Generic;
using UnityEngine;

/*__________________________________________________________________________________
 * This script reveals an iteractable object's tool tip and changes it according to 
 * its type. This was created by Fate, contact me if you need any help.
 *__________________________________________________________________________________*/

public class ToolTip : MonoBehaviour
{
    private SpriteRenderer SR;
    private static int look = 0, use = 1, talk = 2; //Add list elements in Inspector in this order
    public List<Sprite> toolTipImages = new List<Sprite>();

    private void Awake()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start() { SR.enabled = false; }

    public void SetImage(string type)
    {
        switch(type)
        {
            case "Look":
                this.SR.sprite = toolTipImages[look];
                break;
            case "Use":
                this.SR.sprite = toolTipImages[use];
                break;
            case "Talk":
                this.SR.sprite = toolTipImages[talk];
                break;
        }
    }

    public void Show() { SR.enabled = true; }

    public void Hide() { SR.enabled = false; }
}
