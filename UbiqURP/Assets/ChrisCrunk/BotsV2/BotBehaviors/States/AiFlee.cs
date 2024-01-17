using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiFlee : AiState
{
    NavMeshAgent navAgent;
    Animator animator;

    public void Enter(AiAgent agent)
    {
        navAgent = agent.GetComponent<NavMeshAgent>();
        animator = agent.GetComponent<Animator>();
        navAgent.destination = agent.homeBase.position;
        navAgent.isStopped = false;
        navAgent.speed = 4f;
    }

    public void Exit(AiAgent agent)
    {

    }

    public AiStateId GetId()
    {
        return AiStateId.Flee;
    }

    public void GetNetworkUpdates(AiAgent agent)
    {
        throw new System.NotImplementedException();
    }

    public void Update(AiAgent agent)
    {
        navAgent.destination = agent.homeBase.position;
        if (Vector3.Distance(navAgent.destination, navAgent.transform.position) <= 0.1 && navAgent.velocity.magnitude <= 1f && !navAgent.isStopped)
        {
            agent.stateMachine.ChangeState(AiStateId.MessageComrades);
        }

    }

    // run to Agent's safe spot
}
