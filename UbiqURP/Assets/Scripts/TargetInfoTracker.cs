using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInfoTracker : MonoBehaviour
{
    public int countedHits;


    private void OnEnable()
    {
        TrackedShootingTarget.OnHit += UpdateHits;
    }
    void UpdateHits()
    {
        countedHits += 1;
    }
}
