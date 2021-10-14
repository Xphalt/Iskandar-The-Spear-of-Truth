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

    private int horiz, vert;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();

        //Reference to horizontal + vertical parameters in animator.
        horiz = Animator.StringToHash("Horizontal");
        vert = Animator.StringToHash("Vertical");
    }

    public void Running(float speed) { animator.SetFloat("isRunning", speed); }

    public void SimpleAttack()
    {
        ResetAnimationStates();
        animator.SetBool("isSimpleAttacking", true);
    }

    //public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    //{
    //    #region Snapped Movement
    //    /*_______________________________________________________________________
    //     * Animation snapping like in Dark Souls.
    //     * ______________________________________________________________________*/
    //    float snappedHoriz, snappedVert;

    //    //Horizontal
    //    if (horizontalMovement > 0 && horizontalMovement < 0.55f)
    //        snappedHoriz = 1f;
    //    else if (horizontalMovement < -0.55f)
    //        snappedHoriz = -1f;
    //    else
    //        snappedHoriz = 0;
    //    //Vertical
    //    if (verticalMovement > 0 && verticalMovement < 0.55f)
    //        snappedVert = 1f;
    //    else if (verticalMovement < -0.55f)
    //        snappedVert = -1f;
    //    else
    //        snappedVert = 0;
    //    //_______________________________________________________________________
    //    #endregion

    //    animator.SetFloat(horiz, snappedHoriz, 0.1f, Time.deltaTime);
    //    animator.SetFloat(vert, snappedVert, 0.1f, Time.deltaTime);

    //    //animator.SetFloat(horiz, verticalMovement, 0.1f, Time.deltaTime);
    //    //animator.SetFloat(vert, horizontalMovement, 0.1f, Time.deltaTime);
    //}

    public void ResetAnimationStates()
    {
        animator.SetBool("isIdling", false);
        animator.SetBool("isSimpleAttacking", false);
    }
}
