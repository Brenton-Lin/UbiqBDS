using nickmaltbie.OpenKCC.UI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationStateManager : MonoBehaviour
{
    
    public Animator lowerBodyAnimator;
    public float speed;
    public Vector3 worldVelocity;
    private Vector3 previousPosition;
    // Start is called before the first frame update
    void Start()
    {
        //lowerBodyController = GetComponentInChildren<AnimatorController>();
        previousPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        updateSpeed();
        //lowerBodyAnimator.SetFloat
    }

    void updateSpeed()
    { 

        worldVelocity = (transform.position - previousPosition) / Time.deltaTime;
        speed = worldVelocity.magnitude;
        previousPosition = transform.position;
    }

  


}
