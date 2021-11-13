using UnityEngine;

/*________________________________________________________________________
 * This script was created by Fate. Contact me if you need help with it :)
 * _______________________________________________________________________*/
 
public class PlayerAnimationManager : MonoBehaviour
{
    #region Variables
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isLongIdling = false;
    [HideInInspector] public bool isStrafing = false;
    [HideInInspector] public bool isSwordThrowing = false;

    private Vector2 input;
    private float time = 0; 
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAxisValues();
        Strafing();
    }

    public void Turning(float rotation)
    {
        animator.SetFloat("isTurning", rotation);
    }

    public void LongIdling(float playerVelocity) //not working atm
    {
     /*_________________________________________________________________________
     * Player switches between idle states if player is stationary for too long
     * ________________________________________________________________________*/
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
        //If player moves/no longer stationary, reset the idle condition.
        if (playerVelocity >= 0)
        {
            time = 0;
            isLongIdling = false;
        }

    }

    public void Running(float speed) 
    {
        if (speed < 0.1f)
        {
            animator.SetTrigger("isIdling");
            animator.SetBool("isRunning", false);
        }
        else
            animator.SetBool("isRunning", true);
    }

    public void Strafing()
    {
        if (isStrafing)
        { 
            animator.SetTrigger("isIdling");
            animator.SetBool("isStrafing", true);
        }
        else if (!isStrafing)
        {

            animator.SetBool("isStrafing", false);
        }
    }

    public void SimpleAttack() 
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Simple Attack")) animator.SetTrigger("isSimpleAttacking"); 
    }

    public void SwordThrowAttack() 
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Throw")) animator.SetTrigger("isSwordThrowing");
    }

    public void Dodging() 
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge")) animator.SetTrigger("isDodging"); 
    }

    public void Falling() 
    { 
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fall")) animator.SetTrigger("isFalling");
    }

    public void Landing() 
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Land")) animator.SetTrigger("isLanding");
    }

    public void Dead() 
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) animator.SetTrigger("isDead"); 
    }

    private void UpdateAxisValues()
    {
        /*_____________________________________________
         * These are assigned to the animation blend tree.
         *_____________________________________________*/
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputZ", input.y);
        //_____________________________________________
    }
}
