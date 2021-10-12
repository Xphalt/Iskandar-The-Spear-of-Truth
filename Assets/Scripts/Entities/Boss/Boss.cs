using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Transform returnSpot;
    public Transform playerPos;
    private FiniteStateMachine MyFSM;
    public int health;
    public float moveSpeed;
    public int lightAtkDamage;
    public int lightAtkRange;
    public int heavyAtkDamage;
    public int heavyAtkRange;
    public int finalAtkDamage;

    public float idleTime;
    public float idleTimer;
    public float vulnTime;
    public float vulnTimer;
    public int vulnHits;
    public int currVulnHits;
    public bool isVuln = false;
    /*For scripted story event*/
    public bool godMode = false;

    //Changes FSM's current state
    public void ChangeState(State newState)
    {
        MyFSM.ChangeState(newState);
    }

    public void Awake()
    {
        //Create a new State Machine and set its first State
        MyFSM = new FiniteStateMachine(this);
        MyFSM.SetStartState(new Idle(MyFSM));
    }


    public void Update()
    {
        //MyFSM.Update();
        idleTimer += Time.deltaTime;
        vulnTimer += Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        //this is called when Boss is in 'Vulnerability' State
        if((!godMode) && (isVuln))
        {
            health -= amount;
            currVulnHits++;
        }
    }

    public void ReturnToIdle()
    {
        //moves the boss to its 'returnSpot', this is called when Boss is in 'Recovery' State
        transform.position = Vector3.MoveTowards(transform.position,returnSpot.position,moveSpeed);
    }

    //These virtual methods are to be overriden by the specific boss's script
    public virtual bool LightAttack()
    {
        return true;
    }

    public virtual bool HeavyAttack()
    {
        return true;
    }

    public virtual bool FinalAttack()
    {
        return true;
    }
}
