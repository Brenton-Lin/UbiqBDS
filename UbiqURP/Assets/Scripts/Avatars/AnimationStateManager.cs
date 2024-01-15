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
        speed = worldVelocity.magnitude;
        lowerBodyAnimator.SetFloat("WorldSpeed", speed);
        previousPosition = transform.position;
    }

  


}
