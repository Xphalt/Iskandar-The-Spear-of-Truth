using System.Collections.Generic;
using UnityEngine;

/*__________________________________________________________________________________
 * This script reveals an iteractable object's tool tip and changes it according to 
 * its type. This was created by Fate, contact me if you need any help.
 *__________________________________________________________________________________*/

public class ToolTip : MonoBehaviour
{
    #region Variables
    private SpriteRenderer SR;
    private Transform player;
    private GamepadTip gamepadTip;
    private int fadeInCounter, fadeOutCounter;
    [HideInInspector] public bool isTalkType = false;//, isReadType;

    public Sprite lookSprite, useSprite, talkSprite;
    public float nearRadius, farRadius;
    [HideInInspector] public bool inRange;
    #endregion

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gamepadTip = FindObjectOfType<GamepadTip>();
    }

    private void Start() 
    { 
        SR.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        //Radius player can interact with object
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, nearRadius);
        //Radius player can see object in distance
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, farRadius);
    }

    private void Update()
    {
        CheckInRange();
        //Sprite always faces camera
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    public void CheckInRange()
    {
        float toolTipRange = Vector3.Distance(player.position, transform.position);

        if (toolTipRange <= farRadius)
        {
            /*_________________________________________________________________________
             * This is for keyboard and mouse UI tips.
             * ________________________________________________________________________*/
            inRange = true;

            if (toolTipRange > nearRadius)
            {
                SR.color = Color.gray;
            }
            else if (toolTipRange <= nearRadius)
            {
                SR.color = Color.white;
            }

            /*_________________________________________________________________________
            * This is for game pad UI tips.
            * ________________________________________________________________________*/
            if (isTalkType)
                gamepadTip.DisplayGamepadUI("talk");
            else if (!isTalkType)
                gamepadTip.DisplayGamepadUI("read");

            //Fade counter necessary so function doesn't restart
            fadeOutCounter = 0;
            fadeInCounter++;

            if (fadeInCounter == 1)
                gamepadTip.FadeIn();
        }
        else
        {
            inRange = false;

            fadeInCounter = 0;
            fadeOutCounter++;
            if (fadeOutCounter == 1)
                gamepadTip.FadeOut();
        }
    }

    public void SetImage(string type)
    {
        switch(type)
        {
            case "Look":
                SR.sprite = lookSprite;
                break;
            case "Use":
                SR.sprite = useSprite;
                break;
            case "Talk":
                SR.sprite = talkSprite;
                break;
        }
    }

    public void Show() { SR.enabled = true; }

    public void Hide() { SR.enabled = false; }
}
