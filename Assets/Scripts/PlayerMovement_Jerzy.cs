using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Jerzy : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public GameObject playerModel;
    public float m_Speed;
    public GameObject swordObject;
    public GameObject swordEmpty;
    Animator swordAnimator;
    public GameObject swordDefaultPosition;

    float timeSinceLastAttack;

    float attackCooldown;

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


    void Update()
    {
        
    }

    void FixedUpdate()
    {
        returning = swordEmpty.GetComponent<ThrowSword_Jerzy>().returning;
        thrown = swordEmpty.GetComponent<ThrowSword_Jerzy>().thrown;

        timeSinceLastAttack += Time.deltaTime;

        // basic player movement using input system
        Vector3 m_Input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);

        // set the player to look in the same direction as an analogue stick is pointing
        Quaternion targetRotation = Quaternion.LookRotation(m_Input);
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical")!=0)
        {
            playerModel.transform.rotation = targetRotation;
            swordLookRotation = targetRotation;
        }

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
