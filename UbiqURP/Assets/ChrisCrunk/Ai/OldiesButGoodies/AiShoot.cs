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

    // check eyesight every n milliseconds
    float scanInterval;
    float scanTimer;
    public int scanFrequency = 30;

    // list of targets seen
    public List<AiAgent> targets;

    // team bool friendly is squad

    public AiAgent lastBestTarget;

    RaycastHit hit;

    public void Enter(AiAgent agent)
    {
        agent.eyeConstraint.weight = 1.0f;

        targets = agent.eyes.ScanForEnemies(agent.friendly);
        scanInterval = 1.0f / scanFrequency;
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateId GetId()
    {
        return AiStateId.ShootTarget;
    }

    public void Update(AiAgent agent)
    {

        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {

            scanTimer += scanInterval;

            targets = agent.eyes.ScanForEnemies(agent.friendly);

        }

        // keep, visual targetting
        agent.bestTarget = GetBestTarget(agent.gameObject);

        // agent.bestTarget = agent.heardSounds[0].gameObject.GetComponent<AiAgent>();

        /*if (agent != null && agent.mostNoticedSound != null && agent.mostNoticedSound.GetComponent<AiAgent>() != null)
            agent.bestTarget = agent.mostNoticedSound.GetComponent<AiAgent>();*/

        if (agent.bestTarget == null) 
        {
            
        }

        // constrain model rig to point head, and weapons towards source
        // avoid checking repeatedly
        // only do this function if GetBestTarget returns a different target than last check
        if (agent.bestTarget != null && lastBestTarget != agent.bestTarget)
        {
            lastBestTarget = agent.bestTarget;

            // head constraints
            var data = agent.eyeConstraint.data.sourceObjects;
            data.Clear();
            data.SetTransform(0, agent.bestTarget.transform);
            data.Add(new WeightedTransform(agent.bestTarget.transform, 1f));
            agent.eyeConstraint.data.sourceObjects = data;

/*            Debug.Log(agent.eyeConstraintDamp.data.sourceObject);
            agent.eyeConstraintDamp.data.sourceObject = bestTarget.transform;
            Debug.Log(bestTarget.transform);*/

            agent.rigs.Build();

            data.Clear();

            // weapon aim constraint
            data = agent.weaponAimConstraint.data.sourceObjects;
            data.Clear();
            data.SetTransform(0, agent.bestTarget.transform);
            data.Add(new WeightedTransform(agent.bestTarget.transform, 0.4f));
            agent.weaponAimConstraint.data.sourceObjects = data;

            agent.rigs.Build();

        }
        
        // check for muzzle pointing at correct place

        Ray ray = new Ray(agent.gunTip.transform.position, agent.gunTip.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && agent.bestTarget != null && hit.collider.name == agent.bestTarget.name) 
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

    public void GetNetworkUpdates(AiAgent agent)
    {
        throw new System.NotImplementedException();
    }
}
