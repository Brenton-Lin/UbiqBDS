using nickmaltbie.OpenKCC.UI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateManager : MonoBehaviour
{
    
    public Animator lowerBodyAnimator;
    public float speed;
    public float updateInterval;
    public Vector3 worldVelocity;
    public Vector3 xzVelocity;

    private Vector3 previousPosition;
    private float timeRemaining;
    
    // Start is called before the first frame update
    void Start()
    {
        //lowerBodyController = GetComponentInChildren<AnimatorController>();
        previousPosition = Vector3.zero;
        timeRemaining = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining <= 0)
        {
            updateSpeed();
            timeRemaining = updateInterval;
        }
        else
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    void updateSpeed()
    { 

        worldVelocity = (transform.position - previousPosition) / updateInterval;

        //zero out movement in y axis, so that crouching/rising doesn't trigger walking animations
        xzVelocity = new Vector3(worldVelocity.x, 0, worldVelocity.z);
        speed = xzVelocity.magnitude;
        lowerBodyAnimator.SetFloat("WorldSpeed", speed);
        previousPosition = transform.position;
    }

    //establish a threshold, as a percentage of some height of the avatar, that constitutes crouching

    //source a crouch walking animation

    //apply to movements while crouched.


}
