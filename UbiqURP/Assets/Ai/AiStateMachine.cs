using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateMachine
{
    public AiState[] states;
    public AiAgent agent;
    public AiStateId currentState;

    public AiStateMachine(AiAgent agent)
    {
        // assign agent to control to this state machine
        // (a new machine is created for each agent)
        this.agent = agent;
        // Debug.Log("Agent constructed: ", this.agent);

        // create array of states to swap between
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        states = new AiState[numStates];
    }

    // set new states in AiState's AiStateId enum
    public void RegisterState(AiState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    public AiState GetState(AiStateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(AiStateId newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }

}
