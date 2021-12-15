using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/*__________________________________________________________________________________
 * This script reveals an iteractable object's tool tip and changes it according to 
 * its type. This was created by Fate, contact me if you need any help.
 *__________________________________________________________________________________*/

public class ToolTip : MonoBehaviour
{
    #region Variables
    private Transform player;
    private GamepadTip gamepadTip;
    private int fadeInCounter, fadeOutCounter;
    [HideInInspector] public bool isTalkType = false;

    public GameObject questionmarkParticles;
    public float nearRadius, farRadius;
    [HideInInspector] public bool inRange;
    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gamepadTip = FindObjectOfType<GamepadTip>();
    }

    private void Start() 
    {
        questionmarkParticles.SetActive(false);
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
        //transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    public void CheckInRange()
    {
        bool gamepadInUse = UIManager.instance.GetCurrentInput() == UIManager.INPUT_OPTIONS.GAMEPAD;
        float toolTipRange = Vector3.Distance(player.position, transform.position);

        if (toolTipRange <= farRadius)
        {
            /*_________________________________________________________________________
             * This is for keyboard and mouse UI tips.
             * ________________________________________________________________________*/
            inRange = true;

            //if (toolTipRange > nearRadius)
            //{
            //    SR.color = Color.gray;
            //}
            //else if (toolTipRange <= nearRadius)
            //{
            //    SR.color = Color.white;
            //}

            /*_________________________________________________________________________
            * This is for game pad UI tips.
            * ________________________________________________________________________*/
            if (gamepadInUse)
            {
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
        }
        else if (gamepadInUse)
        {
            inRange = false;

            fadeInCounter = 0;
            fadeOutCounter++;
            if (fadeOutCounter == 1)
                gamepadTip.FadeOut();
        }
    }

    //public void SetImage(string type)
    //{
    //    switch(type)
    //    {
    //        case "Look":
    //            MR. = lookParticles;
    //            break;
    //        case "Use":
    //            SR.sprite = useParticles;
    //            break;
    //        case "Talk":
    //            SR.sprite = talkParticles;
    //            break;
    //    }
    //}

    public void Show() { questionmarkParticles.SetActive(true); }

    public void Hide() { questionmarkParticles.SetActive(false); }
}