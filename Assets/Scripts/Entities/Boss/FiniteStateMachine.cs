using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region StateMachine
public class FiniteStateMachine
{
    private BossStats fsmOwner;

    private State CurrentState = null;

    public FiniteStateMachine(BossStats fsmOwner)
    {
        this.fsmOwner = fsmOwner;
    }

    public void SetStartState(State startState)
    {
        startState.Enter(fsmOwner);
        CurrentState = startState;
    }

    public State GetCurrentState()
    {
        return CurrentState;
    }

    public void Update()
    {
        if (CurrentState != null) CurrentState.Execute(fsmOwner);
    }

    public void ChangeState(State newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit(fsmOwner);
        }

        CurrentState = newState;

        if(CurrentState != null)
        {
            CurrentState.Enter(fsmOwner);
        }
    }

}
#endregion

#region AbstractStateClass
abstract public class State
{
    protected FiniteStateMachine owningFSM;

   abstract public void Enter(BossStats Entity);
   abstract public void Execute(BossStats Entity);
   abstract public void Exit(BossStats Entity);
}
#endregion

#region IndividualStates
public sealed class Idle : State
{
    public Idle(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(BossStats Entity)
    {
       Entity.idleTimer = 0f;       //reset timer values, 
                                   //idleTimer increases with Delta Time on BossStats's Update
    }

    public override void Execute(BossStats Entity)
    {
        if (Entity.detector.GetCurTarget() != null)
        {
            //check for BossStats HP
            if (Entity.health <= 10)
                owningFSM.ChangeState(new FinalAttack(owningFSM));
            //specified amount of time check
            if (Entity.idleTimer >= Entity.idleTime)
                owningFSM.ChangeState(new HeavyAttack(owningFSM));
        }
    }

    public override void Exit(BossStats Entity)
    {
        Debug.Log("[BossStats]: Exiting Idle State...");
    }
}

public sealed class Vulnerability : State
{
    public Vulnerability(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(BossStats Entity)
    {
        //Reset timer value, vulnTimer gets increased with Delta Time in BossStats's Update.
        Entity.vulnTimer = 0f;

        //Reset times BossStats can be hit while in vulnerable state, this value gets increased
        //when damage is dealt to the BossStats.
        Entity.currVulnHits = 0;

        //
        Entity.myRigid.velocity = Vector3.zero;
    }

    public override void Execute(BossStats Entity)
    {
        Entity.isVuln = true;
        //checks if vulntime has ended OR amount of hits have been dealt while vulnerable
        if ((Entity.vulnTimer >= Entity.vulnTime) || (Entity.currVulnHits == Entity.vulnHits))
        {
            Entity.isVuln = false;
            owningFSM.ChangeState(new Recovery(owningFSM));
        }
    }

    public override void Exit(BossStats Entity)
    {
        Debug.Log("[BossStats]: Exiting Vulnerability State...");
    }
}

public sealed class Recovery : State
{
    public Recovery(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(BossStats Entity)
    {
        Entity.myRigid.velocity = Vector3.zero;
    }

    public override void Execute(BossStats Entity)
    {
        //move back to returnSpot
        Entity.ReturnToIdle();
        //check for position and if player is in melee range
         if (Entity.hasReturned)
            owningFSM.ChangeState(new Idle(owningFSM));
        else if (Entity.detector.GetCurTarget() != null)
            owningFSM.ChangeState(new LightAttack(owningFSM));
        
    }

    public override void Exit(BossStats Entity)
    {
        Entity.hasReturned = false;
        Debug.Log("[BossStats]: Exiting Recovery State...");
    }
}

public sealed class LightAttack : State
{
    public LightAttack(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(BossStats Entity)
    {

    }

    public override void Execute(BossStats Entity)
    {
        //deal damage and knockback
        Entity.LightAttack();
        owningFSM.ChangeState(new Recovery(owningFSM));
    }

    public override void Exit(BossStats Entity)
    {
        Debug.Log("[BossStats]: Exiting Light Attack State...");
    }
}

public sealed class HeavyAttack : State
{
    public HeavyAttack(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(BossStats Entity)
    {

    }

    public override void Execute(BossStats Entity)
    {
        Entity.HeavyAttack();
        //check if miss or hit, go to recovery if hit, go to vuln. if miss
        if (Entity.HeavyAttackFinished())
        {
        if (Entity.GetHasHitPlayer())
            owningFSM.ChangeState(new Recovery(owningFSM));
        else
            owningFSM.ChangeState(new Vulnerability(owningFSM));
        }

    }

    public override void Exit(BossStats Entity)
    {
        Debug.Log("[BossStats]: Exiting Heavy Attack State...");
    }
}

public sealed class FinalAttack : State
{
    public FinalAttack(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(BossStats Entity)
    {

    }

    public override void Execute(BossStats Entity)
    {
        Entity.FinalAttack();
        owningFSM.ChangeState(new Recovery(owningFSM));
    }

    public override void Exit(BossStats Entity)
    {
        Entity.FinishFinalAttack();
        Debug.Log("[BossStats]: Exiting Final Attack State...");
    }
}
#endregion

//Recovery Execute method wip