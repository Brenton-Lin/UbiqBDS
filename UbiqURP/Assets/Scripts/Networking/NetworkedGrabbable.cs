using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;
using Ubiq.XR;
using Ubiq.Samples;

public class NetworkedGrabbable : MonoBehaviour, IGraspable
{
    private Hand attached;
    private Rigidbody body;
    private NetworkContext context;

    public bool owner;

    public void Awake()
    {
        body = GetComponent<Rigidbody>();
        owner = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
        //body.isKinematic = true;
    }

    public void Grasp(Hand controller)
    {
        attached = controller;
        owner = true;
    }

    public void Release(Hand controller)
    {
        attached = null;
    }

    public struct Message
    {
        public TransformMessage transform;
        public bool ownership;

        public Message(Transform transform, bool ownership)
        {
            this.transform = new TransformMessage(transform);
            this.ownership = false;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (attached)
        {
            transform.position = attached.transform.position;
            transform.rotation = attached.transform.rotation;
            body.isKinematic = true;
            context.SendJson(new Message(transform, false));
        }
        if(owner && !attached)
            {
                body.isKinematic = false;
                context.SendJson(new Message(transform, false));
            }
        else
        {
            body.isKinematic= true;
        }

    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>();
        transform.localPosition = msg.transform.position; // The Message constructor will take the *local* properties of the passed transform.
        transform.localRotation = msg.transform.rotation;
        owner = msg.ownership;
    }
}
