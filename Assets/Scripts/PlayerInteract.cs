using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {

	const float HOLD_DISTANCE = .75f;
	const float INTERACT_DISTANCE = 3f;
	const float HOLD_DISTANCE_BUFFER = 0.1f;

	// External References being found at start or at runtime
	private Transform cameraTransform;
	private GameObject heldObject;
    private Rigidbody heldObjectBody;
    private Default_Obj_Behavior heldObjectBhvr;
    private Pivot pivotOfSnappedCrank;
	private RigidbodyConstraints heldObjectConstraints;
    public float objectSpeed;

	private Text pickupTextObject;

	private PlayerCam playerCamScript;

	//Audio
	public AudioSource pickup;

	// Use this for initialization
	void Start () {
		heldObject = null;
		cameraTransform = gameObject.GetComponentInChildren<Camera> ().gameObject.transform;
		playerCamScript = gameObject.GetComponent<PlayerCam> ();

		pickupTextObject = GameObject.Find ("PickupText").GetComponent<Text>();
	}

	public void centerObjectInCamera () {
        if (heldObject != null)
        {
            Vector3 posVec = new Vector3(cameraTransform.position.x + (cameraTransform.forward.x * HOLD_DISTANCE), cameraTransform.position.y + (cameraTransform.forward.y * HOLD_DISTANCE), cameraTransform.position.z + (cameraTransform.forward.z * HOLD_DISTANCE));
            //heldObject.transform.position = posVec;
            float distMod = Vector3.Distance(heldObject.transform.position, posVec);
            heldObjectBody.velocity = (posVec - heldObject.transform.position).normalized * objectSpeed * Time.deltaTime * distMod;
            heldObject.transform.rotation = cameraTransform.rotation;
        }
	}
	
	// Update is called once per frame
	public void updateInteractLogic (bool interactPressed) {
		if (heldObject == null) {
			// Player isn't holding an object
			RaycastHit hitInfo;
			if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hitInfo, INTERACT_DISTANCE, LayerMask.GetMask ("Pickupable"))) {
				// There's an object that can be picked up in front of the player.
				if (interactPressed) {
					pickupObject (hitInfo.collider.gameObject);
				} else {
					pickupTextObject.enabled = true;
					pickupTextObject.text = "Click to Pick up.";
				}
			} else if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hitInfo, INTERACT_DISTANCE, LayerMask.GetMask ("Bird"))) {
				if (interactPressed) {
					hitInfo.collider.gameObject.GetComponent<BirdBehavior> ().toggleStay ();
				} else {
					pickupTextObject.enabled = true;
					pickupTextObject.text = "Click to feed bird";
				}
			} else {
				pickupTextObject.text = "";
				pickupTextObject.enabled = false;
			}
		} else if (interactPressed) {
			CrankTransformManager snappedTransformManager = dropHeldObject ();

			if (snappedTransformManager != null) {
				// Held Object snapped to something, lets lerp to the expected spot
				playerCamScript.beginLerpToLockPos(snappedTransformManager);

			}
		} else {
			
			pickupTextObject.text = "";
			pickupTextObject.enabled = false;
		}
	}

	private void pickupObject(GameObject obj) {
		pickup.Play();
		heldObject = obj;
        // Store object constraints for future restoration.
        heldObjectBody = heldObject.GetComponent<Rigidbody>();
        heldObjectConstraints = heldObjectBody.constraints;
        heldObjectBody.useGravity = false;
        //heldObjectBody.constraints = RigidbodyConstraints.FreezeAll;
        //heldObjectBody.isKinematic = true;
        heldObjectBhvr = heldObject.GetComponent<Default_Obj_Behavior>();
        heldObjectBhvr.held = true;
	}

	private CrankTransformManager dropHeldObject() {
		// If the held object snapped to a crank when it was dropped, return the crankTransformManager of that crank
		// Else, return null.
		CrankTransformManager snappedTo = null;
		if (heldObject != null) {
            //heldObjectBody.isKinematic = false;
            // Restore object's previous constraints
            heldObjectBody.useGravity = true;
            heldObjectBody.constraints = heldObjectConstraints;

			snappedTo = heldObjectBhvr.SnapTo();

            heldObjectBhvr.held = false;
		}

		heldObject = null;

		return snappedTo;
	}
}
