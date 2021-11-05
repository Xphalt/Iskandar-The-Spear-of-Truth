using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSword_Jerzy : MonoBehaviour
{
    public float swordDamage = 2;

    public GameObject player;
    public bool thrown;
    public bool returning = false;
    public GameObject swordModel;
    Rigidbody swordRigidBody;

    PlayerCombat_Jerzy combatScript;
    private PlayerMovement_Jerzy playerMovement;
    private PlayerStats playerStats;
    private PlayerAnimationManager playerAnim;

    float throwTimeBeforeSpinInPlace;
    float throwTimeSpinningInPlace;

    float timeTravelling;
    float startTime;

    float minThrowSpeed, maxThrowSpeed;
    float returningSpeed;

   // public float pauseBeforeThrow;

    private void Awake()
    {
        swordRigidBody = GetComponent<Rigidbody>(); 
        combatScript = player.GetComponent<PlayerCombat_Jerzy>();
        playerMovement = FindObjectOfType<PlayerMovement_Jerzy>();
        playerStats = player.GetComponent<PlayerStats>();
        playerAnim = GetComponentInParent<PlayerAnimationManager>();
    }

    void Start()
    {
        throwTimeBeforeSpinInPlace = combatScript.throwTimeBeforeSpinInPlace;
        throwTimeSpinningInPlace = combatScript.throwTimeSpinningInPlace;
        minThrowSpeed = combatScript.minThrowSpeed;
        maxThrowSpeed = combatScript.maxThrowSpeed;
        returningSpeed = combatScript.throwReturnSpeed;
    }

    void FixedUpdate()
    {
        if (swordModel.activeInHierarchy)
        {
            ThrowingSwordPhysics();
        }
    }

    private void ThrowingSwordPhysics()
    {
        if (thrown)
        {
            Vector3 smoothVel = new Vector3(Mathf.SmoothStep(minThrowSpeed, maxThrowSpeed, timeTravelling), 0, 0);

            // move in specific direction for a specific amount of time (Stage 1 of throw attack)
            if (timeTravelling < throwTimeBeforeSpinInPlace)
            {
                //float t = (Time.time - startTime);
                //swordRigidBody.velocity = transform.forward * throwSpeed;
                
                swordRigidBody.velocity = transform.forward * smoothVel.magnitude;
            }

            // spin on the spot for a specific amount of time (Stage 2 of throw attack)
            else if (timeTravelling >= throwTimeBeforeSpinInPlace && timeTravelling < (throwTimeBeforeSpinInPlace + throwTimeSpinningInPlace))
            {
                swordRigidBody.velocity = Vector3.zero;
            }

            // return to the player (Stage 3 of throw attack)
            else if (timeTravelling >= throwTimeBeforeSpinInPlace + throwTimeSpinningInPlace)
            {
                returning = true;
                transform.LookAt(player.transform);
                //swordRigidBody.velocity = transform.forward * returningSpeed;
                swordRigidBody.velocity = transform.forward * smoothVel.magnitude;
            }
            timeTravelling += Time.deltaTime;

            print("smooth vel " + smoothVel);
        }
    }

    public void ThrowSword(Quaternion targetRotation)
    {

        if (swordModel.activeInHierarchy)
        {
            // when throw attack is initiated, set the throw direction, unparent the sword, create rigidbody with appropriate settings
            playerAnim.SwordThrowAttack();

            swordModel.GetComponent<BoxCollider>().enabled = true;
            returning = false;
            transform.rotation = targetRotation;
            transform.parent = null;
            thrown = true;
            swordRigidBody.isKinematic = false;

            // play looped spinning animation
            swordModel.GetComponent<Animator>().Play("PlayerSwordSpin");
        }
    }



    public void EndThrowCycle()
    {
        // initiated whenever sword is returning and collides with player
        if (returning)
        {
            swordModel.GetComponent<BoxCollider>().enabled = false;
            returning = false;
            thrown = false;
            swordRigidBody.isKinematic = true;
            swordModel.GetComponent<Animator>().Play("PlayerSwordIdle");
            timeTravelling = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // when the sword returns to the player
        if (other.TryGetComponent(out EnemyStats statsInterface))
        {
            playerStats.DealDamage(statsInterface, swordDamage);
        }
    }
}