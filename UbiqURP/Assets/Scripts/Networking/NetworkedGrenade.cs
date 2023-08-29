using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkedGrenade : NetworkedObject
{
    // Start is called before the first frame update
    public GrenadeMechanism grenade;

    // Update is called once per frame
    public override void DoUse()
    {
        StartCoroutine(grenade.explode());
        
    }
}
