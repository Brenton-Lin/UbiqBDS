using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAdvancePosition : AiState
{
    NavMeshAgent navAgent;
    Animator animator;
    

    public void Enter(AiAgent agent)
    {
        navAgent = agent.navMeshAgent;
        animator = agent.GetComponent<Animator>();

        navAgent.destination = agent.advanceToThisLocation.position;
        navAgent.speed = 5;
        navAgent.isStopped = false;
    }

    public void Exit(AiAgent agent)
    {
        agent.advancingDemonstration = true;
    }

    public AiStateId GetId()
    {
        return AiStateId.AdvancePosition;
    }

    public void GetNetworkUpdates(AiAgent agent)
    {
        throw new System.NotImplementedException();
    }

    public void Update(AiAgent agent)
    {
        if (!agent.advancingDemonstration)
            return;

        if (!agent.isAdvancing)
        {
            
            agent.stateMachine.ChangeState(AiStateId.TakeCover);

        }
    }
}
