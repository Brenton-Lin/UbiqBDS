using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using Valve.VR;
using UnityEngine;
using static UnityEngine.SpatialTracking.TrackedPoseDriver;
using Tilia.SDK.SteamVR.Input;

public class VRTKHandController : HandController
{
    public SteamVRBehaviourBooleanAction triggerButtonState;
    public SteamVRBehaviourFloatAction gripState;
    public SteamVR_Behaviour_Single handSource;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //for Ubiq Networked grip animations
        UpdateGripState();
        //Repurposing Ubiq's UI ray and menu request system.
        
    }

    public new bool Left
    {
        get
        {
            return handSource.inputSource == SteamVR_Input_Sources.LeftHand;
        }
    }

    public new bool Right
    {
        get
        {
            return handSource.inputSource == SteamVR_Input_Sources.RightHand;
        }
    }

    public void UpdateGripState()
    {
        GripValue = gripState.Value;
        TriggerState = triggerButtonState.Value;
        //Debug.Log("VRTK Controller Update");
    }

    private void FixedUpdate()
    {
        
    }
}
