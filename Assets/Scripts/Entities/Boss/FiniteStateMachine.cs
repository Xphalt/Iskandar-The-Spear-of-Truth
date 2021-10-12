using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region StateMachine
public class FiniteStateMachine
{
    private Boss fsmOwner;

    private State CurrentState = null;

    public FiniteStateMachine(Boss fsmOwner)
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

   abstract public void Enter(Boss Entity);
   abstract public void Execute(Boss Entity);
   abstract public void Exit(Boss Entity);
}
#endregion

#region IndividualStates
public sealed class Idle : State
{
    public Idle(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(Boss Entity)
    {
       Entity.idleTimer = 0f;       //reset timer values, 
                                   //idleTimer increases with Delta Time on Boss's Update
    }

    public override void Execute(Boss Entity)
    {
        //check for boss HP
        if (Entity.health <= 10)
            owningFSM.ChangeState(new FinalAttack(owningFSM));
        //specified amount of time check
        if (Entity.idleTimer >= Entity.idleTime)
            owningFSM.ChangeState(new HeavyAttack(owningFSM));
    }

    public override void Exit(Boss Entity)
    {
        Debug.Log("[BOSS]: Exiting Idle State...");
    }
}

public sealed class Vulnerability : State
{
    public Vulnerability(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(Boss Entity)
    {
        //Reset timer value, vulnTimer gets increased with Delta Time in Boss's Update.
        Entity.vulnTimer = 0f;

        //Reset times boss can be hit while in vulnerable state, this value gets increased
        //when damage is dealt to the boss.
        Entity.currVulnHits = 0;
    }

    public override void Execute(Boss Entity)
    {
        Entity.isVuln = true;
        //checks if vulntime has ended OR amount of hits have been dealt while vulnerable
        if ((Entity.vulnTimer >= Entity.vulnTime) || (Entity.currVulnHits == Entity.vulnHits))
        {
            Entity.isVuln = false;
            owningFSM.ChangeState(new Recovery(owningFSM));
        }
    }

    public override void Exit(Boss Entity)
    {
        Debug.Log("[BOSS]: Exiting Vulnerability State...");
    }
}

public sealed class Recovery : State
{
    public Recovery(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(Boss Entity)
    {

    }

    public override void Execute(Boss Entity)
    {
        //move back to returnSpot
        Entity.ReturnToIdle();
        //check for position and if player is in melee range
        if (/*player inside range*/)
            owningFSM.ChangeState(new LightAttack(owningFSM));
        else if (Entity.transform.position == Entity.returnSpot.position)
            owningFSM.ChangeState(new Idle(owningFSM));
        
    }

    public override void Exit(Boss Entity)
    {
        Debug.Log("[BOSS]: Exiting Recovery State...");
    }
}

public sealed class LightAttack : State
{
    public LightAttack(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(Boss Entity)
    {

    }

    public override void Execute(Boss Entity)
    {
        //deal damage and knockback
        if (Entity.LightAttack())
            owningFSM.ChangeState(new Recovery(owningFSM));
    }

    public override void Exit(Boss Entity)
    {
        Debug.Log("[BOSS]: Exiting Final Attack State...");
    }
}

public sealed class HeavyAttack : State
{
    public HeavyAttack(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(Boss Entity)
    {

    }

    public override void Execute(Boss Entity)
    {
        //check if miss or hit, go to recovery if hit, go to vuln. if miss
        if (Entity.HeavyAttack())
            owningFSM.ChangeState(new Recovery(owningFSM));
        else
            owningFSM.ChangeState(new Vulnerability(owningFSM));
    }

    public override void Exit(Boss Entity)
    {
        Debug.Log("[BOSS]: Exiting Heavy Attack State...");
    }
}

public sealed class FinalAttack : State
{
    public FinalAttack(FiniteStateMachine fsm)
    {
        owningFSM = fsm;
    }

    public override void Enter(Boss Entity)
    {

    }

    public override void Execute(Boss Entity)
    {
        if (Entity.FinalAttack())
            owningFSM.ChangeState(new Recovery(owningFSM));
    }

    public override void Exit(Boss Entity)
    {
        Debug.Log("[BOSS]: Exiting Final Attack State...");
    }
}
#endregion