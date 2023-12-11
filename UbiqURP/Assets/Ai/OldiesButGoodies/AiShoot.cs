using Org.BouncyCastle.Asn1.X509;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using Time = UnityEngine.Time;

public class AiShoot : AiState
{

    // senses
    float scanInterval;
    float scanTimer;
    public int scanFrequency = 30;
    public List<AiAgent> targets;

    // team bool friendly is squad


    public AiAgent bestTarget;
    public AiAgent lastBestTarget;

    RaycastHit hit;

    public void Enter(AiAgent agent)
    {
        agent.eyeConstraint.weight = 1.0f;
        bestTarget = null;
        targets = agent.senses.ScanForEnemies(agent.friendly);
        scanInterval = 1.0f / scanFrequency;
    }

    public void Exit(AiAgent agent)
    {
        throw new System.NotImplementedException();
    }

    public AiStateId GetId()
    {
        return AiStateId.Shoot;
    }

    public void Update(AiAgent agent)
    {

        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;

            targets = agent.senses.ScanForEnemies(agent.friendly);
        }



        bestTarget = GetBestTarget(agent.gameObject);

        if (bestTarget == null) { }

        if (bestTarget != null && lastBestTarget != bestTarget)
        {
            lastBestTarget = bestTarget;

            // head constraints
            var data = agent.eyeConstraint.data.sourceObjects;
            data.Clear();
            data.SetTransform(0, bestTarget.transform);
            data.Add(new WeightedTransform(bestTarget.transform, 1f));
            agent.eyeConstraint.data.sourceObjects = data;

/*            Debug.Log(agent.eyeConstraintDamp.data.sourceObject);
            agent.eyeConstraintDamp.data.sourceObject = bestTarget.transform;
            Debug.Log(bestTarget.transform);*/

            agent.rigs.Build();

            data.Clear();

            // weapon aim constraint
            data = agent.weaponAimConstraint.data.sourceObjects;
            data.Clear();
            data.SetTransform(0, bestTarget.transform);
            data.Add(new WeightedTransform(bestTarget.transform, 0.4f));
            agent.weaponAimConstraint.data.sourceObjects = data;

            agent.rigs.Build();

        }
        
        Ray ray = new Ray(agent.gunTip.transform.position, agent.gunTip.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider != null && bestTarget != null && hit.collider.name == bestTarget.name) 
            {
                // Debug.Log("muzzle flagged " + hit.collider.name);
                
            }
        }

    }

    private AiAgent GetBestTarget(GameObject source)
    {
        float closest = 9999f;
        AiAgent target = null;

        foreach (AiAgent agent in targets)
        {
            float distance = Vector3.Distance(agent.transform.position, source.transform.position);
            if (distance < closest)
            {
                closest = distance;
                target = agent;
            }
        }

        return target;
    }

}
