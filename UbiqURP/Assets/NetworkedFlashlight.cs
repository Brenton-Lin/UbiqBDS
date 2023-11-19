using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedFlashlight : PhysicsObject
{
    [SerializeField]
    private Flashlight flashlight;
    // Start is called before the first frame update
    public override void DoUse()
    {
        // bool toggles on press and release
        flashlight.ClickClick();
        flashlight.ClickClick();
    }
}
