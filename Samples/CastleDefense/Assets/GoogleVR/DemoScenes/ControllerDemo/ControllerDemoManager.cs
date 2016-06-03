// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissioßns and
// limitations under the License.

using UnityEngine;
using UnityEngine.UI;

public class ControllerDemoManager : MonoBehaviour {
  public GameObject controllerPivot;
  public GameObject messageCanvas;
  public Text messageText;

  public Material cubeInactiveMaterial;
  public Material cubeHoverMaterial;
  public Material cubeActiveMaterial;

  private Renderer controllerCursorRenderer;

  // Currently selected GameObject.
  private GameObject selectedObject;

  // True if we are dragging the currently selected GameObject.
  private bool dragging;

  // to compute raycast from camera instead of origin
  public GameObject playerCamera;

  // to point weapon towards crosshair
  public GameObject weapon; 

    void Awake() {
  }

  void Update() {
    if (weapon != null)
            UpdateWeapon();
    UpdatePointer();
    UpdateStatusMessage();
  }

  private void UpdateWeapon()
    {
        weapon.transform.rotation = GvrController.Orientation;
        weapon.transform.Rotate(90f, 0f, 0f);
    }

  private void UpdatePointer() {
    if (GvrController.State != GvrConnectionState.Connected) {
      controllerPivot.SetActive(false);
    }

    controllerPivot.SetActive(true);
    controllerPivot.transform.rotation = GvrController.Orientation;

        Vector3 castOrigin = Vector3.zero;
        if (playerCamera != null)
            castOrigin = playerCamera.transform.position;

    if (dragging) {
      if (GvrController.TouchUp) {
        EndDragging();
      }
    } else {
      RaycastHit hitInfo;
      Vector3 rayDirection = GvrController.Orientation * Vector3.forward;
      if (Physics.Raycast(castOrigin, rayDirection, out hitInfo)) {
        if (hitInfo.collider && hitInfo.collider.gameObject) {
          SetSelectedObject(hitInfo.collider.gameObject);
        }
      } else {
        SetSelectedObject(null);
      }
      if (GvrController.TouchDown && selectedObject != null) {
        StartDragging();
      }
    }
  }

  private void SetSelectedObject(GameObject obj) {

    if (selectedObject == null && obj != null)
      Debug.Log("Pointing GVRController at : " + obj.name);


    if (null != selectedObject) {
            if (selectedObject.GetComponent<Renderer>() != null)
                selectedObject.GetComponent<Renderer>().material = cubeInactiveMaterial;
    }
    if (null != obj) {
            if (obj.GetComponent<Renderer>() != null)
              obj.GetComponent<Renderer>().material = cubeHoverMaterial;
    }
    selectedObject = obj;
  }

  private void StartDragging() {
    dragging = true;
        if (selectedObject.GetComponent<Renderer>() != null)
            selectedObject.GetComponent<Renderer>().material = cubeActiveMaterial;

        else return;

    // Reparent the active cube so it's part of the ControllerPivot object. That will
    // make it move with the controller.
    selectedObject.transform.SetParent(controllerPivot.transform, true);
  }

  private void EndDragging() {
    dragging = false;
        if (selectedObject.GetComponent<Renderer>() != null)
            selectedObject.GetComponent<Renderer>().material = cubeHoverMaterial;

    // Stop dragging the cube along.
    selectedObject.transform.SetParent(null, true);
  }

  private void UpdateStatusMessage() {
    // This is an example of how to process the controller's state to display a status message.
    switch (GvrController.State) {
      case GvrConnectionState.Connected:
        messageCanvas.SetActive(false);
        break;
      case GvrConnectionState.Disconnected:
        messageText.text = "Controller disconnected.";
        messageText.color = Color.white;
        messageCanvas.SetActive(true);
        break;
      case GvrConnectionState.Scanning:
        messageText.text = "Controller scanning...";
        messageText.color = Color.cyan;
        messageCanvas.SetActive(true);
        break;
      case GvrConnectionState.Connecting:
        messageText.text = "Controller connecting...";
        messageText.color = Color.yellow;
        messageCanvas.SetActive(true);
        break;
      case GvrConnectionState.Error:
        messageText.text = "ERROR: " + GvrController.ErrorDetails;
        messageText.color = Color.red;
        messageCanvas.SetActive(true);
        break;
      default:
        // Shouldn't happen.
        Debug.LogError("Invalid controller state: " + GvrController.State);
        break;
    }
  }
}
