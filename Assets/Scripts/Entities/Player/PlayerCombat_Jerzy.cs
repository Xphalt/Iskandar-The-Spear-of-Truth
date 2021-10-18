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

    Animator swordAnimator;
    private PlayerAnimationManager playerAnimation;
    private PlayerMovement_Jerzy playerMovement;

    bool returning;
    bool thrown;

    float timeSinceLastAttack = 0;
    public float attackCooldown;

    void Start()
    {
        swordAnimator = swordObject.GetComponent<Animator>();
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();
        playerMovement = FindObjectOfType<PlayerMovement_Jerzy>();
    }

    void FixedUpdate()
    {
        timeSinceLastAttack += Time.deltaTime;
        swordLookRotation = GetComponent<PlayerMovement_Jerzy>().swordLookRotation;
        returning = swordEmpty.GetComponent<ThrowSword_Jerzy>().returning;
        thrown = swordEmpty.GetComponent<ThrowSword_Jerzy>().thrown;

    }

    public void Attack()
    {
        if (timeSinceLastAttack >= attackCooldown && canAttack)
        {
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
            swordEmpty.GetComponent<ThrowSword_Jerzy>().ThrowSword(swordLookRotation);
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
            swordEmpty.GetComponent<ThrowSword_Jerzy>().EndThrowCycle();
            swordEmpty.transform.parent = playerModel.transform;
            swordEmpty.transform.position = swordDefaultPosition.transform.position;
            swordEmpty.transform.rotation = swordDefaultPosition.transform.rotation;
            canAttack = true;
        }
    }
}
