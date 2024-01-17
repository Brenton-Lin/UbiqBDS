using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAliasFollower : MonoBehaviour
{
    // Helper script to have an Parent object track whatever object you're using to offset movement in the slide locomotor
    //this can then be used to provide an accurate offset for the SnapRotate locomotor
    //you can add an offset if needed, but 0,0,0 by default.
    public Transform objectToTrack;
    
    void Update()
    {
        transform.position = objectToTrack.position;
        transform.rotation = objectToTrack.rotation;

    }

}
