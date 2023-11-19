using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.XR;


namespace Ubiq.Avatars{
    public class VRTKGripAvatarHintsProvider : AvatarHintProvider
    {
        // Start is called before the first frame update
        public VRTKHandController controller;

        // Update is called once per frame
        public override float ProvideFloat()
        {
            return controller ? controller.GripValue : 0.0f;
        }
    }
}
