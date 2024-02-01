using System.Collections;
using System.Collections.Generic;
using Ubiq.Avatars;
using Ubiq.Messaging;
using Ubiq.Samples;
using UnityEngine;
using UnityEngine.UIElements;
using VRArmIK;
using static UnityEngine.UI.GridLayoutGroup;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class SoldierAvatar : MonoBehaviour
{
   
    public Transform head;
    public Transform torso;
    public Transform leftHand;
    public Transform rightHand;
    public bool isLocal;
    public Ubiq.Avatars.Avatar avatar;
    public NetworkContext context;


    //Stuff for dynamic avatar scaling, clean this up later...
    public GameObject avatarVisuals;
    public Transform torsoScaleHandle;
    public Transform legScaleHandle;
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
    //change this to track custom 3 point avatar with scale state.!!!!!
    private CustomThreePointAvatar trackedAvatar;
    private Vector3 footPosition;
    private Quaternion torsoFacing;

    private void OnEnable()
    {
        trackedAvatar = GetComponentInParent<CustomThreePointAvatar>();
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
    private struct Message
    {
        public float scale;
    }

   
    IEnumerator SetLocalVisuals()
    {
        //ensure visuals run AFTER avatar script sets isLocal Flag
        yield return new WaitForSeconds(1);
        //disable visuals before scaling.
       
        if (avatar.IsLocal)
        {
            isLocal = true;
            headRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            legRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            ArmTransforms[] arms = GetComponentsInChildren<ArmTransforms>();
            foreach(var arm in arms)
            {
                //disable remote avatar hands on the local client
                arm.handObject.SetActive(false);
                
            }

        }
        //scale avatar components before enabling.
        torsoScaleHandle.localScale = trackedAvatar.avatarScale;
        legScaleHandle.localScale = trackedAvatar.avatarScale;
        //actually enable gameObj
        avatarVisuals.SetActive(true);
    }




    // private Vector3 handsFwdStore;

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawLine(head.position, footPosition);
    //     // Gizmos.DrawLine(head.position,head.position + handsFwdStore);
    // }
}

