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



    private Player_Targeting_Jack _playerTargetingScript;
    private Transform _targetedTransform = null;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        swordAnimator = swordObject.GetComponent<Animator>();
        attackCooldown = GetComponent<PlayerStats_Jerzy>().attackCooldown;
        _playerTargetingScript = GetComponent<Player_Targeting_Jack>();
    }


    void Update()
    {
        
    }

    void FixedUpdate()
    {
        returning = swordEmpty.GetComponent<ThrowSword_Jerzy>().returning;
        thrown = swordEmpty.GetComponent<ThrowSword_Jerzy>().thrown;

        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastDash += Time.deltaTime;

        // basic player movement using input system
        Vector3 m_Input = new Vector3 (Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if(_playerTargetingScript.IsTargeting())
        {
            /////       v1      /////
            // Move relative to targeted object
            //Quaternion targetRotation = _targetedTransform.localRotation;

            //if(m_Input.x >= 0)
            //{
            //    m_Input += targetRotation * Vector3.right;
            //}
            //else
            //{
            //    m_Input += targetRotation * Vector3.left;
            //}

            //m_Input.Normalize();

            ////Vector3 offset = transform.position - _targetedTransform.position;

            //m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);





            // Set player rotation to look at targeted object
            Vector3 playerToTargetVector = new Vector3(_targetedTransform.position.x - transform.position.x,
                                0.0f,
                                _targetedTransform.position.z - transform.position.z);

            playerModel.transform.rotation = Quaternion.LookRotation(playerToTargetVector);





            /////       v2      /////
            Vector3 direction = transform.TransformDirection(m_Input);
            direction.Normalize();

            m_Rigidbody.MovePosition(m_Rigidbody.position + direction * Time.deltaTime * m_Speed);





            /////       v3      /////
            //Vector3 direction =  transform.right * m_Input.x;
            //direction += transform.forward * m_Input.z;

            //direction.Normalize();

            //m_Rigidbody.MovePosition(transform.position + direction * Time.deltaTime * m_Speed);





            /////      v4      /////
            //Vector3 direction = transform.InverseTransformDirection(m_Input);
            //direction.Normalize();

            //m_Rigidbody.MovePosition(m_Rigidbody.position + m_Input * Time.deltaTime * m_Speed);




            /////       v5      /////
            //Vector3 localRightVec = transform.TransformDirection(transform.right);
            //print("localRightVec = " + localRightVec);
            //Vector3 localForwardVec = transform.TransformDirection(transform.forward);
            //print("localForwardVec = " + localForwardVec);

            //m_Input.Normalize();

            //localRightVec *= m_Input.x;
            //localForwardVec *= m_Input.z;

            //Vector3 direction = localForwardVec + localRightVec;
            //direction.Normalize();

            //print(direction);

            //m_Rigidbody.MovePosition(transform.position + direction * Time.deltaTime * m_Speed);


		}
        else
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
        if(canAttack)
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

    public void TakeDamage(int amt)
    {
        if(canBeDamaged)
        {
            GetComponent<PlayerStats_Jerzy>().health -= amt;
            // anything that happens to the player when taking damage happens here

            if(GetComponent<PlayerStats_Jerzy>().health <= 0)
            {
                // player dies
            }
        }
    }

    public void SetTargetedTransform(Transform newTargetedTransform)
    {
        _targetedTransform = newTargetedTransform;
	}
}
