using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform returnSpot;
    public Transform playerPos;
    private FiniteStateMachine MyFSM;
    public float health;
    public float lightAtkDamage;
    public float lightAtkRange;
    public float heavyAtkDamage;
    public float heavyAtkRange;
    public float finalAtkDamage;

    public float idleTime;
    public float vulnTime;
    public bool godMode=false;

    public void Awake()
    {
        //Create a new State Machine and set its first State
        MyFSM = new FiniteStateMachine(this);
        MyFSM.SetStartState(new Idle(MyFSM));
    }

    public void ChangeState(State newState)
    {
        MyFSM.ChangeState(newState);
    }

    public void Update()
    {
        MyFSM.Update();
    }

    public void TakeDamage(float amount)
    {
        if(!godMode)
        {

        }
    }

    public void lightAttack()
    {

    }

    public void heavyAttack()
    {

    }

    public void finalAttack()
    {

    }
}
