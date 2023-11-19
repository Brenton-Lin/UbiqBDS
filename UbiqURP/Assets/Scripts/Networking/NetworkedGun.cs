using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedGun : PhysicsObject
{
    [SerializeField]
    private SimpleGun gun;
    // Start is called before the first frame update

    // Update is called once per frame
   
    public override void DoUse()
    {
        gun.Shoot();
    }
}
