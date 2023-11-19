using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedShootingTarget : MonoBehaviour
{

    public delegate void HitAction();
    public static event HitAction OnHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitTarget()
    {
        OnHit();
    }
}
