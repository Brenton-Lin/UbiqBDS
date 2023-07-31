using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.XR;

public class PlayerController : XRPlayerController
{
    public float gravityValue = -9.8f;

    private bool groundedPlayer;
    public Vector3 playerVelocity;
    private CharacterController controller;
    // Start is called before the first frame update
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!UnityEngine.XR.XRSettings.isDeviceActive)
        {
            enabled = false;
        }
        foreach (var item in handControllers)
        {
            if (item.Right)
            {
                if (item.JoystickSwipe.Trigger)
                {
                    transform.RotateAround(headCamera.transform.position, Vector3.up, 45f * Mathf.Sign(item.JoystickSwipe.Value));
                }
            }
            else if (item.Left)
            {
                var dir = item.Joystick.normalized;
                var mag = item.Joystick.magnitude;
                if (mag > joystickDeadzone)
                {
                    var speedMultiplier = Mathf.InverseLerp(joystickDeadzone, 1.0f, mag);
                    var worldDir = headCamera.transform.TransformDirection(dir.x, 0, dir.y);
                    worldDir.y = 0;
                    var distance = joystickFlySpeed * worldDir.normalized;
                    controller.Move(distance*Time.deltaTime);
                }
            }
        }
        OnGround();


        var headProjectionXZ = transform.InverseTransformPoint(headCamera.transform.position);
        headProjectionXZ.y = 0;
        userLocalPosition.x += (headProjectionXZ.x - userLocalPosition.x) * Time.deltaTime * cameraRubberBand.Evaluate(Mathf.Abs(headProjectionXZ.x - userLocalPosition.x));
        userLocalPosition.z += (headProjectionXZ.z - userLocalPosition.z) * Time.deltaTime * cameraRubberBand.Evaluate(Mathf.Abs(headProjectionXZ.z - userLocalPosition.z));
        userLocalPosition.y = 0;
        //controller.transform.position = userLocalPosition;
    }

    private void OnGround()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }


    // Update is called once per frame

}
