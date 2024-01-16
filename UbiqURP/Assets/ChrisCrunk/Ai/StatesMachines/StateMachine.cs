using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateMachine<EState, Args> : MonoBehaviour where EState : Enum where Args : struct
{
    

    // relate the EState enum from new machine to BaseState structure
    private Dictionary<EState, BaseState<EState, Args>> States = new Dictionary<EState, BaseState<EState, Args>>();

    protected BaseState<EState, Args> CurrentState;

    // protects against transitioning state twice due to framerate
    protected bool isChangingState = false;

    // for passing modifying values to states
    public Args arguments;

    // enter state machine
    void Start()
    {
        CurrentState.EnterState(arguments);
    }

    // update state machine
    void Update() 
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (!isChangingState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!isChangingState)
        {
            ChangeStateTo(nextStateKey);
        }
    }


    void ChangeStateTo(EState stateKey)
    {

        isChangingState = true;

        CurrentState.ExitState();
        // https://www.youtube.com/watch?v=midzgCDtncA
        CurrentState = States[stateKey];
        CurrentState.EnterState();

        isChangingState = false;
    }

    void ChangeStateTo(EState stateKey, Args arguments)
    {
        isChangingState = true;

        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState(arguments);

        isChangingState = false;
    }

    void GetState<BaseState>()
    {

    }

}
