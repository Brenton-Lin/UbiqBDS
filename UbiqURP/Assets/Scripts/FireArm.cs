using System.Collections;
using System.Collections.Generic;
using Ubiq.Samples;
using Ubiq.Spawning;
using Ubiq.XR;
using Unity.VisualScripting;
using UnityEngine;

public class FireArm : MonoBehaviour, IUseable, IGraspable
{
    public bool owner;
    private Hand follow;
    private Rigidbody body;
    // Start is called before the first frame update
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
       
    }
    public void Grasp(Hand controller)
    {
        follow = controller;
    }

    public void Release(Hand controller)
    {
        follow = null;
    }

    public void UnUse(Hand controller)
    {

    }

    public void Use(Hand controller)
    {
        
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
            //body.isKinematic = false;
            //now toggles in NetworkedObject to try and fix rubberbanding on remote;
            //body.useGravity = true;
        }
    }
}
