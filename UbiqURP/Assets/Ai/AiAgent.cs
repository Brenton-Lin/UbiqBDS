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
    public bool friendly;

    // senses
    public AiSensor senses;
    float scanInterval;
    float scanTimer;
    public int scanFrequency = 30;
    public List<AiAgent> targets;

    // animator
    Animator animator;

    // rig layers
    public MultiAimConstraint eyeConstraint;
    public DampedTransform eyeConstraintDamp;
    public MultiAimConstraint weaponAimConstraint;
    

    public RigBuilder rigs;

    public AiAgent bestTarget;
    public AiAgent lastBestTarget;

    public Transform gunTip;
    public ReloadingRifle rifle;

    public AiStateMachine stateMachine;
    public AiStateId initialState;

    public SuppressionSphere suppressionSphere;

    // Start is called before the first frame update
    void Start()
    {

        senses = GetComponent<AiSensor>();

        animator = GetComponent<Animator>();
        rigs = gameObject.GetComponent<RigBuilder>();

        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiShoot());

        stateMachine.ChangeState(initialState);
        
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        if (suppressionSphere != null && suppressionSphere.suppressed)
        {
            animator.SetBool("emergeFromCover", false);
            animator.SetBool("takeCover", true);
            
        }
        if (suppressionSphere != null && !suppressionSphere.suppressed && animator.GetBool("takeCover") == true)
        {
            animator.SetBool("takeCover", false);
            animator.SetBool("emergeFromCover", true);
        }

    }



}
