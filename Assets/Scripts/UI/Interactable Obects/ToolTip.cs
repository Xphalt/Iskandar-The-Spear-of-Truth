using System.Collections.Generic;
using UnityEngine;

/*__________________________________________________________________________________
 * This script reveals an iteractable object's tool tip and changes it according to 
 * its type. This was created by Fate, contact me if you need any help.
 *__________________________________________________________________________________*/

public class ToolTip : MonoBehaviour
{
    private SpriteRenderer SR;
    private Transform player;
    //private static int look = 0, use = 1, talk = 2; //Add list elements in Inspector in this order
    //public List<Sprite> toolTipImages = new List<Sprite>();
    public Sprite lookSprite, useSprite, talkSprite;
    public float nearRadius, farRadius;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        float toolTipRange = Vector3.Distance(player.position, transform.position);


        if (toolTipRange <= farRadius && toolTipRange > nearRadius)
        {
            print("far range");
            SR.color = Color.gray;
        }
        else if (toolTipRange <= nearRadius)
        {
            print("near range");
            SR.color = Color.white;
        }
        else
            print("not in range");
    }

    public void SetImage(string type)
    {
        switch(type)
        {
            case "Look":
                //SR.sprite = toolTipImages[look];
                SR.sprite = lookSprite;
                break;
            case "Use":
                // SR.sprite = toolTipImages[use];
                SR.sprite = useSprite;
                break;
            case "Talk":
                // SR.sprite = toolTipImages[talk];
                SR.sprite = talkSprite;
                break;
        }
    }

    public void Show() { SR.enabled = true; }

    public void Hide() { SR.enabled = false; }
}
