using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Ubiq.Messaging;

public class LocalBotControl : MonoBehaviour
{
    public Transform testTarget;
    NavMeshAgent agent;
    Animator animator;
    public float checkInterval = 1;
    public NetworkContext context;
    public bool owner;
    public bool dead = false;

    float timer = 0.0f;

    //just using this for the demo, way better ways to do it later.
    public float detectionDistance = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private struct Message
    {
        public bool clearOwners;
        public Vector3 agentVelocity;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var m = message.FromJson<Message>();

        agent.velocity = m.agentVelocity;
        owner = m.clearOwners;
    }

    public void SetOwner() { owner = true; }

    // Update is called once per frame
    void Update()
    {
        if (owner && !dead)
        {
            timer -= Time.deltaTime;
            if (timer < 0.0f)
            {
                float sqDistance = (testTarget.position - agent.transform.position).sqrMagnitude;
                if (sqDistance < detectionDistance * detectionDistance)
                {
                    agent.destination = testTarget.position;
                }
                timer = checkInterval;

            }
            animator.SetFloat("AgentSpeed", agent.velocity.magnitude);
        }
        else
        {
            agent.velocity = new Vector3(0,0,0);
        }
        

        //update remote agents
        if(agent.velocity.magnitude != 0)
        {
            if (owner)
            {
                context.SendJson(new Message
                {
                    agentVelocity = agent.velocity,
                    clearOwners = false
                });
            }
        }
        //tell remote agents to stop, since navmesh doesn't restart calculations unless zeroed out
        if (agent.velocity.magnitude == 0)
        {
            if (owner)
            {
                context.SendJson(new Message
                {
                    agentVelocity = agent.velocity,
                    clearOwners = false
                });
            }
        }

    }
}
