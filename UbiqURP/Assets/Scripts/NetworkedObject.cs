

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;

public class NetworkedObject : MonoBehaviour
{
    public NetworkContext context;
    public bool owner;
    private Rigidbody rb;
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
                    clearOwners = false
                }) ;
            }
            else
            {
               // rb.useGravity = false;
            }
            
        }
    }

    private struct Message
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool clearOwners;
        public bool useGravity;
    }
    public void SetOwner() { owner = true; }
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

        //Try to trigger rb gravity based on message
        
        rb.useGravity = m.useGravity;
       
    }
}
