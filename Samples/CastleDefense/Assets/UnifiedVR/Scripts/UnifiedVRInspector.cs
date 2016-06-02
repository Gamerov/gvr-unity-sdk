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
            cameraRig.centerEyeAnchor.GetComponent<AudioListener>().enabled = false;
        }

        else
        {

        }

        /*
        if (instance != this)
            return;

        OVRManager.display.RecenterPose();

        FindPlayerAndCamera();
        // There has to be an event system for the GUI to work
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.Log("Creating EventSystem");
            eventSystem = (EventSystem)GameObject.Instantiate(eventSystemPrefab);

        }
        else
        {
            //and a UnifiedVRInputModule
            if (eventSystem.GetComponent<UnifiedVRInputModule>() == null)
            {
                eventSystem.gameObject.AddComponent<UnifiedVRInputModule>();
            }
        }

        unifiedVRInputModule = eventSystem.GetComponent<UnifiedVRInputModule>();

        playerController = FindObjectOfType<OVRPlayerController>();
        if (playerController)
        {
            CachePlayerControlDefaults();
        }

        cameraRig.EnsureGameObjectIntegrity();
        eventSystem.GetComponent<UnifiedVRInputModule>().rayTransform = cameraRig.centerEyeAnchor;
        canvas.GetComponent<Canvas>().worldCamera = cameraRig.leftEyeCamera;

        // make sure we have a new fader object
        fader = cameraRig.GetComponentInChildren<OVRScreenFade2>();

        if (fader == null)
        {
            GameObject fadeObj = Instantiate(Resources.Load("Prefabs/UnifiedVRFader", typeof(GameObject))) as GameObject;
            fadeObj.transform.SetParent(cameraRig.centerEyeAnchor, false);
            fader = fadeObj.GetComponent<OVRScreenFade2>();
        }
        fader.PositionForCamera(cameraRig);

        // Make sure legacy fader objects are not present
        if (cameraRig.leftEyeAnchor.GetComponent<OVRScreenFade>() != null ||
            cameraRig.rightEyeAnchor.GetComponent<OVRScreenFade>() != null)
        {
            Debug.LogError("Camera rig has ScreenFade objects");
        }
        */
    }
}
