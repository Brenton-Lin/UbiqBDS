using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiInvestigate : AiState
{
    Animator animator;
    NavMeshAgent navMeshAgent;

    public bool isInvestigating = false;

    public void Enter(AiAgent agent)
    {
        animator = agent.GetComponent<Animator>();
        navMeshAgent = agent.navMeshAgent;


        // investigate sound. play animations depending on direction sound came from.

        Vector3 directionToTarget = agent.mostNoticedSound.transform.position - agent.transform.position;
        directionToTarget.Normalize();

        float dotProduct = Vector3.Dot(agent.transform.forward, directionToTarget);

        if (dotProduct >= 0)
        {
            Debug.Log("The target is in front of the bot.");
            
            animator.SetBool("search", true);
            // agent.poseConstraints.DirectHead(agent.lastNoticedSound.transform);

            // wait for animation to stop
                // happens in update

        }

        // if source is behind > 180 degrees, turn animation first
        else if (dotProduct < 0)
        {
            Debug.Log("The target is behind the bot.");
            animator.SetBool("TurnAround", true);
        }
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateId GetId()
    {
        return AiStateId.Investigate;
    }

    public void Update(AiAgent agent)
    {
        // wait til turn/search animation play, then approach target
        if (!AnimatorIsPlaying() && !isInvestigating)
        {
            isInvestigating = true;
            navMeshAgent.destination = agent.lastNoticedSound.transform.position;
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = 1f;
            Debug.Log("Enabled agent");
        }

        // when a target is seen swap to combat
        if (agent.bestTarget != null)
        {
            navMeshAgent.destination = agent.transform.position;
            navMeshAgent.ResetPath();
            agent.navMeshAgent.speed = 1f;

            if (agent.lastBestTarget != agent.bestTarget)
            {

                agent.lastBestTarget = agent.bestTarget;
                agent.lastLocationOfEnemy = agent.bestTarget.transform;
                agent.stateMachine.ChangeState(AiStateId.Flee);
            }
        }
    }


    public bool AnimatorIsPlaying()
    {
        if (animator.IsInTransition(0))
            return true;

        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
        
    }


    public void GetNetworkUpdates(AiAgent agent)
    {

    }

}
