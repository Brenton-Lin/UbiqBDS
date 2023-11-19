using System.Collections;
using System.Collections.Generic;
using Ubiq.Avatars;
using Ubiq.Samples;
using UnityEngine;
using VRArmIK;

public class SoldierAvatar : MonoBehaviour
{
    public Transform head;
    public Transform torso;
    public Transform leftHand;
    public Transform rightHand;
    public bool isLocal;
    public Ubiq.Avatars.Avatar avatar;
   
    /*
    public Renderer torsoRenderer;
    public Renderer leftHandRenderer;
    public Renderer rightHandRenderer;
    */
    public Renderer headRenderer;
    public Renderer legRenderer;
    public Renderer leftHandRenderer;
    public Renderer rightHandRenderer;

    public Transform baseOfNeckHint;

    // public float torsoFacingHandsWeight;
    //public AnimationCurve torsoFootCurve;

    //public AnimationCurve torsoFacingCurve;

    private TexturedAvatar texturedAvatar;
    private ThreePointTrackedAvatar trackedAvatar;
    private Vector3 footPosition;
    private Quaternion torsoFacing;

    private void OnEnable()
    {
        trackedAvatar = GetComponentInParent<ThreePointTrackedAvatar>();
        avatar = GetComponentInParent<Ubiq.Avatars.Avatar>();

        if (trackedAvatar)
        {
            trackedAvatar.OnHeadUpdate.AddListener(ThreePointTrackedAvatar_OnHeadUpdate);
            trackedAvatar.OnLeftHandUpdate.AddListener(ThreePointTrackedAvatar_OnLeftHandUpdate);
            trackedAvatar.OnRightHandUpdate.AddListener(ThreePointTrackedAvatar_OnRightHandUpdate);
        }

        texturedAvatar = GetComponentInParent<TexturedAvatar>();

        if (texturedAvatar)
        {
            texturedAvatar.OnTextureChanged.AddListener(TexturedAvatar_OnTextureChanged);
        }
        StartCoroutine(SetLocalVisuals());
        

    }

    private void OnDisable()
    {
        if (trackedAvatar && trackedAvatar != null)
        {
            trackedAvatar.OnHeadUpdate.RemoveListener(ThreePointTrackedAvatar_OnHeadUpdate);
            trackedAvatar.OnLeftHandUpdate.RemoveListener(ThreePointTrackedAvatar_OnLeftHandUpdate);
            trackedAvatar.OnRightHandUpdate.RemoveListener(ThreePointTrackedAvatar_OnRightHandUpdate);
        }

        if (texturedAvatar && texturedAvatar != null)
        {
            texturedAvatar.OnTextureChanged.RemoveListener(TexturedAvatar_OnTextureChanged);
        }
    }

    private void ThreePointTrackedAvatar_OnHeadUpdate(Vector3 pos, Quaternion rot)
    {
        head.position = pos;
        head.rotation = rot;
    }

    private void ThreePointTrackedAvatar_OnLeftHandUpdate(Vector3 pos, Quaternion rot)
    {
        leftHand.position = pos;
        leftHand.rotation = rot;
    }

    private void ThreePointTrackedAvatar_OnRightHandUpdate(Vector3 pos, Quaternion rot)
    {
        rightHand.position = pos;
        rightHand.rotation = rot;
    }

    private void TexturedAvatar_OnTextureChanged(Texture2D tex)
    {
        //headRenderer.material.mainTexture = tex;
        //torsoRenderer.material = headRenderer.material;
        //leftHandRenderer.material = headRenderer.material;
        //rightHandRenderer.material = headRenderer.material;
    }

    private void Update()
    {
        //Not the most efficient placement, but the IsLocal flag is only set in the Avatar Manager, after the avatar is spawned and enabled
        //Maybe make a method that we can call from the Avatar manager.
        

    }
    private void Start()
    {
        
    }

    IEnumerator SetLocalVisuals()
    {
        yield return new WaitForSeconds(1);
        if (avatar.IsLocal)
        {
            isLocal = true;
            headRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            legRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            ArmTransforms[] arms = GetComponentsInChildren<ArmTransforms>();
            foreach(var arm in arms)
            {
                arm.handObject.SetActive(false);
                
            }

        }
    }




    // private Vector3 handsFwdStore;

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawLine(head.position, footPosition);
    //     // Gizmos.DrawLine(head.position,head.position + handsFwdStore);
    // }
}

