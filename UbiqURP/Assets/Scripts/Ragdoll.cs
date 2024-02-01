using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        if (GetComponent<Animator>())
            animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    // Update is called once per frame
    public void DeactivateRagdoll()
    {
        foreach(var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        if(animator != null)
            animator.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        if (animator != null)
            animator.enabled = false;
    }
}
