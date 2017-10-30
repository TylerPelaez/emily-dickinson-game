using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

	GameObject heldObject;

	// Use this for initialization
	void Start () {
		heldObject = null;
	}

	void centerObjectInCamera() {
		if (heldObject != null) {
			Vector3 posVec = new Vector3 (transform.position.x + transform.forward.x, transform.position.y + transform.forward.y, transform.position.z + transform.forward.z);
			heldObject.transform.position = posVec;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Interact")) {
			if (heldObject == null) {
				// Try to pick up an object
				RaycastHit hitInfo;
				if (Physics.Raycast (transform.position, transform.forward, out hitInfo, 1.0f, LayerMask.GetMask("Player"))) {
					Debug.Log ("Fire");
					Debug.Log (hitInfo.collider.gameObject.name);

					if (hitInfo.collider.gameObject.GetComponent<Pickupable> ()) {
						GameObject other = hitInfo.collider.gameObject;
						heldObject = Instantiate (other, gameObject.transform);
						Destroy (hitInfo.collider.gameObject);
						centerObjectInCamera ();
					}
				}
			} else {
				// Drop Object
			}
		}
	}
}
