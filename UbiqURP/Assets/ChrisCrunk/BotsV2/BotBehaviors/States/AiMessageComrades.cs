using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMessageComrades : AiState
{
    NavMeshAgent navAgent;
    Animator animator;
    AiAgent agent;

    List<AiAgent> comrades;

    public void Enter(AiAgent agent)
    {
        navAgent = agent.GetComponent<NavMeshAgent>();
        animator = agent.GetComponent<Animator>();

        this.agent = agent;

        // play animation
        // physics overlap sphere checking for same-team tag
        comrades = agent.eyes.ScanForEnemies(!agent.friendly);

        Debug.Log("Found: " + comrades.Count);

        foreach(var comrade in comrades) 
        {
            // tell them where the enemy was spotted
            comrade.lastLocationOfEnemy = agent.lastLocationOfEnemy;

            comrade.stateMachine.ChangeState(AiStateId.TakeCover);
            Debug.Log("Alerted: " + comrade);
        }

    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateId GetId()
    {
        return AiStateId.MessageComrades;
    }

    public void GetNetworkUpdates(AiAgent agent)
    {
        
    }

    public void Update(AiAgent agent)
    {
        
    }

}
