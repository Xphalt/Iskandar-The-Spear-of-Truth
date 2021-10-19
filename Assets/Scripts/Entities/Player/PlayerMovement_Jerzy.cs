using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Jerzy : MonoBehaviour
{
    private PlayerAnimationManager playerAnimation;

    Rigidbody m_Rigidbody;
    public GameObject playerModel;
    public float m_Speed;

    public Quaternion swordLookRotation;

    public bool canBeDamaged = true;

    public float dashCooldown;
    public float invincibilityFramesAfterDash;
    public float dashForce;
    public float dashAnalogueReq;
    private Vector3 dashDirection;

    float timeSinceLastDash = 0;

    private Player_Targeting_Jack _playerTargetingScript;
    private Transform _targetedTransform = null;

    float lastMagnitudeFromTarget = 0;

    private const float FIX_DISTANCE_FORCE = 5;


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
    }

    private void FixedUpdate()
    {
        if(!canBeDamaged)
        {
            m_Rigidbody.AddForce(dashDirection * dashForce);
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
        if (timeSinceLastDash >= dashCooldown)
        {
            canBeDamaged = false;
            dashDirection = _dashDirection;
            timeSinceLastDash = 0;
        }
    }

    public void Movement(Vector3 m_Input)
    {
        //This prevents player from moving whilst attacking or dashing
        if ((!playerAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("Simple Attack")) &&
            (!playerAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("SwordThrow&Return")) &&
            timeSinceLastDash >= invincibilityFramesAfterDash)
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
                    m_Rigidbody.AddForce(playerModel.transform.forward * m_Speed * FIX_DISTANCE_FORCE); // What does this 5 means? <--- made into const
                }

                m_Rigidbody.velocity = (direction * m_Speed);

                // this line fixes the thrown sword direction when locked onto an enemy
                swordLookRotation = Quaternion.LookRotation(playerToTargetVector);
            }
            else
            {
                m_Rigidbody.velocity = (m_Input.normalized * m_Speed);
                Rotation(m_Input);
            }
            if (timeSinceLastDash >= invincibilityFramesAfterDash && !canBeDamaged)
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
            playerAnimation.PlayerLongIdle(m_Rigidbody.velocity.magnitude);  //call player idle if waiting for too long
        //_________________________________________________________________________
        //Debug.Log(Mathf.Abs(m_Rigidbody.velocity.magnitude));
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
}
