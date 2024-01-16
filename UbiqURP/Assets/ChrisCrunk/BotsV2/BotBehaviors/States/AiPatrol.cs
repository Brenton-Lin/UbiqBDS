using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Ubiq.Samples.Bots;

public class AiPatrol : AiState
{
    public NetworkContext context;


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
        // if distance from spot to bot is 0 and bot's speed is 0, iterate to next test target

        if (Vector3.Distance(targetDestination.position, navAgent.transform.position) <= 0.1 || navAgent.velocity.magnitude <= 1f && !isStopped)
        {
            pathIterator++;

            if (pathIterator == numPathNodes)
                pathIterator = 0;

            targetDestination = botPath[pathIterator].transform;

            navAgent.destination = targetDestination.position;

            animator.SetFloat("AgentSpeed", navAgent.velocity.magnitude);
        }
        

        // agent turns to sound
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

                // if source is behind > 180 degrees, turn animation first

                Vector3 directionToTarget = agent.mostNoticedSound.transform.position - agent.transform.position;
                directionToTarget.Normalize();

                float dotProduct = Vector3.Dot(agent.transform.forward, directionToTarget);

                if (dotProduct >= 0)
                {
                    Debug.Log("The target is in front of the bot.");
                }
                else if (dotProduct < 0)
                {
                    Debug.Log("The target is behind the bot.");
                    animator.SetBool("TurnAround", true);
                }

                // agent.poseConstraints.DirectHead(agent.mostNoticedSound.transform);

            }
            


            // investigate a sound
            // agent.stateMachine.ChangeState(AiStateId.Investigate);

            

        }


        if (agent.bestTarget != null) 
        {
            // agent gun aim constraints

                // branch statement for insta-shoot, 
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



