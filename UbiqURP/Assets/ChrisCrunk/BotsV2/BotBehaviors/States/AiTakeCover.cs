using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AiTakeCover : AiState
{
    AiAgent agent;
    NavMeshAgent navMeshAgent;

    // object to hide behind
    Transform coverTarget;

    Vector3 farthestPoint;

    public void Enter(AiAgent agent)
    {
        this.agent = agent;
        navMeshAgent = agent.gameObject.GetComponent<NavMeshAgent>();

        List<CoverObject> covers = GameObject.FindObjectsOfType<CoverObject>().ToList();

        coverTarget = covers[0].transform;


        foreach (CoverObject cover in covers) 
        { 
            // find closest cover
            if (Vector3.Distance(agent.transform.position, cover.transform.position) <
                Vector3.Distance(agent.transform.position, coverTarget.transform.position))
            {

/*                // if advancing, make sure cover is closer to target than last
                if (agent.isOrderedToMoveForward)
                {

                    if (agent.coverObject == null)
                        agent.coverObject = cover;

                    // if distance from target location and potential cover is more than the current, skip considering this one // bug, goes to cover closest to pt, not next cover
                    if (Vector3.Distance(agent.advanceToThisLocation.transform.position, cover.transform.position) >
                        Vector3.Distance(agent.coverObject.transform.position, cover.transform.position))
                    {
                        continue;
                    }
                }*/
                coverTarget = cover.transform;

                // set current  cover object
                agent.coverObject = cover;

            }

        }

        Collider collider = coverTarget.GetComponent<Collider>();




        // take cover
        // find nearest cover between self and lastLocationOfEnemy

        Vector3 positionToCollider = collider.transform.position - agent.lastLocationOfEnemy.position;

        Vector3 otherSide = collider.transform.position + positionToCollider;

        farthestPoint = collider.ClosestPointOnBounds(otherSide);
        



    }

    // Update is called once per frame
    public void Update(AiAgent agent)
    {
        navMeshAgent.destination = farthestPoint;

        if (agent.advancingDemonstration)
        {
            if (!agent.isAdvancing)
                return;
            else
                agent.stateMachine.ChangeState(AiStateId.AdvancePosition);
        }

    }

    public void Exit(AiAgent agent)
    {
        // play uncover animation

    }

    public AiStateId GetId()
    {
        return AiStateId.TakeCover;
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
