using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.XR;
using SIPSorceryMedia.Abstractions;
using Ubiq.Rooms.Messages;

public class Grabbable : MonoBehaviour, IGraspable
{
    private NetworkedObject network;
    private Hand follow;
    private Rigidbody body;
    // Start is called before the first frame update
    private void Awake()
    { 
        body = GetComponent<Rigidbody>();
        network = GetComponent<NetworkedObject>();
    }

    public void Grasp(Hand controller)
    {
        follow = controller;
        if(network != null)
        {
            //this should work for now, but how to clear owners on new owner?
            network.owner = true;
           
        }
       
    }

    public void Release(Hand controller)
    {
        follow = null;
        //body.useGravity = true;
        //network.context.SendJson(new Message() { });
    }

    // Update is called once per frame
    void Update()
    {
        if (follow != null)
        {
            transform.position = follow.transform.position;
            transform.rotation = follow.transform.rotation;
            body.isKinematic = true;
            body.useGravity = false;
        }
        else
        {
            body.isKinematic = false;
            //now toggles in NetworkedObject to try and fix rubberbanding on remote;
            //body.useGravity = true;
        }
    }
}
