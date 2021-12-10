using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSword_Jerzy : MonoBehaviour
{
    public float swordDamage = 2;

    public GameObject player;
    public bool thrown;
    public bool returning = false;
    public GameObject swordModel;
    Rigidbody swordRigidBody;

    PlayerCombat_Jerzy combatScript;
    private PlayerMovement_Jerzy playerMovement;
    private PlayerStats playerStats;
    private PlayerAnimationManager playerAnim;
    float throwTimeSpinningInPlace;
    float timeSpinningInPlace;

    public float ACCELERATION_MULTIPLIER = 1.03f;
    public float DECELERATION_MULTIPLIER = 0.97f;
    private const float MAX_RETURNING_SPEED = 30;

    public LayerMask throwLayer;

    float returningSpeed;
    public float throwSpeed;

    public List<Collider> puzzlesHit = new List<Collider>();

    private void Awake()
    {
        swordRigidBody = GetComponent<Rigidbody>(); 
        combatScript = player.GetComponent<PlayerCombat_Jerzy>();
        playerMovement = FindObjectOfType<PlayerMovement_Jerzy>();
        playerStats = player.GetComponent<PlayerStats>();
        playerAnim = GetComponentInParent<PlayerAnimationManager>();
    }

    void Start()
    {

        throwTimeSpinningInPlace = combatScript.throwTimeSpinningInPlace;
        returningSpeed = combatScript.throwReturnSpeed;
        throwSpeed = combatScript.throwSpeed;
    }

    void FixedUpdate()
    {
        if (swordModel.activeInHierarchy)
        {
            ThrowingSwordPhysics();
        }
    }

    private void ThrowingSwordPhysics()
    {
        if (thrown)
        {
            // stage 1 : sword slows down over time until velocity is close to 0 ( less than 1 in this case )
            if (throwSpeed > 1)
            {
                throwSpeed *= DECELERATION_MULTIPLIER;
                swordRigidBody.velocity = transform.forward * throwSpeed;
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.3f, 1, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.tag != "Enemy" && hit.transform.tag != "Pot" && hit.transform.tag != "PuzzleTrigger")
                    {
                        returning = true;
                        throwSpeed = -throwSpeed;
                        if (hit.transform.TryGetComponent(out LightSwitch ls) && !PuzzleHit(hit.collider))
                        {
                            ls.TurnOn();
                            HitPuzzle(hit.collider);
                        }
                    }
                }
            }
            // stage 2 : sword spins in place for some time
            else if (throwSpeed >= 0)
            {
                throwSpeed = 0;
                timeSpinningInPlace += Time.deltaTime;
                if(timeSpinningInPlace >= throwTimeSpinningInPlace)
                {
                    throwSpeed = -returningSpeed;
                    returning = true;
                }
            } 

            // stage 3 : sword returns to the player, increaseing velocity over time ( velocity can not be higher than max returning speed )
            if(throwSpeed < 0)
            {
                transform.LookAt(player.transform);
                throwSpeed *= ACCELERATION_MULTIPLIER;
                if (throwSpeed < -MAX_RETURNING_SPEED) throwSpeed = -MAX_RETURNING_SPEED;
                swordRigidBody.velocity = -transform.forward * throwSpeed;
            }
        }
    }

    public void ThrowSword(Vector3 pos, Quaternion targetRotation)
    {
        throwSpeed = combatScript.throwSpeed;
        timeSpinningInPlace = 0;
        if (swordModel.activeInHierarchy)
        {
            // when throw attack is initiated, set the throw direction, unparent the sword, create rigidbody with appropriate settings
            swordModel.GetComponent<BoxCollider>().enabled = true;
            returning = false;
            transform.rotation = targetRotation;
            transform.parent = null;
            thrown = true;
            swordRigidBody.isKinematic = false;
            puzzlesHit.Clear();

            // play looped spinning animation
            swordModel.GetComponent<Animator>().Play("PlayerSwordSpin");
        }
    }

    public void EndThrowCycle()
    {
        // initiated whenever sword is returning and collides with player
        if (returning)
        {
            swordModel.GetComponent<BoxCollider>().enabled = false;
            returning = false;
            thrown = false;
            swordRigidBody.isKinematic = true;
            swordModel.GetComponent<Animator>().Play("PlayerSwordIdle");
            puzzlesHit.Clear();
            //timeTravelling = 0;
        }
    }

    public void HitPuzzle(Collider puzzle)
    {
        puzzlesHit.Add(puzzle);
    }

    public bool PuzzleHit(Collider puzzle)
    {
        return puzzlesHit.Contains(puzzle);
    }

    public void ClearPuzzles()
    {
        puzzlesHit.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        // when the sword returns to the player
        if (other.TryGetComponent(out EnemyStats statsInterface)) playerStats.DealDamage(statsInterface, swordDamage);
    }
}