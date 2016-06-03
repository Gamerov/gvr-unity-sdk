/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UnifiedVRInspector : OVRInspector
{
    // Input module
    private UnifiedVRInputModule inputModule;

    [HideInInspector]
    public static AudioListenerToUse audioListenerToUse;

    public override void OnAwakeOrLevelLoad()
    {
        base.OnAwakeOrLevelLoad();
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();

        inputModule = eventSystem.GetComponent<UnifiedVRInputModule>();
        eventSystem.GetComponent<UnifiedVRInputModule>().rayTransform = cameraRig.centerEyeAnchor;


        // disable other audio listeners
        if (audioListenerToUse == AudioListenerToUse.Daydream)
        {
            ///cameraRig.centerEyeAnchor.GetComponent<AudioListener>().enabled = false;
        }

        else
        {

        }

    }

    override protected void FindPlayerAndCamera()
    {
        base.FindPlayerAndCamera();

        if (cameraRig)
        {
            Transform t = cameraRig.transform.FindChild("TrackingSpace").FindChild("Head");
            centerEyeTransform = t.FindChild("CenterEyeAnchor");

        }
    }
}
