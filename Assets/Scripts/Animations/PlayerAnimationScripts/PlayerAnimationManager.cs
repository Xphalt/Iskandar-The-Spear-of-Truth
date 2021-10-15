using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*________________________________________________________________________
 * This script was created by Fate. Contact me if you need help with it :)
 * _______________________________________________________________________*/
 
public class PlayerAnimationManager : MonoBehaviour
{
    #region Variables
    [HideInInspector] public Animator animator;

    public bool isLongIdling = false;
    private float time = 0; 
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayerLongIdle(float playerVelocity)
    {
        float waitDuration = 4;

        while ((time < waitDuration) && !isLongIdling)
        {
            time += Time.deltaTime;
        }
        if (time >= waitDuration)
        {
            animator.SetBool("isLongIdling", true);
            isLongIdling = true;
        }

        if (playerVelocity >= 0)
        {
            time = 0;
            isLongIdling = false;
        }

    }

    public void Running(float speed) { animator.SetFloat("isRunning", speed); }

    public void SimpleAttack()
    {
        animator.SetTrigger("isSimpleAttacking");
    }

    public void ResetAnimationStates()
    {
    } 
    
 
}
