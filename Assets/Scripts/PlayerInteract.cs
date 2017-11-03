using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {

	const float HOLD_DISTANCE = 2.0f;
	const float INTERACT_DISTANCE = 3.0f;
	const float HOLD_DISTANCE_BUFFER = 0.1f;

	// External References being found at start or at runtime
	private Transform cameraTransform;
	private GameObject heldObject;
	private RigidbodyConstraints heldObjectConstraints;

	private Text pickupTextObject;

	private PlayerCam playerCamScript;

	// Use this for initialization
	void Start () {
		heldObject = null;
		cameraTransform = gameObject.GetComponentInChildren<Camera> ().gameObject.transform;
		playerCamScript = gameObject.GetComponent<PlayerCam> ();

		pickupTextObject = GameObject.Find ("PickupText").GetComponent<Text>();
	}

	public void centerObjectInCamera() {
		if (heldObject != null) {
			RaycastHit hit;
			Vector3 newHeldObjectPos;

			if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hit, HOLD_DISTANCE, LayerMask.GetMask ("Terrain"))) {
				Vector3 objectExtent = heldObject.GetComponent<Collider> ().bounds.size;

				newHeldObjectPos = new Vector3 (hit.point.x - (cameraTransform.forward.x * objectExtent.x + HOLD_DISTANCE_BUFFER),
									  			hit.point.y - (cameraTransform.forward.y * objectExtent.y + HOLD_DISTANCE_BUFFER), 
									  			hit.point.z - (cameraTransform.forward.z * objectExtent.z + HOLD_DISTANCE_BUFFER));
			} else {
				newHeldObjectPos = new Vector3 (cameraTransform.position.x + (cameraTransform.forward.x * HOLD_DISTANCE),
									  			cameraTransform.position.y + (cameraTransform.forward.y * HOLD_DISTANCE),
					   				  			cameraTransform.position.z + (cameraTransform.forward.z * HOLD_DISTANCE));
			}

			// Neurotically trying to stop held object from jittering
			heldObject.transform.position = newHeldObjectPos;
			heldObject.GetComponent<Rigidbody> ().position = newHeldObjectPos;
			heldObject.transform.eulerAngles = new Vector3 (0f, cameraTransform.rotation.eulerAngles.y, 0f);
		}
	}
	
	// Update is called once per frame
	public void updateInteractLogic () {
		if (heldObject == null) {
			// Player isn't holding an object
			RaycastHit hitInfo;
			if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hitInfo, INTERACT_DISTANCE, LayerMask.GetMask ("Pickupable"))) {
				// There's an object that can be picked up in front of the player.
				if (Input.GetButtonDown ("Interact")) {
					pickupObject (hitInfo.collider.gameObject);
				} else {
					pickupTextObject.enabled = true;
					pickupTextObject.text = "Click to Pick up.";
				}
			} else {
				pickupTextObject.text = "";
				pickupTextObject.enabled = false;
			}
		} else if (Input.GetButtonDown ("Interact")) {
			Transform cameraLerpTransform = dropHeldObject ();

			if (cameraLerpTransform != null) {
				// Held Object snapped to something, lets lerp to the expected spot
				playerCamScript.beginLerpToLockPos(cameraLerpTransform);
			}
		} else {
			
			pickupTextObject.text = "";
			pickupTextObject.enabled = false;
		}
	}

	private void pickupObject(GameObject obj) {
		heldObject = obj;
		// Store object constraints for future restoration.
		heldObjectConstraints = heldObject.GetComponent<Rigidbody> ().constraints;
		heldObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		heldObject.GetComponent<Rigidbody> ().isKinematic = true;
		heldObject.GetComponent<Default_Obj_Behavior>().held = true;
	}

	private Transform dropHeldObject() {
		Transform cameraLerpTransform = null;
		if (heldObject != null) {
			heldObject.GetComponent<Rigidbody> ().isKinematic = false;
			// Restore object's previous constraints
			heldObject.GetComponent<Rigidbody> ().constraints = heldObjectConstraints;

			cameraLerpTransform = heldObject.GetComponent<Default_Obj_Behavior>().SnapTo();
			heldObject.GetComponent<Default_Obj_Behavior>().held = false;
		}

		heldObject = null;

		return cameraLerpTransform;
	}
}
