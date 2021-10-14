using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Jerzy : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public GameObject playerModel;
    public float m_Speed;


    public Quaternion swordLookRotation;

    public bool canBeDamaged = true;

    public float dashCooldown;
    public float invincibilityFramesAfterDash;
    public float dashForce;
    public float dashAnalogueReq;

    float timeSinceLastDash = 0;

    private Player_Targeting_Jack _playerTargetingScript;
    private Transform _targetedTransform = null;

    float lastMagnitudeFromTarget = 0;


    [SerializeField] private float _rotationSpeed;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        _playerTargetingScript = GetComponent<Player_Targeting_Jack>();
    }

    private void Update()
    {
        timeSinceLastDash += Time.deltaTime;
    }

    public void Dash(Vector3 dashDirection)
    {
        if (timeSinceLastDash >= dashCooldown)
        {
            canBeDamaged = false;
            m_Rigidbody.AddForce(dashDirection * dashForce);
            timeSinceLastDash = 0;
        }
    }

    public void Movement(Vector3 m_Input)
    {
        if (_playerTargetingScript.IsTargeting())
        {
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
                m_Rigidbody.AddForce(playerModel.transform.forward * m_Speed * 5); // What does this 5 means?
            }

            m_Rigidbody.velocity = (direction * m_Speed);

            // this line fixes the thrown sword direction when locked onto an enemy
            swordLookRotation = Quaternion.LookRotation(playerToTargetVector);

        }
        else
        {
            m_Rigidbody.velocity = (m_Input * m_Speed);

            if (timeSinceLastDash >= invincibilityFramesAfterDash)
            {
                canBeDamaged = true;
            }

            Rotation(m_Input);
        }
    }

    private void Rotation(Vector3 m_Input)
    {
        Debug.Log(m_Input);
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
