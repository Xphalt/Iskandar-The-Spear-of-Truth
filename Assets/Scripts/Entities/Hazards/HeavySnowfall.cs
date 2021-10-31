using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySnowfall : MonoBehaviour
{

    //some are initially set to public to allow for balancing
    private float CurrentTimer;
    public GameObject player;
    public float StartDash, StartSpeed;

    public int Movement_Debuff = 5;
    public float FrostTicks = 6;
    public float CurrentFrostTicks;
    public bool Frosted;

    public PlayerMovement_Jerzy playerMovement;

    public float AreaInterval = 0.5f;
    public int DOT_DEBUFF;

    
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement_Jerzy>();
        CurrentFrostTicks = FrostTicks;
        StartDash = playerMovement.dashCooldown;
        StartSpeed = playerMovement.m_Speed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("Snowfall is heavy"); 
            if (!Frosted)
            {
                CurrentTimer += Time.deltaTime;
                if (CurrentTimer > AreaInterval)
                {
                    DOT_DEBUFF += 1;
                    CurrentTimer = 0;
                }
                if (DOT_DEBUFF == 10)
                {
                    Debug.Log("Affected by frost blight");
                    Frosted = true;
                    playerMovement.m_Speed -= Movement_Debuff;
                    playerMovement.dashCooldown += 1;
                }
            }            
                   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (Frosted)
            {
                CurrentTimer += Time.deltaTime;

                if (CurrentTimer > AreaInterval)
                {
                    CurrentFrostTicks--;
                    
                    CurrentTimer = 0;
                    Frosted = !(CurrentFrostTicks == 0);
                }

                if (CurrentFrostTicks == 0)
                {
                    DOT_DEBUFF = 0;
                    Frosted = false;
                    playerMovement.dashCooldown = StartDash;
                    playerMovement.m_Speed = StartSpeed;
                }
            }
        }
        
    }

}

