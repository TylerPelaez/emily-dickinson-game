﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

	Transform cameraTransform;
	GameObject heldObject;
	const float HOLD_DISTANCE = 2.0f;
	const float INTERACT_DISTANCE = 3.0f;


	// Use this for initialization
	void Start () {
		heldObject = null;
		cameraTransform = gameObject.GetComponentInChildren<Camera> ().gameObject.transform;
	}

	void centerObjectInCamera() {
		if (heldObject != null) {
			Vector3 posVec = new Vector3 (cameraTransform.position.x + (cameraTransform.forward.x * HOLD_DISTANCE), cameraTransform.position.y + (cameraTransform.forward.y * HOLD_DISTANCE), cameraTransform.position.z + (cameraTransform.forward.z * HOLD_DISTANCE));
			heldObject.transform.position = posVec;
			heldObject.transform.eulerAngles = new Vector3 (0f, cameraTransform.rotation.eulerAngles.y, 0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Interact")) {
			if (heldObject == null) {
				// Try to pick up an object
				RaycastHit hitInfo;
				// Debug.Log ("Fire");
				// Debug.DrawRay (transform.position, transform.forward * INTERACT_DISTANCE, Color.green, 5.0f);
				if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hitInfo, INTERACT_DISTANCE, LayerMask.GetMask("Default"))) {
					
					Debug.Log (hitInfo.collider.gameObject.name);

					if (hitInfo.collider.gameObject.GetComponent<Pickupable> ()) {
						heldObject = hitInfo.collider.gameObject;
						heldObject.GetComponent<Rigidbody> ().useGravity = false;
						heldObject.GetComponent<Rigidbody> ().detectCollisions = false;
					}
				}
			} else {
				// Drop Object
				heldObject.GetComponent<Rigidbody> ().useGravity = true;
				heldObject.GetComponent<Rigidbody> ().detectCollisions = true;
				heldObject = null;
			}
		}
		centerObjectInCamera ();
	}
}
