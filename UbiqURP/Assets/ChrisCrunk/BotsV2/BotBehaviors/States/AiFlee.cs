using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiFlee : AiState
{
    NavMeshAgent navAgent;
    Animator animator;

    public bool isFleeing;

    public void Enter(AiAgent agent)
    {
        navAgent = agent.GetComponent<NavMeshAgent>();
        animator = agent.GetComponent<Animator>();

        agent.poseConstraints.DirectHead(agent.bestTarget.transform);
        animator.SetBool("stare", true);

        isFleeing = false;
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

        // wait til turn/search animation play, then approach target
        if (!agent.AnimatorIsPlaying() && !isFleeing)
        {
            agent.poseConstraints.ClearAim();
            Debug.Log("Returning to homeBase");
            isFleeing = true;
            navAgent.destination = agent.homeBase.transform.position;
            navAgent.isStopped = false;
            navAgent.speed = 2f;
        }

        // wait til animation finishes
        if (!isFleeing)
            return;

        // when home is reached
        if (Vector3.Distance(navAgent.destination, navAgent.transform.position) <= 0.1 && navAgent.velocity.magnitude <= 1f && !navAgent.isStopped)
        {
            agent.stateMachine.ChangeState(AiStateId.MessageComrades);
        }

    }

    // run to Agent's safe spot
}
