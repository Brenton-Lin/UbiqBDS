using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class NetworkedRifle : NetworkedObject
{

    public NetworkContext context;
    public bool owner;
    public bool use;
    private Rigidbody rb;
    [SerializeField]
    public RifleAndSnapReload gun;
    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
        rb = GetComponent<Rigidbody>();
    }

    Vector3 lastPosition;

    // Update is called once per frame
    void Update()
    {

        if (lastPosition != transform.localPosition)
        {

            lastPosition = transform.localPosition;
            if (owner)
            {
                context.SendJson(new Message()
                {
                    position = transform.localPosition,
                    rotation = transform.localRotation,
                    clearOwners = false,
                    isKinematic = true,
                    use = use
                });
            }
            else
            {

            }

        }
        else
        {


        }

        use = false;
        
    }

    private struct Message
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool clearOwners;
        public bool isKinematic;
        public bool use;
    }

    public void DoUse() 
    {
        Debug.Log("Trigger pulled");     
        if (gun.magInGun)
        {
            Debug.Log("Shot");
            gun.Shoot();
            use = true;
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        // Parse the message
        var m = message.FromJson<Message>();

        // Use the message to update the Component
        transform.localPosition = m.position;
        transform.localRotation = m.rotation;
        owner = m.clearOwners;
        // Make sure the logic in Update doesn't trigger as a result of this message
        lastPosition = transform.localPosition;
        if (rb != null)
        {
            rb.isKinematic = m.isKinematic;
        }

        use = m.use;
        Debug.Log("Got message. Use value: " + m.use);
        if (m.use) 
        { 
            DoUse();
            Debug.Log("Shot over network");
        }
    }



}
