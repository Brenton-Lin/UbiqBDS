using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Ubiq.Messaging;

public class LocalBotControl : MonoBehaviour
{
    public Transform targetDestination;

    NavMeshAgent navAgent;
    Animator animator;

    public float checkInterval = 0.1f;
    public NetworkContext context;
    public bool owner;
    public bool dead = false;

    // for use in AiState
    public bool isFarFromPlayer = true;

    float timer = 0.0f;

    Vector3 directionToTarget;
    float angle;

    // just using this for the demo, way better ways to do it later.
    public float detectionDistance = 5.0f;
    public bool detectByDistance = false;

    public bool detectByFov = true;
    public float fieldOfViewAngle = 30f;

    public List<GameObject> botPath;
    int numPathNodes = 0;
    int pathIterator = 0;

    bool isStopped = false;

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        directionToTarget = targetDestination.position - transform.position;
        //angle = Vector3.Angle(directionToTarget, botEyes.transform.forward);

        numPathNodes = botPath.Count;
    }

    private struct Message
    {
        public bool clearOwners;
        public Vector3 navAgentVelocity;
    }

    // remote bot listener
    // navAgentVelocity: speed of remote bot
    // clearOwners: clears local 'owner' variable to prevent recursive remote messages
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var m = message.FromJson<Message>();

        navAgent.velocity = m.navAgentVelocity;
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
                    // get distance from bot to destination
                    float sqDistance = (targetDestination.position - navAgent.transform.position).sqrMagnitude;
                    if (sqDistance < detectionDistance * detectionDistance)
                    {
                        // move to destination if in detection range
                        navAgent.destination = targetDestination.position;

                    }
                    timer = checkInterval;

                    // if distance from spot to bot is 0 and bot's speed is 0, iterate to next test target
                    if (Vector3.Distance(targetDestination.position, navAgent.transform.position) <= 0.1 || navAgent.velocity.magnitude <= 1f && !isStopped)
                    {
                        pathIterator++;

                        if (pathIterator == numPathNodes)
                            pathIterator = 0;

                        targetDestination = botPath[pathIterator].transform;

                        navAgent.destination = targetDestination.position;
                    }

                    TravelNode nodeArgs = targetDestination.GetComponent<TravelNode>();

                    if (nodeArgs.additionalWaitTime > 0)
                    {
                        isStopped = true;   
                        navAgent.isStopped = true;
                        StartCoroutine(WaitForSeconds(nodeArgs.additionalWaitTime));
                    }
                    


                }
                animator.SetFloat("navAgentSpeed", navAgent.velocity.magnitude);
            }

        }
        else
        {
            navAgent.velocity = new Vector3(0,0,0);
        }
        

        // update remote navAgents
        if (navAgent.velocity.magnitude != 0)
        {
            if (owner)
            {
                context.SendJson(new Message
                {
                    navAgentVelocity = navAgent.velocity,
                    clearOwners = false
                });
            }
        }
        // tell remote navAgents to stop, since navmesh doesn't restart calculations unless zeroed out
        if (navAgent.velocity.magnitude == 0)
        {
            if (owner)
            {
                context.SendJson(new Message
                {
                    navAgentVelocity = navAgent.velocity,
                    clearOwners = false
                });
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Navigation")
        {
            pathIterator++;
            if (pathIterator == numPathNodes)
                pathIterator = 0;

            targetDestination = botPath[pathIterator].transform;

            navAgent.destination = targetDestination.position;



            Debug.Log("Hit sphere");

        }
    }

    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        navAgent.isStopped = false;
        isStopped = false;
    }
}
