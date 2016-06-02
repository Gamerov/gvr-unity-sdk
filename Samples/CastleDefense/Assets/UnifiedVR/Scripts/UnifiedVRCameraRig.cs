/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.3 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculus.com/licenses/LICENSE-3.3

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR = UnityEngine.VR;

public class UnifiedVRCameraRig : OVRCameraRig
{
    protected override Transform ConfigureEyeAnchor(Transform root, VR.VRNode eye)
    {
        string eyeName = (eye == VR.VRNode.CenterEye) ? "Center" : (eye == VR.VRNode.LeftEye) ? "Left" : "Right";
        string name = eyeName + eyeAnchorName;

        Transform head = transform.Find(root.name + "/Head");

        if (head == null)
        {
            head = new GameObject("Head").transform;
            head.name = "Head";
            head.parent = root;
            head.localScale = Vector3.one;
            head.localPosition = Vector3.zero;
            head.localRotation = Quaternion.identity;
        }

        Transform anchor = transform.Find(root.name + "/" + head.name + "/" + name);

        if (anchor == null)
        {
            anchor = transform.Find(name);
        }

        if (anchor == null)
        {
            string legacyName = legacyEyeAnchorName + eye.ToString();
            anchor = transform.Find(legacyName);
        }

        if (anchor == null)
        {
            anchor = new GameObject(name).transform;
        }

        anchor.name = name;
        anchor.parent = head;
        anchor.localScale = Vector3.one;
        anchor.localPosition = Vector3.zero;
        anchor.localRotation = Quaternion.identity;

        return anchor;
    }

}
