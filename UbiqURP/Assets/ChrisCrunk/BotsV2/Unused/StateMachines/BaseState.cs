using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<EState, Args> where EState : Enum where Args : struct
{
    // constructor
    public BaseState(EState key, Args val)
    {
        StateKey = key;
        Value = val;
    }

    // enumerator storing states
    public EState StateKey { get; private set; }

    // arguments for affecting states, like speed
    public Args Value { get; private set; } = default(Args);

    public abstract void EnterState();
    public abstract void EnterState(Args val);
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract EState GetNextState();




    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);

}
