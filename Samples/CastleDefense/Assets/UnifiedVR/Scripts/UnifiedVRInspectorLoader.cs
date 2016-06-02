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
using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public enum AudioListenerToUse
{
    Daydream,
    Oculus
}

public class UnifiedVRInspectorLoader : MonoBehaviour
{

    [Tooltip("Makes sure there is only 1 audio listener in the scene")]
    public AudioListenerToUse audioListenerToUse;

    [Tooltip("Allow the player to close the UI")]
    public bool allowClose = true;
    [Tooltip("Panel which will be added to the main UI when it is loaded")]
    public Transform donorPanel;
    [Tooltip("You may want to delay summoning the menu a small amount of time to allow for physics to settle the players position. -1 means don't summon")]
    public float summonMenuDelay = 0.65f;

    [Header("Events")]
    [SerializeField]
    public UnifiedVRInspector.InspectorShowEvent onInspectorShow = new UnifiedVRInspector.InspectorShowEvent();

    [SerializeField]
    public UnifiedVRInspector.InspectorHideEvent onInspectorHide = new UnifiedVRInspector.InspectorHideEvent();

    // Use this for initialization
    void Awake()
    {
        var inspectorLoaders = GameObject.FindObjectsOfType(typeof(OVRInspectorLoader));
        if (inspectorLoaders.Length < 0) GameObject.FindObjectsOfType(typeof(UnifiedVRInspectorLoader));

        if (inspectorLoaders.Length > 1)
        {
            Debug.LogError("More than 1 InspectorLoader in scene");
        }

        if (!UnifiedVRInspector.instance)
        {
            UnifiedVRInspector inspectorPrefab = (UnifiedVRInspector)Resources.Load("Prefabs/UnifiedVRInspector", typeof(UnifiedVRInspector));
            Instantiate(inspectorPrefab).name = "UnifiedVRInspector";
        }

        // Register event handlers
        UnifiedVRInspector.instance.onInspectorShow = onInspectorShow;
        UnifiedVRInspector.instance.onInspectorHide = onInspectorHide;
        // Add our context to the main UI
        UnifiedVRInspector.instance.LoadSceneSpecificContextsFromPanel(donorPanel);

        UnifiedVRInspector.instance.allowClose = allowClose;

        UnifiedVRInspector.audioListenerToUse = audioListenerToUse;
    }

    void Start()
    {
        if (summonMenuDelay > 0)
        {
            StartCoroutine(DelayedMenuSummon());
        }
        else if (summonMenuDelay == 0)
            UnifiedVRInspector.instance.Show();
    }

    IEnumerator DelayedMenuSummon()
    {
        yield return new WaitForSeconds(summonMenuDelay);
        UnifiedVRInspector.instance.Show();
        // Now we don't need this anymore, disable it to reduce the draw call overhead
        gameObject.SetActive(false);
    }
}
