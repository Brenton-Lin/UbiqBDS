using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Ubiq.Messaging;

public class LocalBotControl : MonoBehaviour
{
    public Transform testTarget;
    public Transform botEyes;
    NavMeshAgent agent;
    Animator animator;

    public float checkInterval = 1;
    public NetworkContext context;
    public bool owner;
    public bool dead = false;

    // for use in AiState
    public bool isFarFromPlayer = true;

    float timer = 0.0f;

    Vector3 directionToPlayer;
    float angle;

    // just using this for the demo, way better ways to do it later.
    public float detectionDistance = 5.0f;
    public bool detectByDistance = false;

    public bool detectByFov = true;
    public float fieldOfViewAngle = 30f;



    // Start is called before the first frame update
    void Start()
    {


        context = NetworkScene.Register(this);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        directionToPlayer = testTarget.position - transform.position;
        angle = Vector3.Angle(directionToPlayer, botEyes.transform.forward);
    }

    private struct Message
    {
        public bool clearOwners;
        public Vector3 agentVelocity;
    }

    // remote bot listener
    // agentVelocity: speed of remote bot
    // clearOwners: clears local 'owner' variable to prevent recursive remote messages
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
        // check if bot needs to move to a destination
        if (!dead)
        {
            if (detectByDistance)
            {
                timer -= Time.deltaTime;
                if (timer < 0.0f)
                {
                    // get distance from bot to player
                    float sqDistance = (testTarget.position - agent.transform.position).sqrMagnitude;
                    if (sqDistance < detectionDistance * detectionDistance)
                    {
                        // move to player if in detection range
                        agent.destination = testTarget.position;

                    }
                    timer = checkInterval;

                }
                animator.SetFloat("AgentSpeed", agent.velocity.magnitude);
            }

            if (detectByFov)
            {
                directionToPlayer = testTarget.position - transform.position;
                angle = Vector3.Angle(directionToPlayer, botEyes.transform.forward);

                if (angle < fieldOfViewAngle)
                {
                    agent.destination = testTarget.position;
                }
            }
        }
        else
        {
            agent.velocity = new Vector3(0,0,0);
        }
        

        // update remote agents
        if (agent.velocity.magnitude != 0)
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
        // tell remote agents to stop, since navmesh doesn't restart calculations unless zeroed out
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
