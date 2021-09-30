using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSword_Jerzy : MonoBehaviour
{
    public GameObject player;
    public bool thrown;
    public bool returning = false;
    public GameObject swordModel;
    public GameObject swordModelBlade;
    Rigidbody swordRigidBody;



    float throwTimeBeforeSpinInPlace;
    float throwTimeSpinningInPlace;

    float timeTravelling;

    float throwSpeed;
    float returningSpeed;


    void Start()
    {
        throwTimeBeforeSpinInPlace = player.GetComponent<PlayerStats_Jerzy>().throwTimeBeforeSpinInPlace;
        throwTimeSpinningInPlace = player.GetComponent<PlayerStats_Jerzy>().throwTimeSpinningInPlace;
        throwSpeed = player.GetComponent<PlayerStats_Jerzy>().throwSpeed;
        returningSpeed = player.GetComponent<PlayerStats_Jerzy>().throwReturnSpeed;

    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // if the sword is thrown
        if(thrown)
        {
            // move in specific direction for a specific amount of time (Stage 1 of throw attack)
            if (timeTravelling < throwTimeBeforeSpinInPlace)
            {
                swordRigidBody.velocity = transform.forward * throwSpeed;
            }

            // spin on the spot for a specific amount of time (Stage 2 of throw attack)
            else if (timeTravelling >= throwTimeBeforeSpinInPlace && timeTravelling < (throwTimeBeforeSpinInPlace + throwTimeSpinningInPlace))
            {
                swordRigidBody.velocity = new Vector3(0, 0, 0);
            }

            // return to the player (Stage 3 of throw attack)
            else if (timeTravelling >= throwTimeBeforeSpinInPlace + throwTimeSpinningInPlace)
            {
                returning = true;
                transform.LookAt(player.transform);
                swordRigidBody.velocity = transform.forward * returningSpeed;
            }
            timeTravelling += Time.deltaTime;
        }


    }

    public void ThrowSword(Quaternion targetRotation)
    {
        // when throw attack is initiated, set the throw direction, unparent the sword, create rigidbody with appropriate settings
        returning = false;
        transform.rotation = targetRotation;
        transform.parent = null;
        thrown = true;
        swordRigidBody = gameObject.AddComponent<Rigidbody>();
        swordRigidBody.useGravity = false;
        swordRigidBody.isKinematic = false;
        swordRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        // play looped spinning animation
        swordModel.GetComponent<Animator>().Play("PlayerSwordThrown");
    }

    public void EndThrowCycle()
    {
        // initiated whenever sword is returning and collides with player
        if(returning)
        {
            returning = false;
            thrown = false;
            Destroy(swordRigidBody);
            swordModel.GetComponent<Animator>().Play("PlayerSwordIdle");
            timeTravelling = 0;
        }

    }


}
