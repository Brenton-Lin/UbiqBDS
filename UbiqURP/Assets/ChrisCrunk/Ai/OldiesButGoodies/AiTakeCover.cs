using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiTakeCover : AiState
{
    AiAgent agent;
    NavMeshAgent navMeshAgent;
    Transform coverTarget;

    public void Enter(AiAgent agent)
    {
        this.agent = agent;
        navMeshAgent = agent.gameObject.GetComponent<NavMeshAgent>();

        // take cover

    }

    // Update is called once per frame
    public void Update(AiAgent agent)
    {


    }

    public void Exit(AiAgent agent)
    {
        // play uncover animation

    }

    public AiStateId GetId()
    {
        return AiStateId.GetSmall;
    }

    // find nearest Cover object

    // go to it through NavMesh
    // play animation

    // take cover behind it



    public void GetNetworkUpdates(AiAgent agent)
    {
        throw new System.NotImplementedException();
    }
}
