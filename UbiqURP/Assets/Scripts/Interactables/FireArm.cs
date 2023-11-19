using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.Samples;
using Ubiq.Spawning;
using Ubiq.XR;
using UnityEngine;

public class FireArm : NetworkedGrabbable, IUseable
{
    public bool fire;
    public ParticleSystem effects;
    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();
        effects = GetComponentInChildren<ParticleSystem>();
    }

    public void UnUse(Hand controller)
    {
        fire = false;
    }

    public void Use(Hand controller)
    {
        fire = true;
    }

    // Update is called once per frame
    public new struct Message
    {
        public TransformMessage transform;
        public bool ownership;
        public bool fire;

        public Message(Transform transform, bool ownership, bool fire)
        {
            this.transform = new TransformMessage(transform);
            this.ownership = ownership;
            this.fire = fire;
        }
    }

    // Update is called once per frame
    public new void Update()
    {
       base.Update();
       if(fire)
        {
            effects.Play();
        }
    }

    public new void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>();
        transform.localPosition = msg.transform.position; // The Message constructor will take the *local* properties of the passed transform.
        transform.localRotation = msg.transform.rotation;
        owner = msg.ownership;
        fire = msg.fire;
    }
}
