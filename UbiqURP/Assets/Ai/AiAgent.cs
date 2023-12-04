using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;


public class AiAgent : MonoBehaviour
{
    public bool isPlayerCharacter;

    // senses
    public AiSensor senses;
    float scanInterval;
    float scanTimer;
    public int scanFrequency = 30;
    public List<AiAgent> targets;

    public MultiAimConstraint headFocus;
    public RigBuilder rigs;

    // team bool friendly is squad
    public bool friendly;

    public AiAgent bestTarget;
    public AiAgent lastBestTarget;

    // statemachine
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public List<AiStateId> possibleStates;

    // Start is called before the first frame update
    void Start()
    {
        senses = GetComponent<AiSensor>();
        headFocus.weight = 1.0f;

        bestTarget = null;
        targets = senses.ScanForEnemies(friendly);

        scanInterval = 1.0f / scanFrequency;

        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.ChangeState(initialState);

        rigs = gameObject.GetComponent<RigBuilder>();

    }

    // Update is called once per frame
    void Update()
    {
        // stateMachine.Update();
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            
            targets = senses.ScanForEnemies(friendly);
        }

        bestTarget = GetBestTarget();
        if (bestTarget != null && lastBestTarget != bestTarget)
        {
            lastBestTarget = bestTarget;
            var data = headFocus.data.sourceObjects;
            data.Clear();
            data.SetTransform(0, bestTarget.transform);
            data.Add(new WeightedTransform(bestTarget.transform, 1));
            headFocus.data.sourceObjects = data;
            rigs.Build();
                       
        }
    }

    private AiAgent GetBestTarget()
    {
        float closest = 9999f;
        AiAgent target = null;

        foreach (AiAgent agent in targets)
        {
            float distance = Vector3.Distance(agent.transform.position, gameObject.transform.position);
            if (distance < closest)
            {
                closest = distance;
                target = agent;
            }
        }

        return target;
    }

}
