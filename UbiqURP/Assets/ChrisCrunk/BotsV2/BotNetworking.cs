using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;


public class BotNetworking : MonoBehaviour
{
    public bool isOwner;
    private NetworkContext context;

    void Start()
    {
        // destroy and wait for Owner to spawn its own dummy bot
/*        if (!isOwner)
        {
            Destroy(this.gameObject);
            Destroy(this);
        }*/
        context = NetworkScene.Register(this);
        isOwner = ServerClientFlag.Instance.isServer;

    }

    void Update()
    {
        if (isOwner)
        {
            context.SendJson(new BotTransformMessage(transform, false));
        }
    }

    // message structure, transform and owner bool
    public struct BotTransformMessage
    {
        public TransformMessage transform;
        public bool ownership;

        public BotTransformMessage(Transform transform, bool ownership)
        {
            this.transform = new TransformMessage(transform);
            this.ownership = false;
        }
    }

    // transform message receiving
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<BotTransformMessage>();
        transform.localPosition = msg.transform.position; // The Message constructor will take the *local* properties of the passed transform.
        transform.localRotation = msg.transform.rotation;
        isOwner = msg.ownership;
    }

}
