using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using Ubiq.XR;
using UnityEngine;
using Zinnia.Action;
using Extensions = System.Xml.Linq.Extensions;

public class BDSPlayerController : MonoBehaviour
{

    public bool dontDestroyOnLoad = true;

    private static BDSPlayerController singleton;

    public static BDSPlayerController Singleton
    {
        get { return singleton; }
    }
    
    
    public FloatAction verticalAxis;
    public FloatAction horizontalAxis;
    public Camera headCamera;
    public AnimationCurve cameraRubberBand;
    public Vector3 velocity;
    public Vector3 userLocalPosition;

    public float joystickDeadzone = 0.1f;
    public float joystickFlySpeed = 1.2f;

    private void Awake()
    {
        if (dontDestroyOnLoad)
        {
            if (singleton != null)
            {
                gameObject.SetActive(false);
                DestroyImmediate(gameObject);
                return;
            }

            singleton = this;
            DontDestroyOnLoad(gameObject);
         
        }

        
    }

    private void OnGround()
    {
        var height = Mathf.Clamp(transform.InverseTransformPoint(headCamera.transform.position).y, 0.1f, float.PositiveInfinity);
        var origin = transform.position + userLocalPosition + Vector3.up * height;
        var direction = Vector3.down;

        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(origin, direction), out hitInfo))
        {
            var virtualFloorHeight = hitInfo.point.y;

            if (transform.position.y < virtualFloorHeight)
            {
                transform.position += Vector3.up * (virtualFloorHeight - transform.position.y) * Time.deltaTime * 3f;
                velocity = Vector3.zero;
            }
            else
            {
                velocity += Physics.gravity * Time.deltaTime;
            }
        }
        else
        {
            velocity = Vector3.zero; // if there is no 'ground' in the scene, then do nothing
        }

        transform.position += velocity * Time.deltaTime;
    }

    private void Update()
    {
        Vector2 joystickInput = new Vector2(horizontalAxis.Value, verticalAxis.Value);

        var dir = joystickInput.normalized;
        var mag = joystickInput.magnitude;
        OnGround();
        if (mag > joystickDeadzone)
        {
            var speedMultiplier = Mathf.InverseLerp(joystickDeadzone, 1.0f, mag);
            var worldDir = headCamera.transform.TransformDirection(dir.x, 0, dir.y);
            worldDir.y = 0;
            var distance = (joystickFlySpeed * Time.deltaTime);
            transform.position += distance * worldDir.normalized;
        }
        var headProjectionXZ = transform.InverseTransformPoint(headCamera.transform.position);
        headProjectionXZ.y = 0;
        userLocalPosition.x += (headProjectionXZ.x - userLocalPosition.x) * Time.deltaTime * cameraRubberBand.Evaluate(Mathf.Abs(headProjectionXZ.x - userLocalPosition.x));
        userLocalPosition.z += (headProjectionXZ.z - userLocalPosition.z) * Time.deltaTime * cameraRubberBand.Evaluate(Mathf.Abs(headProjectionXZ.z - userLocalPosition.z));
        userLocalPosition.y = 0;
    }
}
