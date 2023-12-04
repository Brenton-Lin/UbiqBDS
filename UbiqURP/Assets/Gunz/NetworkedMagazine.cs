using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;

public class NetworkedMagazine : NetworkedObject
{
    [SerializeField]
    private Magazine mag;

    public NetworkContext context;
    public bool owner;
    public bool use;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
        rb = GetComponent<Rigidbody>();
    }

    Vector3 lastPosition;

    void Update()
    {
/*        Debug.Log("Last position: " + lastPosition);
        Debug.Log("CurrentPosition: " + transform.position);*/

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

    }

    private struct Message
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool clearOwners;
        public bool isKinematic;
        public bool use;
    }

    public virtual void DoUse() { }

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
    }

}
