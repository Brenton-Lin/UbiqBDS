using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class AiEars : MonoBehaviour
{

    public float hearingDistance;
    public float reactionTime;

    public float hearingSensitivity;

    public AudioSource[] audioSources = new AudioSource[50];

    public int sampleWindow = 64;

    public float updateStep = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // when a Sound is played, get its intensity of sound

    // calculate sound strength from source to ear

    // vector directional

    // if above threshold, register

    // stretch : sound classification



    //// considerations:
    /// 1 AudioListener per scene
    /// Multiple Ubiq instances
    /// /// AudioListener centered on player (one per instance)
    /// 
    /// Sound is played centered on a source
    /// 




    // get loudest sound

    // 2 sounds

    // get volumes and divide by distance to choose 

    // case when a target is visually seen, but loud sound


    // note audio clip for bullet has loudest start but explosion should be more noticeable

    // due to how it samples it is based on probability that the peak is reached in the window

}
