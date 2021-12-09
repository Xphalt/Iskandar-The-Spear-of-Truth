using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat_Jerzy : MonoBehaviour
{
    public GameObject playerModel;
    
    
    public float throwTimeSpinningInPlace;
    public float throwSpeed;
    public float throwReturnSpeed = 5;
    public bool attackOffCooldown = true;
    public bool canAttack = true;
    Quaternion swordLookRotation;
    private float timeInteractHeld = 0f;
    public float timeToThrowSword;

    public GameObject swordObject;
    public GameObject swordEmpty;
    public GameObject swordDefaultPosition;

    private Animator swordAnimator;
    private PlayerAnimationManager playerAnimation;
    private PlayerMovement_Jerzy playerMovement;
    private PlayerStats playerStats;
    private ThrowSword_Jerzy throwSword;
    private Collider swordCollider;

    bool returning;
    bool thrown;

    float timeSinceLastAttack = 0;
    public float attackCooldown;

    public float TIME_BEFORE_DISABLING_COLLIDER = 0.6f; // May need to change for target pads

    private float poisonDamage;
    private float maxPoisonTicks;
    private float poisonTicks;
    private float poisonDelay;
    private float timeSinceLastPoisonDamage;
    private bool isPoisoned = false;
    private bool isThrowing = false;

    public GameObject chargeParticle;
    public GameObject chargeFinishedParticle;
    public GameObject electricParticle;

    ParticleSystem throwPowerUpParticle;
    ParticleSystem throwReadyParticle;
    ParticleSystem throwPowerUpElectricParticle;


    void Start()
    {
        swordAnimator = swordObject.GetComponent<Animator>();
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();
        playerMovement = FindObjectOfType<PlayerMovement_Jerzy>();
        playerStats = FindObjectOfType<PlayerStats>();
        throwSword = swordEmpty.GetComponent<ThrowSword_Jerzy>();
        swordCollider = swordObject.GetComponent<Collider>();
    }

    void FixedUpdate()
    {
 
        if (swordObject.activeInHierarchy)
        {
            timeSinceLastPoisonDamage += Time.deltaTime;
            timeSinceLastAttack += Time.deltaTime;
            swordLookRotation = playerMovement.swordLookRotation;
            returning = throwSword.returning;
            thrown = throwSword.thrown;

            if (timeSinceLastAttack >= TIME_BEFORE_DISABLING_COLLIDER && !thrown)
            {
                swordCollider.enabled = false;
            }
            if (!playerMovement.falling && !playerMovement.knockedBack && playerMovement.timeSinceLastDash > playerMovement.dashDuration && !playerMovement.respawning && !playerMovement.gettingConsumed)
            {
                canAttack = true;
            }
            else
                canAttack = false;

            if (isPoisoned && timeSinceLastPoisonDamage >= poisonDelay && poisonTicks < maxPoisonTicks)
            {
                playerStats.TakeDamage(poisonDamage);
                timeSinceLastPoisonDamage = 0;
                poisonTicks++;
            }
            else if (isPoisoned && poisonTicks >= maxPoisonTicks)
            {
                isPoisoned = false;
            }
        }
    }

    public void Attack()
    {
        if (throwSword.thrown && playerStats.Accessory && playerStats.Accessory.accessory == AccessoryType.BraceletOfScouting)
        {
            playerStats.Accessory.UseCurrent();    //Teleport
        }
        if (timeSinceLastAttack >= attackCooldown && attackOffCooldown && canAttack)
        {
            //swordCollider.enabled = true;
            playerAnimation.SimpleAttack();
            timeSinceLastAttack = 0;
            playerMovement.LockPlayerMovement();
        }
    }

    public void SetSwordCollider(int active)
    {
        swordCollider.enabled = active > 0;
        throwSword.ClearPuzzles();
    }

    public void ThrowAttack()
    {
        if (swordObject.activeInHierarchy)
        {
            if (timeSinceLastAttack >= attackCooldown && attackOffCooldown && canAttack)
            {
                if (!Physics.Raycast(transform.position + new Vector3(0,1,0), transform.forward, out RaycastHit hit, 1.3f, 1, QueryTriggerInteraction.Ignore))
                {
                    ThrowAction();

                }
                else if (hit.transform.tag == "Enemy" || hit.transform.tag == "Pot" || hit.transform.tag == "PuzzleTrigger")
                {
                    ThrowAction();
                }


            }
        }
    }

    void ThrowAction()
    {

        attackOffCooldown = false;
        //StartCoroutine(PauseForThrow());
        playerAnimation.SwordThrowAttack();

        playerMovement.LockPlayerMovement();
        isThrowing = true;
    }

    public void ReleaseSword()
    {
        //This function is linked to an animation event in PlayerThrow anim
        if (isThrowing)
        {
            throwSword.ThrowSword(transform.position,swordLookRotation);
            isThrowing = false;
            timeSinceLastAttack = 0;
            chargeFinishedParticle.GetComponent<ParticleSystem>().Stop();
            electricParticle.GetComponent<ParticleSystem>().Stop();
        } 
    }

    private void OnTriggerStay(Collider other)
    {
        // when the sword returns to the player
        if (other.CompareTag("playerSword") && returning && thrown)
        {
            // end throw cycle, attach sword to player, set appropriate position and rotation for the sword
            playerMovement.LockPlayerMovement();
            throwSword.EndThrowCycle();
            swordEmpty.transform.parent = playerModel.transform;
            swordEmpty.transform.SetPositionAndRotation(swordDefaultPosition.transform.position, swordDefaultPosition.transform.rotation);
            attackOffCooldown = true;
        }
    }

    public void Poison(Poison poisoner)
    {
        if (playerStats.poisonProtection) return;

        playerStats.TakeDamage(poisoner.damage);
        poisonDamage = poisoner.poisonDamage;
        poisonDelay = poisoner.poisonDelay;
        maxPoisonTicks = poisoner.amountOfPoisonTicks;
        timeSinceLastPoisonDamage = 0;
        poisonTicks = 0;
        isPoisoned = true;
    }
    public void EndPoison()
    {
        isPoisoned = false;
    }

    public void startChargingEffect()
    {
       chargeParticle.GetComponent<ParticleSystem>().Play();
    }

    public void swordChargedEffect()
    {
        chargeParticle.GetComponent<ParticleSystem>().Stop();
        chargeFinishedParticle.GetComponent<ParticleSystem>().Play();
        electricParticle.GetComponent<ParticleSystem>().Play();
    }

    public void cancelChargedAttack()
    {
        chargeParticle.GetComponent<ParticleSystem>().Stop();
        electricParticle.GetComponent<ParticleSystem>().Stop();
    }
}
