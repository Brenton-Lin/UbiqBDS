using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerClientFlag : MonoBehaviour
{
    public static ServerClientFlag Instance { get; private set; }

    public bool isServer;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
