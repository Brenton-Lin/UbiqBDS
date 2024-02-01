using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Ubiq.Samples.Bots;

public class AiPatrol : AiState
{

    public Transform targetDestination;
    NavMeshAgent navAgent;
    Animator animator;

    Vector3 directionToTarget;

    List<GameObject> botPath;
    int numPathNodes = 0;
    int pathIterator = 0;

    bool isStopped = false;

    public void Enter(AiAgent agent)
    {
        navAgent = agent.GetComponent<NavMeshAgent>();
        animator = agent.GetComponent<Animator>();

        botPath = agent.botPath;
        targetDestination = botPath[0].transform;

        numPathNodes = botPath.Count;

        animator.SetBool("patrol", true);

        agent.poseConstraints.IdlePose();
    }

    public void Exit(AiAgent agent)
    {

    }

    public AiStateId GetId()
    {
        return AiStateId.Patrol;
    }

    public void Update(AiAgent agent)
    {

        // Bot Route logic

        // if distance from spot to bot is 0 and bot's speed is 0, iterate to next test target

        if (Vector3.Distance(targetDestination.position, navAgent.transform.position) <= 0.1 || navAgent.velocity.magnitude <= 1f && !isStopped)
        {
            pathIterator++;

            if (pathIterator == numPathNodes)
                pathIterator = 0;

            targetDestination = botPath[pathIterator].transform;

            navAgent.destination = targetDestination.position;

            
        }
        animator.SetFloat("AgentSpeed", navAgent.velocity.magnitude);

        // Bot spotting and hearing logic


        // sees target
        if (agent.bestTarget != null)
        {
            navAgent.destination = agent.transform.position;
            navAgent.ResetPath();
            agent.navMeshAgent.velocity = new Vector3(0, 0, 0);

            if (agent.lastBestTarget != agent.bestTarget)
            {
                agent.lastBestTarget = agent.bestTarget;
                agent.lastLocationOfEnemy = agent.bestTarget.transform;
                agent.stateMachine.ChangeState(AiStateId.Flee);

            }
        }

        // hears target
        if (agent.mostNoticedSound != null && agent.bestTarget == null) 
        {
            navAgent.destination = agent.transform.position;
            navAgent.ResetPath();
            agent.navMeshAgent.velocity = new Vector3(0,0,0);

            isStopped = true;

            // check current mostNoticedSound once
            if (agent.lastNoticedSound != agent.mostNoticedSound)
            {
                agent.lastNoticedSound = agent.mostNoticedSound;

                // investigate sound
                agent.stateMachine.ChangeState(AiStateId.Investigate);
            }
                       

        }


        agent.NetworkBots(1);

    }



    public void GetNetworkUpdates(AiAgent agent)
    {
        // helper BotAudit

        // which computer is bot manager

        // master sends position and runs logic 

        // bots when damaged tell the master

        // master damages on other clients

        // add bool to AiAgent to check MasterClient


        /*
         * 
        Network

        Bot idle

        Bot standing, sees player, looking at player

        Sync looking
        Sync death

        Bot patrolling
        
         */

        


    }
}



