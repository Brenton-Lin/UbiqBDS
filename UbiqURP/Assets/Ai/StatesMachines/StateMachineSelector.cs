using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateMachineId
{
    EnemyStateMachine,
    FriendlyStateMachine
}

public struct bleh
{
    public int Id;
}

public class StateMachineSelector : MonoBehaviour
{
    public AiStateMachineId machineId;

    // Start is called before the first frame update
    void Start()
    {
        switch (machineId)
        {
            case AiStateMachineId.EnemyStateMachine:
                // b = CreateStateMachine<EnemyEntrenchedStateMachine>();

                break;
            case AiStateMachineId.FriendlyStateMachine:
                FriendlyRiflemanStateMachine friendlyMachine = new FriendlyRiflemanStateMachine();
                break;
        }
    }

    public T CreateStateMachine<T>() where T : new()
    {
        return new T();
    }


}
