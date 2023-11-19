using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

public class SwapGrip : MonoBehaviour
{
    public BooleanAction x;
    public GameObject objectToFlip;
    public GameObject timerIndication;

    private bool xHeld;
    private bool timerActive;

    // timer for flipping
    float currentTime = 0.0f;
    bool readyToFlip = false;

    // Start is called before the first frame update
    void Start()
    {
        xHeld = false;
        timerActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (x.Value && !timerActive)
        {
            Debug.Log("Holding");
            xHeld = true;
            timerActive = true;
            StartCoroutine(ScaleOverTime(0.5f, 0.5f));
            StartCoroutine(WaitThenFlip(1f));
        }
        else if (!x.Value)
        {
            xHeld = false;
        }
    }
    IEnumerator WaitThenFlip(float time)
    {
        float localTime = 0.0f;
        do
        {
            localTime += Time.deltaTime;
            yield return null;
            if (xHeld && localTime >= time && readyToFlip)
            {
                // rotate object 180
                
                xHeld = false;
                localTime = 0.0f;
                readyToFlip = false;
            }

        } while (localTime <= time);


        yield return null;

    }

    IEnumerator ScaleOverTime(float time, float wait)
    {
        Debug.Log("Entered ScaleOverTime");
        Vector3 originalScale = new Vector3(0, 0, 0); // Starting scale, usually (0, 0, 0)
        Vector3 targetScale = new Vector3(0.5992818f, 0.5992818f, 0.5992818f); // Target scale

        currentTime = 0.0f;

        do
        {
            // scale to 0 when button released early
            if (!xHeld)
            {
                timerIndication.transform.localScale = originalScale;
                currentTime = 0.0f;
                break;
            }

            // scale over time
            timerIndication.transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
            currentTime += Time.deltaTime;

            if (currentTime >= time)
                readyToFlip = true;

            yield return null;
        } while (currentTime <= time);

        yield return new WaitForSeconds(wait);
        // scale to 0 when timer finishes

        if (readyToFlip)
            objectToFlip.transform.Rotate(180, 0, 0);

        readyToFlip = false;

        timerIndication.transform.localScale = originalScale;
        timerActive = false;


        yield return null;

    }

    // if x is held for 2 seconds, start timer
    // timer is a transparent sphere blowing up
    // when timer is done, rotate Object 180 degrees
}
