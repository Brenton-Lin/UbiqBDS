using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;


public class AiAgent : MonoBehaviour
{
    public bool changeStateInPlayMode = false;

    // team selection
    public bool friendly;
    public bool isDead = false;

    // eyes
    public AiEyes eyes;
    public int scanFrequency = 30;

    public List<AiAgent> seeableTargets;

    // navigation
    public NavMeshAgent navMeshAgent;

    // cover taking
    public CoverObject closestCover;

    // sound eyes
    public AiEars ears;
    public float hearingSensitivity;

    public List<AudioSource> heardSounds;
    public AudioSource mostNoticedSound = null;
    public AudioSource lastNoticedSound = null;

    // detecting suppressing fire
    public SuppressionSphere suppressionSphere;

    // rig layers and constraints

    public RigBuilder rigs;

    public MultiAimConstraint eyeConstraint;
    public DampedTransform eyeConstraintDamp;
    public MultiAimConstraint weaponAimConstraint;

    public BotPoseConstraints poseConstraints;

        // animator
        Animator animator;

    // targeting
    public AiAgent bestTarget;
    public AiAgent lastBestTarget;

    // gun
    public Transform gunTip;
    public ReloadingRifle rifle;

    // state control
    public AiStateMachine stateMachine;
    public AiStateId initialState;

    public AiStateId currentState;

    public bool isPatrolling;

    public List<GameObject> botPath;

    // Networking

    NetworkContext context;
    public bool owner;
    public bool isGuard;
    public bool isAim;

    public NetworkId Id { get; } = new NetworkId();

    private struct Message
    {
        public bool clearOwners;
        public Vector3 navAgentVelocity;
        public Vector3 destination;
        public bool isGuard;
        public bool isAim;

        public bool isDead;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var m = message.FromJson<Message>();

        navMeshAgent.velocity = m.navAgentVelocity;
        navMeshAgent.destination = m.destination;
        owner = m.clearOwners;
        isDead = m.isDead;
        isAim = m.isAim;
        isGuard = m.isGuard;


        if (isDead)
        {
            BotHealth health = GetComponent<BotHealth>();
            health.DoDamage(health.currentHealth);
        }

/*        if (m.pose != 0)
        {
            poseConstraints.NetworkReceiver(m.pose);
            Debug.Log("Got a pose: " + m.pose);
        }*/

        if (m.isAim)
        {
            poseConstraints.NetworkReceiver(2);
            Debug.Log("GotAim");
        }
        if (m.isGuard)
        {
            poseConstraints.NetworkReceiver(3);
            Debug.Log("GotGuard");
        }
    }

    public void SetOwner() { owner = true; }

    // Start is called before the first frame update
    void Start()
    {
        // initialize new eyes and ears with the sensitivity parameters


        eyes = GetComponent<AiEyes>();

        animator = GetComponent<Animator>();
        rigs = gameObject.GetComponent<RigBuilder>();
        poseConstraints = GetComponent<BotPoseConstraints>();

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        stateMachine = new AiStateMachine(this);

        /*        stateMachine.RegisterState(new AiShoot());
                stateMachine.RegisterState(new AiTakeCover());*/
        stateMachine.RegisterState(new AiGuard());
        stateMachine.RegisterState(new AiPatrol());
        stateMachine.RegisterState(new AiCombat());

        stateMachine.ChangeState(initialState);

        context = NetworkScene.Register(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState = stateMachine.currentState;

        if (ears != null)
            mostNoticedSound = GameObject.Find("BotSoundPerceptionHub").GetComponent<BotSoundPerceptionHub>().GetMostNoticedSound(this);

        if (eyes != null)
        {
            // find all targets in range
            seeableTargets = eyes.ScanForEnemies(friendly);

            // get best target 
            bestTarget = GetBestTarget(gameObject);
        }


        if (changeStateInPlayMode)
            stateMachine.ChangeState(initialState);

        // let controller bot handle this
        if (!owner || isDead)
            return;

        // closestCover = FindObjectOfType<CoverManager>().GetClosestCover(gameObject);


        // AiAgent performs duties while watching for enemies 

        // Default to Duty state

        // Constantly Look and Listen
        // Change state when enemy is heard or seen
        // Change state when told to by Leader bot

        // look at a target
        if (bestTarget != null)
        {
            // stateMachine.ChangeState(AiStateId.Shoot);
        }

        if (stateMachine.currentState == AiStateId.Patrol)
        {
            // look toward sound -- seeableTargets may update eh
            // bot will investigate if nothing seen
        }

        // take cover from start
        if (suppressionSphere != null && suppressionSphere.suppressed)
        {
            animator.SetBool("emergeFromCover", false);
            animator.SetBool("takeCover", true);

            stateMachine.ChangeState(AiStateId.TakeCover);
            
        }

        // emerge after taking cover
        if (suppressionSphere != null && !suppressionSphere.suppressed && animator.GetBool("takeCover") == true)
        {
            animator.SetBool("takeCover", false);
            animator.SetBool("emergeFromCover", true);
        }

        stateMachine.Update();

    }

    private AiAgent GetBestTarget(GameObject source)
    {
        float closest = 9999f;
        AiAgent target = null;

        foreach (AiAgent agent in seeableTargets)
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


    public void NetworkBots(int mess)
    {
        switch (mess)
        {
            case 1:
                // move bots
                if (navMeshAgent.velocity.magnitude != 0)
                {
                    if (owner)
                    {
                        context.SendJson(new Message
                        {

                            navAgentVelocity = navMeshAgent.velocity,
                            destination = navMeshAgent.destination,
                            clearOwners = false,
                            isDead = false
                        });
                    }
                }
                // tell remote navAgents to stop, since navmesh doesn't restart calculations unless zeroed out
                if (navMeshAgent.velocity.magnitude == 0)
                {
                    if (owner)
                    {
                        context.SendJson(new Message
                        {
                            navAgentVelocity = navMeshAgent.velocity,
                            destination = navMeshAgent.destination,
                            clearOwners = false,
                            isDead = false
                        });
                    }
                }
                break;

                // kill
            case 2:
                if (owner)
                {
                    context.SendJson(new Message
                    {
                        clearOwners = false,
                        isDead = true
                    });
                }
                break;

                // pose idle
            case 3:
                if (owner)
                {
                    context.SendJson(new Message
                    {
                        clearOwners = true,
                        //isAim = false,
                        isGuard = true
                    });
                    
                }
                break;

                // pose combat
            case 4:
                if (owner)
                {
                    context.SendJson(new Message
                    {
                        clearOwners = true,
                        isAim = true,
                        isGuard = false
                    });

                }
                break;
        }


    }

}
