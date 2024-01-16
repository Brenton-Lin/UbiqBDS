using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BotPoseConstraints : MonoBehaviour
{

    [SerializeField] TwoBoneIKConstraint leftHandConstraint;
    [SerializeField] TwoBoneIKConstraint rightHandConstraint;
    [SerializeField] MultiAimConstraint headConstraint;

    [SerializeField] MultiAimConstraint weaponAimConstraint;
    [SerializeField] Rig weaponAimLayer;
    [SerializeField] Rig headLayer;

    AiAgent agent;
    AiAgent lastBestTarget;

    AudioSource lastNoticedSound;


    public enum PoseId
    {
        None,
        Idle,
        Aim
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<AiAgent>();
        lastBestTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DirectHead(Transform target)
    {
        headLayer.weight = 1.0f;
        // head constraints
        var data = headConstraint.data.sourceObjects;
        data.Clear();
        data.SetTransform(0, target.transform);
        data.Add(new WeightedTransform(target.transform, 1f));
        headConstraint.data.sourceObjects = data;

        /*            Debug.Log(agent.eyeConstraintDamp.data.sourceObject);
                    agent.eyeConstraintDamp.data.sourceObject = bestTarget.transform;
                    Debug.Log(bestTarget.transform);*/

        agent.rigs.Build();

        data.Clear();

    }

    public void DirectArms()
    {

    }

    public void DirectWeapon(Transform target)
    {
        weaponAimLayer.weight = 1f;
        // weapon aim constraint
        var data = weaponAimConstraint.data.sourceObjects;
        data.Clear();

        data.SetTransform(0, target.transform);
        data.Add(new WeightedTransform(target.transform, 0.4f));
        weaponAimConstraint.data.sourceObjects = data;

        agent.rigs.Build();

        data.Clear();
    }

    public void CombatPose()
    {

    }

    public void ClearAim()
    {
        var data = weaponAimConstraint.data.sourceObjects;
        data.Clear();

        data = headConstraint.data.sourceObjects;
        data.Clear();
        
        weaponAimLayer.weight = 0f;
    }

    public void IdlePose()
    {
        headLayer.weight = 0f;
        weaponAimLayer.weight = 0f;
        
    }

    public void RebuildRig()
    {
        agent.rigs.Build();
    }

    public void NetworkReceiver(int pose)
    {
        switch (pose)
        {
            case 0:
                break;
            case 1:
                IdlePose();
                break;
            case 2:
                DirectHead(agent.bestTarget.transform);
                DirectWeapon(agent.bestTarget.transform);
                break;
        }
    }

}
