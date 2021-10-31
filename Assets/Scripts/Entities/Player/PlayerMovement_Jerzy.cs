using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Jerzy : MonoBehaviour
{
    public PlayerAnimationManager playerAnimation;

    Rigidbody m_Rigidbody;
    public GameObject playerModel;
    public float m_Speed, m_floorDistance;
    public float fallingSpeed;
    public bool falling;
    public bool respawning = false;
    public bool gettingConsumed = false;

    public Quaternion swordLookRotation;

    public bool canBeDamaged = true;

    public float dashCooldown;
    public float dashDuration;
    public float dashForce;
    public float dashAnalogueReq;
    private Vector3 dashDirection;

    public float timeSinceLastDash = 0;

    private const float STARTING_DASH_MULTIPLIER = 1f;
    private const float DASH_MULTIPLIER_NUMERATOR = 10;

    private const float KNOCKBACK_DENOMINATOR_ADDITION = 0.5f;

    private Player_Targeting_Jack _playerTargetingScript;
    private Transform _targetedTransform = null;

    float lastMagnitudeFromTarget = 0;

    private const float FIX_DISTANCE_FORCE = 5;

    private float fallingSpeedMultiplier = 1;
    float timeSpentFalling = 0;
    bool onGround;
    private const float TIME_BEFORE_FALLING_DOWNWARDS = 0.1f;

    public bool knockedBack = false;
    Vector3 knockBackDirection;
    private float knockBackDuraction;
    private float knockBackSpeed;
    private float timeKnockedBack;
    private float dashSpeedMultiplier = STARTING_DASH_MULTIPLIER;

    private bool isRooted = false;
    private float rootDuration;
    private float timeRooted;

    private float respawnTime;
    private float timeSinceRespawnStarted;
    private Vector3 respawnPosition;
    private float respawnDamage;

    private float consumeDuration;
    private float timeSinceConsumed;
    private float consumeMoveAmount;
    

    public GameObject FadeUI;

    private const int RAYCAST_LAYER_MASK = -1;

    [SerializeField] private float _rotationSpeed;

    private void Awake()
    {
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();
    }

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        _playerTargetingScript = GetComponent<Player_Targeting_Jack>();

        swordLookRotation = playerModel.transform.rotation;
    }

    private void Update()
    {
        timeSinceLastDash += Time.deltaTime;
        dashSpeedMultiplier += Time.deltaTime;

        if(isRooted && timeSinceLastDash > dashDuration)
        {
            m_Rigidbody.velocity = Vector3.zero;
            timeRooted += Time.deltaTime;
            if(timeRooted>=rootDuration)
            {
                isRooted = false;
                timeRooted = 0;
            }
        }

        if (!gettingConsumed && !respawning)
        {
            CheckGround();
        }


        float verticalVelocity = m_Rigidbody.velocity.y;

        // check if player's velocity is less than 0 and not on the ground
        // !onGround is required because the player may be decending a ramp and it may think the player is falling without the check
        if (verticalVelocity < 0 && !onGround)
        {
            falling = true;
            if (timeSpentFalling >= TIME_BEFORE_FALLING_DOWNWARDS && !onGround)
                playerAnimation.Falling();
            timeSpentFalling += Time.deltaTime;
        }
        else if (verticalVelocity == 0 && onGround)
        {
            if (falling)
                playerAnimation.Landing();
                

            falling = false;
            timeSpentFalling = 0;

        }

        // if player has stepped off a ledge, reset only their x and z velocity. This is optional if we want the player to fall directly downwards.
        //if (falling && timeSpentFalling >= TIME_BEFORE_FALLING_DOWNWARDS)
        //{
        //    m_Rigidbody.velocity = new Vector3(0, verticalVelocity, 0);
        //}
    }

    private void FixedUpdate()
    {
        if (gettingConsumed)
        {
            timeSinceConsumed += Time.deltaTime;

            if(timeSinceConsumed <= consumeDuration)
            {
                transform.Translate(Vector3.up * -consumeMoveAmount);
            }
            else
            {
                timeSinceRespawnStarted = 0;
                gettingConsumed = false;
                timeSinceConsumed = 0;
                respawning = true;
                GetComponent<CapsuleCollider>().enabled = true;
                playerAnimation.Landing();
            }

        }

        if (respawning)
        {
            timeSinceRespawnStarted += Time.deltaTime;

            if (timeSinceRespawnStarted >= respawnTime)
            {
                FadeUI.GetComponent<Fading>().FadeIn();
                transform.position = respawnPosition;
                GetComponent<PlayerStats>().TakeDamage(respawnDamage);
                respawning = false;
            }
        }
        else
            timeSinceRespawnStarted = 0;

    
        if (falling || respawning || gettingConsumed)
            timeSinceLastDash = dashDuration;

        if(timeSinceLastDash < dashDuration)
        {
            //m_Rigidbody.velocity = dashDirection * dashForce * Time.deltaTime * (DASH_MULTIPLIER_NUMERATOR / dashSpeedMultiplier);
            m_Rigidbody.velocity = dashDirection * dashForce;// / dashSpeedMultiplier;
        }

        if (timeKnockedBack < knockBackDuraction && knockedBack && !falling)
        {
            timeKnockedBack += Time.deltaTime;
            //knockBackDirection * Time.deltaTime * knockBackSpeed * (knockBackDuraction/ timeKnockedBack + KNOCKBACK_DENOMINATOR_ADDITION);
            m_Rigidbody.velocity = knockBackDirection * knockBackSpeed * (1 - timeKnockedBack / knockBackDuraction * KNOCKBACK_DENOMINATOR_ADDITION);
            playerAnimation.Falling();
        }
        else if (knockedBack)
        {
            timeKnockedBack = 0;
            knockedBack = false;
            playerAnimation.Landing();
        }

    }

    public float GetPlayerVelocity()
    {
        return m_Rigidbody.velocity.magnitude;
    }

    public void LockPlayerMovement()
    {
        m_Rigidbody.velocity = Vector3.zero;
    }

    public void Dash(Vector3 _dashDirection)
    {
        if (timeSinceLastDash >= dashCooldown && !knockedBack)
        {
            canBeDamaged = false;
            dashDirection = _dashDirection.normalized;
            timeSinceLastDash = 0;
            dashSpeedMultiplier = STARTING_DASH_MULTIPLIER;
            playerAnimation.Dodging();
        }
    }

    public void Movement(Vector3 m_Input)
    {

            //This prevents player from moving whilst attacking, dashing, falling
            if ((!playerAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("Simple Attack")) &&
                (!playerAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Throw")) &&
                (!playerAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Return")) &&
                timeSinceLastDash >= dashDuration && !falling && !knockedBack && !isRooted && !respawning && !gettingConsumed)
            {
                if (_playerTargetingScript.IsTargeting())
                {
                    Transform tempTransform = _playerTargetingScript.GetTargetTransform();
                    _targetedTransform = tempTransform;

                    // Set player rotation to look at targeted object
                    Vector3 playerToTargetVector = new Vector3(_targetedTransform.position.x - transform.position.x,
                                        0.0f,
                                        _targetedTransform.position.z - transform.position.z);

                    playerModel.transform.rotation = Quaternion.LookRotation(playerToTargetVector);

                    Vector3 direction = playerModel.transform.TransformDirection(m_Input);

                    // this block of code ensures that the player does not spiral away from the targeted enemy
                    if (m_Input.x == 0)
                    {
                        lastMagnitudeFromTarget = playerToTargetVector.magnitude;
                    }
                    if (playerToTargetVector.magnitude > lastMagnitudeFromTarget)
                    {
                        m_Rigidbody.AddForce(playerModel.transform.forward * m_Speed * FIX_DISTANCE_FORCE); 
                    }

                    m_Rigidbody.velocity = (direction * m_Speed);

                    // this line fixes the thrown sword direction when locked onto an enemy
                    swordLookRotation = Quaternion.LookRotation(playerToTargetVector);
                }
                else
                {
                    Vector3 newVel = m_Input.normalized * m_Speed;
                    newVel.y = m_Rigidbody.velocity.y;
                    m_Rigidbody.velocity = (newVel);
                    Rotation(m_Input);
                }
                if (timeSinceLastDash >= dashDuration && !canBeDamaged)
                {
                    canBeDamaged = true;
                }
            }
        /*_________________________________________________________________________
         * Player animation.
         * ________________________________________________________________________*/
        if (!playerAnimation.isLongIdling)
            {
                playerAnimation.Running(Mathf.Abs(m_Rigidbody.velocity.magnitude));
                playerAnimation.Strafing();
            }

        else if (playerAnimation.isLongIdling)
            playerAnimation.LongIdling(m_Rigidbody.velocity.magnitude);  //call player idle if waiting for too long
       //_________________________________________________________________________
    }

    void CheckGround()
    { 
        Vector3 newVel = m_Rigidbody.velocity;

        if (!Physics.Raycast(transform.position, Vector3.down, m_floorDistance, RAYCAST_LAYER_MASK, QueryTriggerInteraction.Ignore))
        {
            fallingSpeedMultiplier += Time.deltaTime;
            newVel.y = -(fallingSpeed* fallingSpeedMultiplier* fallingSpeedMultiplier);
            onGround = false;
        }
        else
        {
            newVel.y = 0;
            onGround = true;
            fallingSpeedMultiplier = 1;
        }
        m_Rigidbody.velocity = newVel;
    }


    private void Rotation(Vector3 m_Input)
    {
        //Debug.Log(m_Input);
        if (m_Input != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(m_Input);
            playerModel.transform.rotation = Quaternion.Lerp(playerModel.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            swordLookRotation = playerModel.transform.rotation;
        }
    }


    public void SetTargetedTransform(Transform newTargetedTransform)
    {
        _targetedTransform = newTargetedTransform;
    }

    public void KnockBack(Vector3 otherPosition, float speed, float duration)
    {
        timeSinceLastDash = dashDuration;
        otherPosition = new Vector3(otherPosition.x, transform.position.y, otherPosition.z);
        knockBackDuraction = duration;
        knockBackSpeed = speed;
        playerModel.transform.LookAt(otherPosition);
        knockBackDirection = (transform.position - otherPosition).normalized;
        knockedBack = true;
    }

    public void Root(float duration)
    {
        if(!isRooted)
        {
            isRooted = true;
            rootDuration = duration;
        }
    }

    public void Respawn(Vector3 position, float time, float damage)
    {
        timeSinceRespawnStarted = 0;
        FadeUI.GetComponent<Fading>().FadeOut();
        respawnDamage = damage;
        respawnPosition = position;
        respawnTime = time;
        LockPlayerMovement();
        respawning = true;
        playerAnimation.Dead();
    }

    public void GetConsumed(Vector3 position, float time, float damage, float duration, float moveAmt)
    {
        timeSinceConsumed = 0;
        respawnDamage = damage;
        respawnPosition = position;
        respawnTime = time;
        consumeDuration = duration;
        consumeMoveAmount = moveAmt;
        gettingConsumed = true;
        playerAnimation.Falling();
        GetComponent<CapsuleCollider>().enabled = false;
        FadeUI.GetComponent<Fading>().FadeOut();
        LockPlayerMovement();
    }

}
