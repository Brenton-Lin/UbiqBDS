using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverObject : MonoBehaviour
{
    public int capacity;

    public List<AiAgent> agents;

    // Start is called before the first frame update
    void Start()
    {
        agents = new List<AiAgent>(capacity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
