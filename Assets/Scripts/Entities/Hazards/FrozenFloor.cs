using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this was written in notepad. Thanks, university! 

public class FrozenFloor : MonoBehaviour
{

	public GameObject player;
	private Vector3 tempdirec;
	public float decreaseRate;
	private PlayerMovement_Jerzy script;

    	void Start()
	{ 
	    script = player.GetComponent<PlayerMovement_Jerzy>();
	}


	private void OnTriggerStay (Collider collision)
	{
		if (collision.transform.tag == "Player")
		{
			if(collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 2.5f)
			{
			    tempdirec = collision.gameObject.GetComponent<Rigidbody>().velocity; 
			    script.Slide(true);
			    collision.gameObject.GetComponent<Rigidbody>().velocity *= decreaseRate; 
			}
			else
			{
 			   script.Slide(false);
			}
			
		}

	}

	private void OnTriggerExit (Collider collision)
	{
	    if (collision.transform.tag == "Player")
		{
	    	    script.Slide(false);
		}
	}

}
