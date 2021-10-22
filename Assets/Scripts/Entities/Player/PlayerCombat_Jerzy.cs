using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat_Jerzy : MonoBehaviour
{
    public GameObject playerModel;

    public float throwTimeBeforeSpinInPlace;
    public float throwTimeSpinningInPlace;
    public float throwSpeed;
    public float throwReturnSpeed;
    public bool canAttack = true;
    Quaternion swordLookRotation;
    private float timeInteractHeld = 0f;
    public float timeToThrowSword, swordReleaseDelay;

    public GameObject swordObject;
    public GameObject swordEmpty;
    public GameObject swordDefaultPosition;

    private Animator swordAnimator;
    private PlayerAnimationManager playerAnimation;
    private PlayerMovement_Jerzy playerMovement;
    private ThrowSword_Jerzy throwSword;
    private Collider swordCollider;

    bool returning;
    bool thrown;

    float timeSinceLastAttack = 0;
    public float attackCooldown;

    public float TIME_BEFORE_DISABLING_COLLIDER = 0.6f; // May need to change for target pads


    void Start()
    {
        swordAnimator = swordObject.GetComponent<Animator>();
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();
        playerMovement = FindObjectOfType<PlayerMovement_Jerzy>();
        throwSword = swordEmpty.GetComponent<ThrowSword_Jerzy>();
        swordCollider = swordObject.GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        timeSinceLastAttack += Time.deltaTime;
        swordLookRotation = playerMovement.swordLookRotation;
        returning = throwSword.returning;
        thrown = throwSword.thrown;
        if (timeSinceLastAttack >= TIME_BEFORE_DISABLING_COLLIDER && !thrown)
        {
            swordCollider.enabled = false;
        }

    }

    public void Attack()
    {
        if (timeSinceLastAttack >= attackCooldown && canAttack)
        {
            swordCollider.enabled = true;
            playerAnimation.SimpleAttack();
            timeSinceLastAttack = 0;
            playerMovement.LockPlayerMovement();
        }
    }

    public void ThrowAttack()
    {
        StartCoroutine(PauseForThrow());
    }

    IEnumerator PauseForThrow() 
    {
        /*___________________________________________________________________________
         * This makes the attack line up with animation sword release time.
         * __________________________________________________________________________*/
        playerAnimation.SwordThrowAttack();

        yield return new WaitForSeconds(swordReleaseDelay);

        if (timeSinceLastAttack >= attackCooldown && canAttack)
        {
            throwSword.ThrowSword(swordLookRotation);
            canAttack = false;
            timeSinceLastAttack = 0;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        // when the sword returns to the player
        if (other.tag == "playerSword" && returning && thrown)
        {
            // end throw cycle, attach sword to player, set appropriate position and rotation for the sword
            throwSword.EndThrowCycle();
            swordEmpty.transform.parent = playerModel.transform;
            swordEmpty.transform.SetPositionAndRotation(swordDefaultPosition.transform.position, swordDefaultPosition.transform.rotation);
            canAttack = true;
        }
    }
}
