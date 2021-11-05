using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Jerzy2 : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public GameObject playerModel;
    public float m_Speed;
    public GameObject swordObject;
    public GameObject swordEmpty;
    public GameObject swordDefaultPosition;

    private Animator swordAnimator;

    bool canInteract = false;

    float timeSinceLastAttack;

    float attackCooldown;

    public bool canBeDamaged = true;
    public bool canAttack = true;

    public float dashCooldown;
    public float attackCooldownAfterDash;
    public float invincibilityFramesAfterDash;
    public float dashForce;
    public float dashAnalogueReq;

    float timeSinceLastDash = 0;

    Quaternion swordLookRotation;

    bool returning;
    bool thrown;

    private float timeInteractHeld = 0f;
    public float timeToThrowSword;
    private bool isAttacking = false;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        swordAnimator = swordObject.GetComponent<Animator>();
        attackCooldown = GetComponent<PlayerStats_Jerzy>().attackCooldown;
    }

    void FixedUpdate()
    {
        returning = swordEmpty.GetComponent<ThrowSword_Jerzy>().returning;
        thrown = swordEmpty.GetComponent<ThrowSword_Jerzy>().thrown;

        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastDash += Time.deltaTime;

        // basic player movement using input system
        Vector3 m_Input = new Vector3 (Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        
        if(true)
        {
            // Move normally
            m_Input.Normalize();

            m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);

            if (timeSinceLastDash >= attackCooldownAfterDash)
            {
                canAttack = true;
            }
            if (timeSinceLastDash >= invincibilityFramesAfterDash)
            {
                canBeDamaged = true;
            }

            if (Input.GetAxis("Dash") > 0 && timeSinceLastDash >= dashCooldown)
            {
                if (Mathf.Abs(m_Input.x) > dashAnalogueReq || Mathf.Abs(m_Input.z) > dashAnalogueReq)
                {
                    // dash
                    canAttack = false;
                    canBeDamaged = false;
                    m_Rigidbody.AddForce(m_Input * dashForce);
                    timeSinceLastDash = 0;
                }

            }

            // set the player to look in the same direction as an analogue stick is pointing
            Quaternion targetRotation = Quaternion.LookRotation(m_Input);
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                playerModel.transform.rotation = targetRotation;
                swordLookRotation = targetRotation;
            }

        }


            // canAttack also includes interacting
            if (canAttack)
            {
                if (!canInteract)
                {
                    // if the interact button is held, begin counter to determine if melee or thrown attack
                    if (Input.GetAxis("Interact") > 0)
                    {
                        if (timeSinceLastAttack >= attackCooldown)
                        {
                            isAttacking = true;
                        }
                        timeInteractHeld += Time.deltaTime;

                    }
                    // the interact button is no longer held
                    else
                    {
                        if (isAttacking)
                        {
                            // if interact has been held long enough to do a throw attack, then throw the sword
                            if (timeInteractHeld >= timeToThrowSword)
                            {
                                swordEmpty.GetComponent<ThrowSword_Jerzy>().ThrowSword(swordLookRotation);
                            }
                            // otherwise perform a simple swing attack
                            else
                            {
                                swordAnimator.Play("PlayerSwordSwing");
                                timeSinceLastAttack = 0;
                            }
                        }

                        timeInteractHeld = 0;
                        isAttacking = false;
                    }
                    // prevent player from attacking whilst the sword is mid-air
                    if (thrown)
                    {
                        timeSinceLastAttack = 0;
                    }
                }
                else
                {
                    // interact code goes here
                }
            }
        
    }

    private void OnTriggerStay(Collider other)
    {
        // when the sword returns to the player
        if (other.tag == "playerSword" && returning && thrown)
        {
            // end throw cycle, attach sword to player, set appropriate position and rotation for the sword
            swordEmpty.GetComponent<ThrowSword_Jerzy>().EndThrowCycle();
            swordEmpty.transform.parent = playerModel.transform;
            swordEmpty.transform.position = swordDefaultPosition.transform.position;
            swordEmpty.transform.rotation = swordDefaultPosition.transform.rotation;
            timeSinceLastAttack = 0;
        }
    }

}
