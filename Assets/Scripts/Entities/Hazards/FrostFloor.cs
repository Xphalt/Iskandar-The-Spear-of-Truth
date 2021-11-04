using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostFloor : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement_Jerzy move;
    private Vector3 tempDirec, veloc;

    public float CurrentTimer;
    private int DOT_DEBUFF;
    private float AreaInterval = 0.8f;

    void Start()
    {
        veloc = player.GetComponent<Rigidbody>().velocity;
        move = player.GetComponent<PlayerMovement_Jerzy>();
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (veloc.magnitude == 0)
            {
                CurrentTimer += Time.deltaTime;
                veloc = tempDirec;
                if (CurrentTimer > AreaInterval)
                {
                    DOT_DEBUFF += 1;
                    CurrentTimer = 0;
                    if (DOT_DEBUFF == 8)
                    {
                        veloc = Vector3.zero;
                    }
                }           
            }
            else if (veloc.magnitude != 0) 
            {
                tempDirec = veloc;
                CurrentTimer = 0;
            }
        }
    }
}
