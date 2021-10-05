using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerContoller : MonoBehaviour
{
    Player_Interaction_Jack playerInteractionScript;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInteractionScript = GetComponent<Player_Interaction_Jack>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerInteractionScript.Interact();
		}
        else if(Input.GetKeyDown(KeyCode.T))
        {
            print("interaction available = " + playerInteractionScript.IsInteractionAvailable());
		}
    }
}
