using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Jerzy : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public GameObject playerModel;
    public float m_Speed;


    public Quaternion swordLookRotation;

    public bool canBeDamaged = true;

    public float dashCooldown;
    public float invincibilityFramesAfterDash;
    public float dashForce;
    public float dashAnalogueReq;

    float timeSinceLastDash = 0;

    float timeInteractHeld = 0;
    const float HELD_TIME_FOR_THROWN_ATTACK = 0.4f;

    private Player_Targeting_Jack _playerTargetingScript;
    private Transform _targetedTransform = null;

    float lastMagnitudeFromTarget = 0;


    private Player_Interaction_Jack _playerInteractionScript;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        _playerTargetingScript = GetComponent<Player_Targeting_Jack>();
        _playerInteractionScript = GetComponent<Player_Interaction_Jack>();
    }


    void FixedUpdate()
    {

        timeSinceLastDash += Time.deltaTime;

        // basic player movement using input system
        Vector3 m_Input = new Vector3 (Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (_playerTargetingScript.IsTargeting())
        {
            // Set player rotation to look at targeted object
            Vector3 playerToTargetVector = new Vector3(_targetedTransform.position.x - transform.position.x,
                                0.0f,
                                _targetedTransform.position.z - transform.position.z);

            playerModel.transform.rotation = Quaternion.LookRotation(playerToTargetVector);

            //print(playerToTargetVector.magnitude);


            Vector3 direction = playerModel.transform.TransformDirection(m_Input);
            direction.Normalize();


            // this block of code ensures that the player does not spiral away from the targeted enemy
            if (m_Input.x == 0)
            {
                lastMagnitudeFromTarget = playerToTargetVector.magnitude;
            }
            if(playerToTargetVector.magnitude > lastMagnitudeFromTarget)
            {
                m_Rigidbody.AddForce(playerModel.transform.forward * m_Speed*5);
            }

            


            m_Rigidbody.velocity = (direction * m_Speed);

            if (Input.GetAxis("Dash") > 0 && timeSinceLastDash >= dashCooldown)
            {

                Dash(direction);

            }

            // this line fixes the thrown sword direction when locked onto an enemy
            swordLookRotation = Quaternion.LookRotation(playerToTargetVector);

        }
        else
        {
            m_Input.Normalize();
            m_Rigidbody.velocity = ( m_Input * m_Speed);

            if (timeSinceLastDash >= invincibilityFramesAfterDash)
            {
                canBeDamaged = true;
            }

            if (Input.GetAxis("Dash") > 0 && timeSinceLastDash >= dashCooldown)
            {

                Dash(m_Input);

            }

            // set the player to look in the same direction as an analogue stick is pointing
            Quaternion targetRotation = Quaternion.LookRotation(m_Input);
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                playerModel.transform.rotation = targetRotation;
                swordLookRotation = targetRotation;
            }
        }
       


        // combat - interact key is [Controller X] / [Mouse 1]
        if(Input.GetAxis("Interact") > 0)
        {
            timeInteractHeld += Time.deltaTime;
        }
        else if (timeInteractHeld > 0)
        {
            if(_playerInteractionScript.IsInteractionAvailable())
            {
                _playerInteractionScript.Interact();
            }

            else
            {

                if (timeInteractHeld > HELD_TIME_FOR_THROWN_ATTACK)
                {
                    GetComponent<PlayerCombat_Jerzy>().ThrowAttack();
                }
                else
                {
                    GetComponent<PlayerCombat_Jerzy>().Attack();
                }

            }

            timeInteractHeld = 0;
        }


    }

    void Dash(Vector3 dashDirection)
    {
        if (Mathf.Abs(dashDirection.x) > dashAnalogueReq || Mathf.Abs(dashDirection.z) > dashAnalogueReq)
        {
            canBeDamaged = false;
            m_Rigidbody.AddForce(dashDirection * dashForce);
            timeSinceLastDash = 0;

        }
    }



    public void SetTargetedTransform(Transform newTargetedTransform)
    {
        _targetedTransform = newTargetedTransform;
    }
}
