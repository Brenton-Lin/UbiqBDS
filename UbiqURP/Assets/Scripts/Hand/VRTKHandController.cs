using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using Valve.VR;
using UnityEngine;
using static UnityEngine.SpatialTracking.TrackedPoseDriver;
using Tilia.SDK.SteamVR.Input;
using Zinnia.Action;

public class VRTKHandController : HandController
{
    public BooleanAction desktopTrigger;
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
        //No longer a need for Ubiq's desktop controls
        /*if (desktopTrigger.Value)
        {
            TriggerState = desktopTrigger.Value;
        }*/
        
        TriggerState = triggerButtonState.Value;
        
        GripValue = gripState.Value;
        
        //Debug.Log("VRTK Controller Update");
    }

    private void FixedUpdate()
    {
        
    }
}
