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
    List<GameObject> listOfInteractablesWithinRange;
    GameObject closestInteractable;

    public float throwTimeBeforeSpinInPlace;
    public float throwTimeSpinningInPlace;
    public float throwSpeed;
    public float throwReturnSpeed;

    public float interactCooldown;

    public GameObject interactableIcon;

    bool canInteract = false;

    float timeSinceLastInteract;

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
        listOfInteractablesWithinRange = new List<GameObject>();
        _playerTargetingScript = GetComponent<Player_Targeting_Jack>();
    }


    void FixedUpdate()
    {


        returning = swordEmpty.GetComponent<ThrowSword_Jerzy>().returning;
        thrown = swordEmpty.GetComponent<ThrowSword_Jerzy>().thrown;

        timeSinceLastInteract += Time.deltaTime;
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

            print(playerToTargetVector.magnitude);



            Vector3 direction = playerModel.transform.TransformDirection(m_Input);
            direction.Normalize();

            m_Rigidbody.MovePosition(m_Rigidbody.position + direction * Time.deltaTime * m_Speed);


        }
        else
        {
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
                print("dash");
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
       

        // determines whether player will interact or attack
        if (listOfInteractablesWithinRange.Count > 0)
        {
            canInteract = true;
            interactableIcon.SetActive(true);
        }
        else
        {
            canInteract = false;
            interactableIcon.SetActive(false);
        }

        // canAttack also includes interacting
        if(canAttack)
        {
            if (!canInteract)
            {
                // if the interact button is held, begin counter to determine if melee or thrown attack
                if (Input.GetAxis("Interact") > 0)
                {
                    if (timeSinceLastInteract >= interactCooldown)
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
                            timeSinceLastInteract = 0;
                        }
                    }

                    timeInteractHeld = 0;
                    isAttacking = false;
                }
                // prevent player from attacking whilst the sword is mid-air
                if (thrown)
                {
                    timeSinceLastInteract = 0;
                }
            }
            else
            {
                // for each interactable object within player range, determine which is closest
                // only the closest object will be interacted with
                float shortestDistanceBetweenInteractables = 0;
                foreach (var interactableObject in listOfInteractablesWithinRange)
                {
                    float distanceBetweenObjects = Vector3.Distance(interactableObject.transform.position, transform.position);
                    if (shortestDistanceBetweenInteractables == 0)
                    {
                        shortestDistanceBetweenInteractables = distanceBetweenObjects;
                        closestInteractable = interactableObject;
                    }
                    else if (distanceBetweenObjects < shortestDistanceBetweenInteractables)
                    {
                        shortestDistanceBetweenInteractables = distanceBetweenObjects;
                        closestInteractable = interactableObject;
                    }
                    interactableIcon.transform.position = closestInteractable.transform.position;
                }
                // placeholder line of code until there is a universal interact script written
                if (Input.GetAxis("Interact") > 0)
                {
                  
                    if (timeSinceLastInteract >= interactCooldown)
                    {
                        closestInteractable.GetComponent<LootChest_Jerzy>().Interact();
                        timeSinceLastInteract = 0;
                    }
                    timeInteractHeld += Time.deltaTime;

                }


            }
        }


       listOfInteractablesWithinRange.Clear();

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
            timeSinceLastInteract = 0;
        }
        if(other.tag == "LootChest")
        {
            if (other.gameObject.GetComponent<LootChest_Jerzy>().isInteractable)
            {
                listOfInteractablesWithinRange.Add(other.gameObject);


            }

        }
    }

    public void SetTargetedTransform(Transform newTargetedTransform)
    {
        _targetedTransform = newTargetedTransform;
    }
}
