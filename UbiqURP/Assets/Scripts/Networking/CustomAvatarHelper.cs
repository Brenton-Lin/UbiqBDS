using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.XR;
using Zinnia.Tracking.CameraRig;
using Tilia.CameraRigs.TrackedAlias;

namespace Ubiq.Avatars
{
    [RequireComponent(typeof(CustomAvatarManager))]
    public class CustomAvatarHelper : MonoBehaviour
    {
        [SerializeField] private string headPositionNode = "HeadPosition";
        [SerializeField] private string headRotationNode = "HeadRotation";
        [SerializeField] private string leftHandPositionNode = "LeftHandPosition";
        [SerializeField] private string leftHandRotationNode = "LeftHandRotation";
        [SerializeField] private string leftWristPositionNode = "LeftWristPosition";
        [SerializeField] private string leftWristRotationNode = "LeftWristRotation";
        [SerializeField] private string rightHandPositionNode = "RightHandPosition";
        [SerializeField] private string rightHandRotationNode = "RightHandRotation";
        [SerializeField] private string rightWristPositionNode = "RightWristPosition";
        [SerializeField] private string rightWristRotationNode = "RightWristRotation";
        [SerializeField] private string leftGripNode = "LeftGrip";
        [SerializeField] private string rightGripNode = "RightGrip";

        private void Start()
        {
            //Finds the VRTK player controller rather than the Ubiq
            var pcs = FindObjectsOfType<TrackedAliasFacade>(includeInactive: false);

            if (pcs.Length == 0)
            {
                Debug.LogWarning("No VRTK player controller found, assuming desktop client!");
                //let Avatar manager know to use the desktop avatar prefab.
                GetComponent<CustomAvatarManager>().SetVRFlag(false);
                
                //no VRTK controller found, so we should have disabled VRTK and enabled a desktop client
                //now setup the transform providers so that all nodes are just the main camera.
                var desktopCamera = Camera.main.transform;
                SetTransformProvider(headPositionNode, headRotationNode, desktopCamera);
                SetTransformProvider(leftHandPositionNode, leftHandRotationNode, desktopCamera);
                SetTransformProvider(leftWristPositionNode, leftWristRotationNode, desktopCamera);
                SetTransformProvider(rightHandPositionNode, rightHandRotationNode, desktopCamera);
                SetTransformProvider(rightWristPositionNode, rightWristRotationNode, desktopCamera);
            }
            else if (pcs.Length > 1)
            {
                Debug.LogWarning("Multiple VRTK player controllers found. Using: " + pcs[0].name);
            }
            else //Just one player controller found
            {
                var pc = pcs[0];
                //Sets the transform to track for the head, the transform of the first camera object in ubiq players found in scene.
                //Now we're just searching for a Unity Camera so the code should work from here.
                SetTransformProvider(headPositionNode, headRotationNode,
                    pc.transform.Find("Aliases/HeadsetAlias"));

                var hcs = pc.GetComponentsInChildren<VRTKHandController>();

                GetLeftHand(hcs, out var leftHand, out var leftWrist, out var leftHc);
                SetTransformProvider(leftHandPositionNode, leftHandRotationNode, leftHand);
                SetTransformProvider(leftWristPositionNode, leftWristRotationNode, leftWrist);
                SetGripProvider(leftGripNode, leftHc);

                GetRightHand(hcs, out var rightHand, out var rightWrist, out var rightHc);
                SetTransformProvider(rightHandPositionNode, rightHandRotationNode, rightHand);
                SetTransformProvider(rightWristPositionNode, rightWristRotationNode, rightWrist);
                SetGripProvider(rightGripNode, rightHc);
            }
        }

        private void GetLeftHand(VRTKHandController[] handControllers,
            out Transform hand, out Transform wrist, out VRTKHandController handController)
        {
            if (handControllers != null && handControllers.Length > 0)
            {
                foreach (var hc in handControllers)
                {
                    if (hc.Left)
                    {
                        hand = hc.transform.Find("UbiqHints/Hand");
                        wrist = hc.transform.Find("UbiqHints/Wrist");
                        handController = hc;
                        return;
                    }
                }
            }
            hand = null;
            wrist = null;
            handController = null;
        }

        private void GetRightHand(VRTKHandController[] handControllers,
            out Transform hand, out Transform wrist, out VRTKHandController handController)
        {
            if (handControllers != null && handControllers.Length > 0)
            {
                foreach (var hc in handControllers)
                {
                    if (hc.Right)
                    {
                        hand = hc.transform.Find("UbiqHints/Hand");
                        wrist = hc.transform.Find("UbiqHints/Wrist");
                        handController = hc;
                        return;
                    }
                }
            }
            hand = null;
            wrist = null;
            handController = null;
        }

        private void SetTransformProvider(string posNode, string rotNode, Transform transform)
        {
            if (posNode == string.Empty && rotNode == string.Empty)
            {
                return;
            }

            if (!transform)
            {
                Debug.LogWarning("Could not find a transform hint source. Has the Ubiq player prefab changed?");
                return;
            }

            var hp = gameObject.AddComponent<TransformAvatarHintProvider>();
            var manager = GetComponent<CustomAvatarManager>();
            hp.hintTransform = transform;
            if (posNode != string.Empty)
            {
                manager.hints.SetProvider(posNode, AvatarHints.Type.Vector3, hp);
            }
            if (rotNode != string.Empty)
            {
                manager.hints.SetProvider(rotNode, AvatarHints.Type.Quaternion, hp);
            }
        }

        private void SetGripProvider(string node, VRTKHandController handController)
        {
            if (node == string.Empty)
            {
                return;
            }

            if (!handController)
            {
                Debug.LogWarning("Could not find a grip hint source. Has the Ubiq player prefab changed?");
                return;
            }

            var hp = gameObject.AddComponent<VRTKGripAvatarHintsProvider>();
            var manager = GetComponent<CustomAvatarManager>();
            hp.controller = handController;
            manager.hints.SetProvider(node, AvatarHints.Type.Float, hp);
        }
    }
}
