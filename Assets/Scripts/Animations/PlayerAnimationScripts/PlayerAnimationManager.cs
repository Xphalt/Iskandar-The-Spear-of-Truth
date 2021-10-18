using UnityEngine;

/*________________________________________________________________________
 * This script was created by Fate. Contact me if you need help with it :)
 * _______________________________________________________________________*/
 
public class PlayerAnimationManager : MonoBehaviour
{
    #region Variables
    [HideInInspector] public Animator animator;

  //  [HideInInspector] public bool isIdling = false;
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
        Strafing();

        // print("is idling:" + isIdling);
        print("is strafing:" + isStrafing);
    }

    public void PlayerLongIdle(float playerVelocity) //not working atm
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
        if (!isStrafing)
            animator.SetFloat("isRunning", speed); 
        else if (isStrafing)
            animator.SetFloat("isRunning", 0);
    }

    public void Strafing()
    {
        if (isStrafing)
        {
            UpdateAxisValues();
            animator.SetBool("isIdling", false);
            animator.SetBool("isStrafing", true);
        }
        else if (!isStrafing)
        {
            animator.SetBool("isIdling", true);
            animator.SetBool("isStrafing", false);
        }
    }

    public void SimpleAttack() 
    {
        animator.SetTrigger("isSimpleAttacking");
    }

    public void SwordThrowAttack()
    {
        animator.SetTrigger("isSwordThrowing"); 
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
