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

    // team management
    [Header("Team management\n")]
    public bool friendly;
    public bool isDead = false;

    public bool isInFormation = false;

    
    
    public AiEyes eyes;
    public int scanFrequency = 30;
    
    public List<AiAgent> potentialVisualTargets;

    // visual targeting
    [Header("Eyes\n")]
    [Tooltip("Arbitary text message")]
    public AiAgent bestTarget;
    public AiAgent lastBestTarget;

    // navigation
    [Header("Navigation\n")]
    public NavMeshAgent navMeshAgent;
    public List<GameObject> botPath;
    public Transform homeBase;
    public Transform lastLocationOfEnemy;

    // cover taking
    // public CoverObject closestCover;


    // sound hearing
    [Header("Hearing\n")]
    public AiEars ears;

    public List<AudioSource> heardSounds;
    public AudioSource mostNoticedSound = null;
    public AudioSource lastNoticedSound = null;

    // detecting suppressing fire
    SuppressionSphere suppressionSphere;

    // rig layers and constraints

    [Header("Constraints\n")]
    public RigBuilder rigs;
    public BotPoseConstraints poseConstraints;

        // animator
        Animator animator;

    // gun
    public Transform gunTip;
    public ReloadingRifle rifle;

    // state control
    [Header("StateMachine\n")]
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public string currentState;

    public bool swapToCurrentStateMidPlay = false;

    // Networking

    NetworkContext context;
    [Header("Network Vars\n")]
    public bool owner;
    bool isGuard;
    bool isAim;


    // Start is called before the first frame update
    void Start()
    {

        eyes = GetComponent<AiEyes>();

        // animation and riggimmmmng
        animator = GetComponent<Animator>();
        rigs = gameObject.GetComponent<RigBuilder>();
        poseConstraints = GetComponent<BotPoseConstraints>();

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        suppressionSphere = gameObject.GetComponent<SuppressionSphere>();

        stateMachine = new AiStateMachine(this);

        stateMachine.RegisterState(new AiGuard());
        stateMachine.RegisterState(new AiPatrol());
        stateMachine.RegisterState(new AiCombat());
        stateMachine.RegisterState(new AiInvestigate());
        stateMachine.RegisterState(new AiFlee());
        stateMachine.RegisterState(new AiMessageComrades());

        stateMachine.ChangeState(initialState);

        context = NetworkScene.Register(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState = stateMachine.currentState.ToString();

        if (ears != null)
            mostNoticedSound = GameObject.Find("BotSoundPerceptionHub").GetComponent<BotSoundPerceptionHub>().GetMostNoticedSound(this);

        if (eyes != null)
        {
            // find all targets in range
            potentialVisualTargets = eyes.ScanForEnemies(friendly);

            // get best target 
            bestTarget = GetBestTarget(gameObject);
        }


        
        if (swapToCurrentStateMidPlay)
        {
            swapToCurrentStateMidPlay = false;
            stateMachine.ChangeState(initialState);
        }

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
            // look toward sound -- potentialVisualTargets may update eh
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

        foreach (AiAgent agent in potentialVisualTargets)
        {
            if (!eyes.IsInSight(agent.gameObject))
                continue;

            float distance = Vector3.Distance(agent.transform.position, source.transform.position);
            if (distance < closest)
            {
                closest = distance;
                target = agent;
            }
        }

        return target;
    }


    /***************************************************************************************************************************************
    *--/\/--/\/--/\/--/\/--/\/--/\/--/\/--/\/--/\/--/\/--/\/-      Networking      -/\/--/\/--/\/--/\/--/\/--/\/--/\/--/\/--/\/--/\/--/\/--*
    ***************************************************************************************************************************************/


    public void NetworkBots(int mess)
    {

        // set ActivateToEnableBotNetworkCalls to active in hierarchy to enable networking
        if (GameObject.Find("ActivateToEnableBotNetworkCalls") == null)
            return;

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
                            isDead = false,
                            move = true
                        });;
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
                            isDead = false,
                            move = true
                        }); ;
                    }
                }
                break;

                // killSS
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
                        clearOwners = false,
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
                        clearOwners = false,
                        isAim = true
                    });

                }
                break;
        }


    }

    public NetworkId Id { get; } = new NetworkId();

    private struct Message
    {
        public bool clearOwners;
        public Vector3 navAgentVelocity;
        public Vector3 destination;
        public bool isGuard;
        public bool isAim;

        public bool isDead;

        public bool move;
    }


    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var m = message.FromJson<Message>();

        if (m.move)
        {
            navMeshAgent.velocity = m.navAgentVelocity;
            navMeshAgent.destination = m.destination;
        }
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
            stateMachine.ChangeState(AiStateId.Combat);
            //poseConstraints.NetworkReceiver(2);
            Debug.Log("GotAim");
        }
        if (m.isGuard)
        {
            stateMachine.ChangeState(AiStateId.Guard);
            //poseConstraints.NetworkReceiver(3);
            Debug.Log("GotGuard");
        }
    }

    public void SetOwner() { owner = true; }


}
