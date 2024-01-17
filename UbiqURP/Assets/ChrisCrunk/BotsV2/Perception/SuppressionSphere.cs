using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuppressionSphere : MonoBehaviour
{
    public bool suppressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator coroutine;

    void HitByRay()
    {
        Debug.Log("I was hit by a Ray");
        suppressed = true;
        
        StopCoroutine("WaitItOut");
        StartCoroutine("WaitItOut");
    }

    IEnumerator WaitItOut()
    {
        yield return new WaitForSeconds(3f);
        suppressed=false;
    }
}
