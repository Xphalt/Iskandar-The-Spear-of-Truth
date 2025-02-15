using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStats : StatsInterface
{
    public Transform returnSpot, fleeSpot;
    internal Rigidbody myRigid;
    private FiniteStateMachine MyFSM;
    public PlayerDetection detector;
    //public int health;
    public float moveSpeed;
    public int lightAtkDamage;
    public float lightAtkCooldown;
    public float lightAtkRange;
    protected float lightAtkTimer;
    public int heavyAtkDamage;
    public int finalAtkDamage;

    public float idleTime;
    internal float idleTimer;
    public float vulnTime;
    internal float vulnTimer;
    public int vulnHits;
    internal int currVulnHits;
    public bool isVuln = false;
    protected bool heavyAtkHit = false, lightAtkHit = false;
    internal bool hasReturned, hasFled;
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
        detector = GetComponent<PlayerDetection>();
        myRigid = GetComponent<Rigidbody>();
        MyFSM.SetStartState(new Idle(MyFSM));
    }


    public void Update()
    {
        MyFSM.Update();
        detector.FindVisibleTargets();
        idleTimer += Time.deltaTime;
        vulnTimer += Time.deltaTime;
        lightAtkTimer += Time.deltaTime;
    }

    public override void TakeDamage(float amount, bool scriptedKill)
    {
        //this is called when Boss is in 'Vulnerability' State
        if(!godMode && isVuln)
        {
            health -= amount;
            currVulnHits++;
        }
    }

    public override void DealDamage(StatsInterface target, float amount, bool scriptedKill)
    {
        target.TakeDamage(amount, scriptedKill);
    }

    public void ReturnToIdle()
    {
        //moves the boss to its 'returnSpot', this is called when Boss is in 'Recovery' State
        transform.position = Vector3.MoveTowards(transform.position,returnSpot.position,moveSpeed * Time.deltaTime);
    }

    public void BeginFlee()
    {
        returnSpot.gameObject.SetActive(false);
        hasReturned = false;
    }

    public void RunAway()
    {
        //moves the boss to its 'returnSpot', this is called when Boss is in 'Recovery' State
        transform.position = Vector3.MoveTowards(transform.position, fleeSpot.position, moveSpeed * Time.deltaTime);
    }

    //These virtual methods are to be overriden by the specific boss's script
    public virtual void LightAttack() { }

    public virtual void HeavyAttack() { }

    public virtual bool HeavyAttackFinished()
    {
        return true;
    }

    public virtual void FinalAttack() { }

    public virtual bool HasFinalAttackFinished() { return true; }

    public virtual void FinishFinalAttack() { }

    public virtual bool LightAttackHit()
    {
        return lightAtkHit;
    }

    public virtual bool HeavyAttackHit()
    {
        return heavyAtkHit && !godMode;
    }

    public virtual bool ScriptedKillFinished()
    {
        return (lightAtkHit || heavyAtkHit) && godMode;
    }

    public virtual void EndFight()
    {
        gameObject.SetActive(false);
    }
}
