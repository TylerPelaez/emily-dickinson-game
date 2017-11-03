using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {

	Transform cameraTransform;
	GameObject heldObject;
	const float HOLD_DISTANCE = 2.0f;
	const float INTERACT_DISTANCE = 3.0f;
	Text pickupTextObject;
	RigidbodyConstraints heldObjectConstraints;

	// Use this for initialization
	void Start () {
		heldObject = null;
		cameraTransform = gameObject.GetComponentInChildren<Camera> ().gameObject.transform;

		pickupTextObject = GameObject.Find ("PickupText").GetComponent<Text>();
	}

	void centerObjectInCamera() {
		if (heldObject != null) {
			RaycastHit hit;
			Vector3 posVec;
			Vector3 objectExtent = heldObject.GetComponent<Collider> ().bounds.size;

			if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hit, HOLD_DISTANCE, LayerMask.GetMask ("Terrain"))) {
				posVec = new Vector3 (hit.point.x - (cameraTransform.forward.x * objectExtent.x + 0.1f), hit.point.y - (cameraTransform.forward.y * objectExtent.y+ 0.1f), hit.point.z - (cameraTransform.forward.z * objectExtent.z + 0.1f));
			} else {
				posVec = new Vector3 (cameraTransform.position.x + (cameraTransform.forward.x * HOLD_DISTANCE), cameraTransform.position.y + (cameraTransform.forward.y * HOLD_DISTANCE), cameraTransform.position.z + (cameraTransform.forward.z * HOLD_DISTANCE));
				Debug.Log (posVec);
			}
			heldObject.transform.position = posVec;
			heldObject.transform.eulerAngles = new Vector3 (0f, cameraTransform.rotation.eulerAngles.y, 0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (heldObject == null) {
			RaycastHit hitInfo;
			if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hitInfo, INTERACT_DISTANCE, LayerMask.GetMask ("Pickupable"))) {
				if (Input.GetButtonDown ("Interact")) {
					Debug.Log (hitInfo.collider.gameObject.name);

					if (hitInfo.collider.gameObject.GetComponent<Pickupable> ()) {
						heldObject = hitInfo.collider.gameObject;
						heldObject.GetComponent<Rigidbody> ().useGravity = false;
						heldObject.GetComponent<Rigidbody> ().detectCollisions = false;
						heldObjectConstraints = heldObject.GetComponent<Rigidbody> ().constraints;
						heldObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
						heldObject.GetComponent<Default_Obj_Behavior>().held = true;
					}
				} else {
					pickupTextObject.enabled = true;
					pickupTextObject.text = "Click to Pick up.";
				}
			} else {
				pickupTextObject.text = "";
				pickupTextObject.enabled = false;
			}
		} else if (Input.GetButtonDown ("Interact")) {
			// Drop Object
			heldObject.GetComponent<Rigidbody> ().useGravity = true;
			heldObject.GetComponent<Rigidbody> ().detectCollisions = true;
			heldObject.GetComponent<Rigidbody> ().constraints = heldObjectConstraints;
			heldObject.GetComponent<Default_Obj_Behavior>().SnapTo();
			heldObject.GetComponent<Default_Obj_Behavior>().held = false;
			heldObject = null;
		} else {
			
			pickupTextObject.text = "";
			pickupTextObject.enabled = false;
		}
	}

	void FixedUpdate() {
		centerObjectInCamera ();
	}

}
