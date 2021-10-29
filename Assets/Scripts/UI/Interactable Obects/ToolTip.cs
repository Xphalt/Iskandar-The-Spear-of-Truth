using UnityEngine;

//This script was created by Fate, contact me if you need any help.

public class ToolTip : MonoBehaviour
{
    public Sprite image; //later create list of sprites
    private SpriteRenderer SR;

    private void Awake()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SR.enabled = false;
      
    }

    //private void OnMouseEnter()
    //{
    //    SetAndShowToolTip();
    //}

    //private void OnMouseExit()
    //{
    //    HideToolTip();
    //}

    public void SetAndShowToolTip()
    {
        SR.sprite = image;
        SR.enabled = true;
 
    }

    public void HideToolTip()
    {
        SR.enabled = false;
    }
}
