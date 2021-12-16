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
    public bool usingWand = false;

    public float lockedSpeedMultiplier;

    public Quaternion swordLookRotation;

    public bool canBeDamaged = true;

    public float dashCooldown;
    public float dashDuration;
    public float dashForce;
    public float dashAnalogueReq;
    private Vector3 dashDirection;
    private Vector3 SavedMaxMovement;

    public float timeSinceLastDash = 0;

    private const float STARTING_DASH_MULTIPLIER = 1f;
    private const float DASH_MULTIPLIER_NUMERATOR = 10;

    private const float KNOCKBACK_DENOMINATOR_ADDITION = 0.5f;

    private Player_Targeting_Jack _playerTargetingScript;
    private PlayerStats _playerStats;
    private Transform _targetedTransform = null;

    public float maxLockOnDistance = 5.0f;

    float lastMagnitudeFromTarget = 0;

    private const float FIX_DISTANCE_FORCE = 5;

    private float fallingSpeedMultiplier = 1;
    float timeSpentFalling = 0;
    bool onGround;
    private const float TIME_BEFORE_FALLING_DOWNWARDS = 0.1f;

    public bool knockedBack = false;
    Vector3 knockBackDirection;
    private float knockBackDuration;
    private float knockBackSpeed;
    private float timeKnockedBack;
    private float dashSpeedMultiplier = STARTING_DASH_MULTIPLIER;

    private const float FALLING_SPEED_VALUE = 11;

    private bool isRooted = false;
    private float rootDuration;
    private float timeRooted;

    private bool isSliding = false;
    private bool online;

    private float respawnTime;
    private float timeSinceRespawnStarted;
    private Vector3 respawnPosition;
    private float respawnDamage;

    private float consumeDuration;
    private float timeSinceConsumed;
    private float consumeMoveAmount;

    private float stunDuration = 0;
    private float stunTimer = 0;
    private bool stunned = false;
    private bool stunnable = false;
    private const float FALLING_SPEED_MULTIPLIER = 0.5f;

    Quaternion targetRotation = new Quaternion();

    public Fading FadeUI;

    private const int RAYCAST_LAYER_MASK = -1;

    public bool CanMove => (!playerAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("Simple Attack")) &&
                (!playerAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Throw")) &&
                (!playerAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Return")) &&
                timeSinceLastDash >= dashDuration &&
                !falling && !knockedBack && !isRooted && !isSliding && !respawning && !gettingConsumed && !usingWand && !stunned;

    [SerializeField] private float _rotationSpeed;


    private float gradualSpeedMultiplier = 0.1f;
    private const float SPEED_MULTI_CHANGE_INCREASE = 1.2f;
    private const float SPEED_MULTI_CHANGE_DECREASE = 0.95f;
    private const float MAX_SPEED_MULTIPLIER = 1;
    private const float MIN_SPEED_MULTIPLIER = 0.1f;

    private float slowMult = 1;
    private float slowDuration = 0;
    private float slowTimer = 0;
    private bool slowed = false;

    float inAntlionTimer;
    const float maxInAntlionTime = 5.0f;
    public bool canBeConsumed = true;
    bool inAntlion = false;

    private void Awake()
    {
        playerAnimation = FindObjectOfType<PlayerAnimationManager>();
        _playerTargetingScript = GetComponent<Player_Targeting_Jack>();
        _playerStats = GetComponent<PlayerStats>();
        FadeUI = FindObjectOfType<Fading>();
    }

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        swordLookRotation = playerModel.transform.rotation;
    }

    private void Update()
    {
        timeSinceLastDash += Time.deltaTime;
        dashSpeedMultiplier += Time.deltaTime;

        if (isRooted && timeSinceLastDash > dashDuration)
        {
            m_Rigidbody.velocity = Vector3.zero;
            timeRooted += Time.deltaTime;
            if (timeRooted >= rootDuration)
            {
                isRooted = false;
                timeRooted = 0;
            }
        }

        if (stunned)
        {
            stunTimer += Time.deltaTime;
            stunned = stunTimer < stunDuration;
        }

        if (!gettingConsumed && !respawning)
        {
            CheckGround();
        }

        float verticalVelocity = m_Rigidbody.velocity.y;

        // check if player's velocity is less than 0 and not on the ground
        // !onGround is required because the player may be decending a ramp and it may think the player is falling without the check
        if (verticalVelocity < -FALLING_SPEED_VALUE && !onGround)
        {

            if (timeSpentFalling >= TIME_BEFORE_FALLING_DOWNWARDS && !onGround)
            {
                playerAnimation.Falling();
                falling = true;
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x * FALLING_SPEED_MULTIPLIER, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z * FALLING_SPEED_MULTIPLIER);

            }

            timeSpentFalling += Time.deltaTime;
        }
        else if (verticalVelocity == 0 && onGround)
        {
            if (falling)
                playerAnimation.Landing();


            falling = false;
            timeSpentFalling = 0;
        }
        if (verticalVelocity >= 0 && !onGround)
        {
            falling = true;
        }
        if (!gettingConsumed && !respawning)
        {
            CheckGround();
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

        // if player has stepped off a ledge, reset only their x and z velocity. This is optional if we want the player to fall directly downwards.
        //if (falling && timeSpentFalling >= TIME_BEFORE_FALLING_DOWNWARDS)
        //{
        //    m_Rigidbody.velocity = new Vector3(0, verticalVelocity, 0);
        //}

        if (SavedMaxMovement == new Vector3(0, 0, 0) && timeSinceLastDash >= 1)
        {
            SavedMaxMovement = new Vector3(0, 0, 1);
        }

    }

    private void FixedUpdate()
    {
        if (gettingConsumed)
        {
            timeSinceConsumed += Time.deltaTime;

            if (timeSinceConsumed <= consumeDuration)
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
                if (FadeUI) FadeUI.FadeIn();
                transform.position = respawnPosition;
                GetComponent<PlayerStats>().TakeDamage(respawnDamage);
                respawning = false;
            }
        }
        else
            timeSinceRespawnStarted = 0;


        if (falling || respawning || gettingConsumed)
            timeSinceLastDash = dashDuration;

        if (timeSinceLastDash < dashDuration && !falling)
        {
            // DO IT LATER TIAGO 
            if (m_Rigidbody.velocity.x <= 12 && m_Rigidbody.velocity.x >= -12 && m_Rigidbody.velocity.z <= 12 && m_Rigidbody.velocity.z >= -12)
            {
                m_Rigidbody.AddForce(SavedMaxMovement.normalized * dashForce * 9);
                Rotation(SavedMaxMovement.normalized);
            }
        }

        if (timeKnockedBack < knockBackDuration && knockedBack)
        {
            timeKnockedBack += Time.deltaTime;
            Vector3 newVel = knockBackDirection * knockBackSpeed * (1 - timeKnockedBack / knockBackDuration * KNOCKBACK_DENOMINATOR_ADDITION);
            newVel.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = newVel;
            playerAnimation.Falling();
        }
        else if (knockedBack)
        {
            EndKnockback();
        }

        if (inAntlion) inAntlionTimer += Time.deltaTime;
        else inAntlionTimer -= Time.deltaTime;
        if (inAntlionTimer < 0) inAntlionTimer = 0;
        if (inAntlionTimer >= maxInAntlionTime) inAntlionTimer = maxInAntlionTime;
        if (inAntlionTimer >= maxInAntlionTime)
        {
            canBeConsumed = true;
        }
        else canBeConsumed = false;

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
        if (timeSinceLastDash >= dashCooldown && !knockedBack && !_playerStats.HasBeenDefeated)
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
        // acceleration and decelereation of player movement
        if (m_Input.magnitude > 0)
        {
            gradualSpeedMultiplier *= SPEED_MULTI_CHANGE_INCREASE;
            if (gradualSpeedMultiplier > MAX_SPEED_MULTIPLIER) gradualSpeedMultiplier = MAX_SPEED_MULTIPLIER;
        }
        else
        {
            gradualSpeedMultiplier *= SPEED_MULTI_CHANGE_DECREASE;
            if (gradualSpeedMultiplier <= MIN_SPEED_MULTIPLIER) gradualSpeedMultiplier = MIN_SPEED_MULTIPLIER;
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x * gradualSpeedMultiplier, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z * gradualSpeedMultiplier);
        }

        if ((m_Input.x >= 0.4 || m_Input.x <= -0.4 || m_Input.z >= 0.4 || m_Input.z <= -0.4) && timeSinceLastDash >= dashDuration)
        {
            SavedMaxMovement = m_Input;
        }


        // used for the gamepad/touch movement (Will change it in the future once I create the device detection
        float speedMultiplier = Vector3.Distance(Vector3.zero, m_Input);
        if (speedMultiplier > 1) speedMultiplier = 1;

        //This prevents player from moving whilst attacking, dashing, falling
        if (CanMove)
        {
            if (_playerTargetingScript.IsTargeting())
            {
                Transform tempTransform = _playerTargetingScript.GetTargetTransform();
                _targetedTransform = tempTransform;

                // Set player rotation to look at targeted object
                Vector3 playerToTargetVector = new Vector3(_targetedTransform.position.x - transform.position.x,
                                    0.0f,
                                    _targetedTransform.position.z - transform.position.z);
                if (playerToTargetVector.magnitude > maxLockOnDistance) _playerTargetingScript.UnTargetObject();
                else
                {
                    playerModel.transform.rotation = Quaternion.LookRotation(playerToTargetVector);

                    Vector3 direction = playerModel.transform.TransformDirection(m_Input).normalized;

                    // this block of code ensures that the player does not spiral away from the targeted enemy
                    if (m_Input.x == 0)
                    {
                        lastMagnitudeFromTarget = playerToTargetVector.magnitude;
                    }
                    if (playerToTargetVector.magnitude > lastMagnitudeFromTarget)
                    {
                        m_Rigidbody.AddForce(playerModel.transform.forward * m_Speed * FIX_DISTANCE_FORCE);
                    }

                    m_Rigidbody.velocity = (direction * m_Speed * slowMult * lockedSpeedMultiplier);

                    // this line fixes the thrown sword direction when locked onto an enemy
                    swordLookRotation = Quaternion.LookRotation(playerToTargetVector);
                }


            }
            if (!_playerTargetingScript.IsTargeting())
            {
                if (m_Input.magnitude > 0)
                {
                    Vector3 newVel = m_Input.normalized * (m_Speed * speedMultiplier * gradualSpeedMultiplier * slowMult);
                    newVel.y = m_Rigidbody.velocity.y;
                    m_Rigidbody.velocity = (newVel);
                }

                Rotation(m_Input);
            }
            if (timeSinceLastDash >= dashDuration && !canBeDamaged)
            {
                canBeDamaged = true;
            }
            // trying this out to match the running animation speed with player speed
            if (speedMultiplier > 0)
                playerAnimation.animator.SetFloat("runSpeed", speedMultiplier * gradualSpeedMultiplier);
            else
                playerAnimation.animator.SetFloat("runSpeed", gradualSpeedMultiplier);
        }

    }

    void CheckGround()
    {
        Vector3 newVel = m_Rigidbody.velocity;

        if (!Physics.Raycast(transform.position, Vector3.down, m_floorDistance, RAYCAST_LAYER_MASK, QueryTriggerInteraction.Ignore))
        {
            fallingSpeedMultiplier += Time.deltaTime;
            if (knockedBack) newVel.y -= fallingSpeed * Time.deltaTime;
            else newVel.y = -(fallingSpeed * fallingSpeedMultiplier * fallingSpeedMultiplier);
            onGround = false;
        }
        else
        {
            if (newVel.y < 0) newVel.y = 0;
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
            targetRotation = Quaternion.LookRotation(m_Input);
            playerModel.transform.rotation = Quaternion.Lerp(playerModel.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            playerAnimation.Turning(playerModel.transform.rotation.x);
            swordLookRotation = playerModel.transform.rotation;
        }
        else
        {
            playerModel.transform.rotation = Quaternion.Lerp(playerModel.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            playerAnimation.Turning(playerModel.transform.rotation.x);
            swordLookRotation = playerModel.transform.rotation;
        }

    }


    public void SetTargetedTransform(Transform newTargetedTransform)
    {
        _targetedTransform = newTargetedTransform;
    }

    public void KnockBack(Vector3 otherPosition, float speed, float duration, float stunTime = 0, float verticalVel = 0)
    {
        timeSinceLastDash = dashDuration;
        otherPosition = new Vector3(otherPosition.x, transform.position.y, otherPosition.z);
        knockBackDuration = duration;
        knockBackSpeed = speed;
        playerModel.transform.LookAt(otherPosition);
        knockBackDirection = (transform.position - otherPosition).normalized;
        knockedBack = true;
        stunnable = stunTime > 0;
        if (stunnable) stunDuration = stunTime;
        if (verticalVel != 0)
        {
            knockBackDuration = verticalVel / fallingSpeed;
            Launch(verticalVel);
        }
    }

    private void EndKnockback(bool collided = false)
    {
        timeKnockedBack = 0;
        knockedBack = false;
        if (stunnable && collided) Stun(stunDuration);
        stunnable = false;
        playerAnimation.Landing();
        Vector3 newVel = Vector3.zero;
        newVel.y = m_Rigidbody.velocity.y;
        m_Rigidbody.velocity = newVel;
    }

    public void Stun(float duration = 1)
    {
        stunned = true;
        stunDuration = duration;
        stunTimer = 0;
    }

    public IEnumerator Slow(float newMult, float duration)
    {
        slowMult = newMult;
        yield return new WaitForSeconds(duration);
        slowMult = 1;
    }

    public void Root(float duration)
    {
        if (!isRooted)
        {
            isRooted = true;
            rootDuration = duration;
        }
    }

    public void Slide(bool online)
    {
        isSliding = online;
        if (!isSliding)
            LockPlayerMovement();
    }

    public void Respawn(Vector3 position, float time, float damage)
    {
        timeSinceRespawnStarted = 0;
        if (FadeUI) FadeUI.FadeOut();
        respawnDamage = damage;
        respawnPosition = position;
        respawnTime = time;
        LockPlayerMovement();
        SetRespawn();
        playerAnimation.FakeDeath();
    }

    public void SetRespawn(int active = 1)
    {
        respawning = active > 0;
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
        //GetComponent<CapsuleCollider>().enabled = false;
        if (FadeUI) FadeUI.FadeOut();
        LockPlayerMovement();
    }

    public void Launch(float yVel)
    {
        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, yVel, m_Rigidbody.velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check for object type??
        if (knockedBack) EndKnockback(true);
        GetComponent<Rigidbody>().useGravity = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        GetComponent<Rigidbody>().useGravity = true;
        if (collision.gameObject.tag == "Antlion")
            inAntlion = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Antlion")
        {
            inAntlion = true;
        }
  
    }


    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Stone" || other.tag == "Wood")
        {
            switch (other.tag)
            {
                case "Stone":
                    GetComponent<PlayerSFXPlayer>().footstepType = PlayerSFXPlayer.FootstepType.stone;
                    break;                
                case "Wood":
                    GetComponent<PlayerSFXPlayer>().footstepType = PlayerSFXPlayer.FootstepType.wood;
                    break;
            }
        }        
    }

    private void OnTriggerStay(Collider other)
    {
        if (GetComponent<PlayerSFXPlayer>().footstepType == PlayerSFXPlayer.FootstepType.defaultMass)
        {
            if (other.tag == "Stone" || other.tag == "Wood" || other.tag == "Metal")
            {

                switch (other.tag)
                {
                    case "Stone":
                        GetComponent<PlayerSFXPlayer>().footstepType = PlayerSFXPlayer.FootstepType.stone;
                        break;
                    case "Wood":
                        GetComponent<PlayerSFXPlayer>().footstepType = PlayerSFXPlayer.FootstepType.wood;
                        break;
                    case "Metal":
                        GetComponent<PlayerSFXPlayer>().footstepType = PlayerSFXPlayer.FootstepType.metal;
                        break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Stone" || other.tag == "Wood" || other.tag == "Metal")
        {
            GetComponent<PlayerSFXPlayer>().footstepType = PlayerSFXPlayer.FootstepType.defaultMass;
        }                     
    }
}
