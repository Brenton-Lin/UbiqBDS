using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.AI;

public class AiGuard : AiState
{
    public NetworkContext context;

    NavMeshAgent navAgent;
    Animator animator;

    public void Enter(AiAgent agent)
    {
        navAgent = agent.GetComponent<NavMeshAgent>();
        animator = agent.GetComponent<Animator>();

        agent.poseConstraints.IdlePose();

        // send network update if owner
        if (agent.owner)
        {
            
            GetNetworkUpdates(agent);
        }

        // set constraints to relaxed idle state

    }

    public void Update(AiAgent agent)
    {


        // switch when target is found
        if (agent.bestTarget != null)
        {
            agent.stateMachine.ChangeState(AiStateId.Combat);
        }

        
    }

    public void Exit(AiAgent agent)
    {

    }

    public AiStateId GetId()
    {
        return AiStateId.Guard;
    }

    // send network updates lol
    public void GetNetworkUpdates(AiAgent agent)
    {
        //Debug.Log("Sending message to update pose");
        agent.NetworkBots(3);
    }



}
