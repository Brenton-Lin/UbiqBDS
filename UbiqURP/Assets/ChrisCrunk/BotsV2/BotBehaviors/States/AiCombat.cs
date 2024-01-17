using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCombat : AiState
{
    public void Enter(AiAgent agent)
    {
        // switch to combat pose
        agent.poseConstraints.DirectHead(agent.bestTarget.transform);
        agent.poseConstraints.DirectWeapon(agent.bestTarget.transform);

        if (agent.owner)
        {

            GetNetworkUpdates(agent);
            // iniate a fucking stnad in bullshit objectg
        }

    }

    public void Exit(AiAgent agent)
    {

    }

    public AiStateId GetId()
    {
        return AiStateId.Combat;
    }

    public void GetNetworkUpdates(AiAgent agent)
    {
        agent.NetworkBots(4);
    }

    public void Update(AiAgent agent)
    {

        // if not in cover, look around for nearest cover

        // if in cover stay in cover

        // agent.closestCover;

        // navMeshAgent.destination = coverTarget.position;

        // control tactic based on role


        // set pose when target changes



        if (agent.bestTarget != null && agent.lastBestTarget != agent.bestTarget)
        {
            agent.lastBestTarget = agent.bestTarget;

            agent.poseConstraints.DirectHead(agent.bestTarget.transform);
            agent.poseConstraints.DirectWeapon(agent.bestTarget.transform);
            agent.poseConstraints.RebuildRig();

            GetNetworkUpdates(agent);
        }


        // if target is gone, set to idle *testing*

        if (agent.bestTarget == null)
        {
            agent.stateMachine.ChangeState(AiStateId.Guard);
        }


    }

}
