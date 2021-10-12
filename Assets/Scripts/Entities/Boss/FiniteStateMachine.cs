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
        //check for boss HP
    }

    public override void Execute(Boss Entity)
    {
        //specified amount of time check
    }

    public override void Exit(Boss Entity)
    {
        //reset timer values
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
        
    }

    public override void Execute(Boss Entity)
    {
        //checks for vulnerability time & amount of hits
    }

    public override void Exit(Boss Entity)
    {
        
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
        //check for position and if player is in melee range
    }

    public override void Exit(Boss Entity)
    {

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
    }

    public override void Exit(Boss Entity)
    {

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
    }

    public override void Exit(Boss Entity)
    {

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
       
    }

    public override void Exit(Boss Entity)
    {

    }
}
#endregion