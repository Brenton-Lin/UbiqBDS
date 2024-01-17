using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    public float sphereCheckRadius;
    public LayerMask layers;

    int count = 0;

    List<CoverObject> AllCover;

    // Start is called before the first frame update
    void Start()
    {
        // find all objects in the scene that can be used as cover
        FindAllCover();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindAllCover()
    {
        AllCover = new List<CoverObject>(FindObjectsOfType<CoverObject>());
    }

    // called by bots in AiAgent, used in AiTakeCover (namespace workaround)
    public CoverObject GetClosestCover(GameObject coverSeeker)
    {
        CoverObject closestCover = null;

        // find closest cover
        foreach (CoverObject cover in AllCover)
        {
            if (closestCover == null)
            {
                closestCover = cover;
                continue;
            }

            // compare distance from coverSeeker to best cover, with current cover
            if (Vector3.Distance(cover.transform.position, coverSeeker.transform.position) >
                Vector3.Distance(closestCover.transform.position, coverSeeker.transform.position))
            {
                closestCover = cover;
            }
        }

        return closestCover;
    }
}
