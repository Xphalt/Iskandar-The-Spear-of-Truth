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

    float throwSpeed;
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
        throwSpeed = combatScript.throwSpeed;
        returningSpeed = combatScript.throwReturnSpeed;
    }

    void FixedUpdate()
    {
        ThrowingSwordPhysics();
    }

    private void ThrowingSwordPhysics()
    {

        if (thrown)
        {
            // move in specific direction for a specific amount of time (Stage 1 of throw attack)
            if (timeTravelling < throwTimeBeforeSpinInPlace)
            {
                swordRigidBody.velocity = transform.forward * throwSpeed;
            }

            // spin on the spot for a specific amount of time (Stage 2 of throw attack)
            else if (timeTravelling >= throwTimeBeforeSpinInPlace && timeTravelling < (throwTimeBeforeSpinInPlace + throwTimeSpinningInPlace))
            {
                swordRigidBody.velocity = new Vector3(0, 0, 0);
            }

            // return to the player (Stage 3 of throw attack)
            else if (timeTravelling >= throwTimeBeforeSpinInPlace + throwTimeSpinningInPlace)
            {
                returning = true;
                transform.LookAt(player.transform);
                swordRigidBody.velocity = transform.forward * returningSpeed;
            }
            timeTravelling += Time.deltaTime;

            //playerMovement.LockPlayerMovement();
        }
    }

    public void ThrowSword(Quaternion targetRotation)
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