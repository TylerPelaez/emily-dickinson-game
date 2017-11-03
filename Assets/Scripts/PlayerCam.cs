using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerCam : MonoBehaviour
{
	private enum CAMERA_STATE
	{
		FREE_CAM,
		LOCK_CAM,
		LERPING_TO_LOCK_CAM,
		LERPING_TO_FREE_CAM
	};

	private enum CURRENT_INPUT
	{
		INTERACT_DOWN,
		INTERACT_HELD,
		INTERACT_UP,
		CANCEL_INTERACT,
		NONE
	}

	// For moving the camera around in Free cam mode
    public MouseLook camLook;

	// These 2 classes are heavily intermingled. Reduce coupling if there is a chance.
	private PlayerInteract playerInteract;
	private PlayerMove playerMove;

	// For lerping:
	private const float LERP_SPEED = 2.0f;
	private Transform lockPos;
	private CAMERA_STATE cameraState;
	private float startLerpTime;
	private float lerpTimer;
	private Vector3 lerpOrigin;
	private Quaternion lerpRotationOrigin;

	// For turning the crank
	private const int CIRCLE_TURN_CONSISTENCY_THRESHOLD = 10;
	private const int CIRCLE_TURN_CONSISTENCY_MAX = 15;
	Vector2 oldest;
	Vector2 middle;
	Vector2 newest;
	int consistentTurnCount;

	// Sun revolution
	const float SUN_REVOLVE_SPEED = 60f;
	Pivot currentCrankPivot;

	private CURRENT_INPUT inputState;
	private float deltaX;
	private float deltaY;
    
	void Start ()
    {
        camLook = new MouseLook();
        camLook.Init(gameObject.transform, Camera.main.transform);
        cameraState = CAMERA_STATE.FREE_CAM;
		playerInteract = gameObject.GetComponent<PlayerInteract> ();
		playerMove = gameObject.GetComponent<PlayerMove> ();
		inputState = CURRENT_INPUT.NONE;
		deltaX = 0f;
		deltaY = 0f;

	}

	void Update() {
		if (Input.GetButtonDown ("Interact")) {
			inputState = CURRENT_INPUT.INTERACT_DOWN;
		} else if (Input.GetButton ("Interact")) {
			inputState = CURRENT_INPUT.INTERACT_HELD;
		} else if (Input.GetButtonUp ("Interact")) {
			inputState = CURRENT_INPUT.INTERACT_UP;
		} else if (Input.GetButtonDown ("Cancel Interact")) {
			inputState = CURRENT_INPUT.CANCEL_INTERACT;
		} else {
			inputState = CURRENT_INPUT.NONE;
		}
		deltaX = CrossPlatformInputManager.GetAxis ("Mouse X");
		deltaY = CrossPlatformInputManager.GetAxis ("Mouse Y");
	}

	void FixedUpdate () {
		// We want lerping to occur in fixed intervals
		if (cameraState == CAMERA_STATE.LERPING_TO_LOCK_CAM) {
			moveToCrank ();
			SetCursorLock ();
		} else if (cameraState == CAMERA_STATE.LERPING_TO_FREE_CAM) {
			moveFromCrank ();
			SetCursorLock ();
		} else if (cameraState == CAMERA_STATE.FREE_CAM) {
			// player cam and player interact are very closely coupled
			// TODO: Split them apart or put them together, just end this silly message passing.

			playerInteract.updateInteractLogic (inputState == CURRENT_INPUT.INTERACT_DOWN);
			playerInteract.centerObjectInCamera ();
			camLook.LookRotation (gameObject.transform, Camera.main.transform);

		}  else if (cameraState == CAMERA_STATE.LOCK_CAM){
			crankCrank ();
			SetCursorLock ();
		}
		inputState = CURRENT_INPUT.NONE;
	}

	public void beginLerpToLockPos(CrankTransformManager snappedCrankTransformManager) {
		currentCrankPivot = snappedCrankTransformManager.getControlledPivot ();
		playerMove.lockMovement ();

		gameObject.GetComponent<Rigidbody> ().isKinematic = true;


		Transform newLockPos = snappedCrankTransformManager.getCameraLerpTransform();
		lockPos = newLockPos;

		lerpOrigin = Camera.main.transform.position;
		lerpRotationOrigin = Camera.main.transform.rotation;

		cameraState = CAMERA_STATE.LERPING_TO_LOCK_CAM;
		startLerpTime = Time.time;
		// Calculate how long we want to lerp for
		lerpTimer = (Vector3.Distance (transform.position, newLockPos.position) / LERP_SPEED);
	}

	private void beginLerpToOldPos() {
		cameraState = CAMERA_STATE.LERPING_TO_FREE_CAM;
		startLerpTime = Time.time;

		lerpOrigin = Camera.main.transform.position;
		lerpRotationOrigin = Camera.main.transform.rotation;
		// Calculate how long we want to lerp for
		lerpTimer = (Vector3.Distance (Camera.main.transform.position, transform.position) / LERP_SPEED);
		lockPos = null;
		currentCrankPivot = null;
	}

	private bool lerpCameraToTransform(Vector3 originPosition, Quaternion originQuaternion, Transform destination) {
		// I finally found out how to lerp!!!
		// http://www.blueraja.com/blog/404/how-to-use-unity-3ds-linear-interpolation-vector3-lerp-correctly
		// The power of math!
		float timeSinceStarted = Time.time - startLerpTime;
		float percentageComplete = timeSinceStarted / lerpTimer;

		Camera.main.transform.position = Vector3.Lerp (originPosition, destination.position, percentageComplete);
		Camera.main.transform.rotation = Quaternion.Lerp (originQuaternion, destination.rotation, percentageComplete);
		if (percentageComplete >= 1.0f) {
			Camera.main.transform.position = destination.position;
			Camera.main.transform.rotation = destination.rotation;
			return true;
		}
		return false;
	}

    private void moveToCrank()
    {
		if (lerpCameraToTransform (lerpOrigin, lerpRotationOrigin, lockPos)) {
			cameraState = CAMERA_STATE.LOCK_CAM;
			Debug.Log ("Finished Lerp");
		}
    }

	private void moveFromCrank()
	{
		if (lerpCameraToTransform (lerpOrigin, lerpRotationOrigin, gameObject.transform)) {
			cameraState = CAMERA_STATE.FREE_CAM;
			playerMove.unlockMovement ();
			gameObject.GetComponent<Rigidbody> ().isKinematic = false;
			Debug.Log ("Finished Lerp");
		}
	}

    private void crankCrank() //potentially move this into PlayerInteract once it's working
    {
		if (inputState == CURRENT_INPUT.CANCEL_INTERACT) {
			oldest = new Vector2(float.NaN, float.NaN);
			middle = new Vector2(float.NaN, float.NaN);
			newest = new Vector2(float.NaN, float.NaN);
			consistentTurnCount = 0;
			beginLerpToOldPos ();
		} else if (inputState == CURRENT_INPUT.INTERACT_DOWN) {
			newest = new Vector2 (0f, 0f);
			oldest = new Vector2(float.NaN, float.NaN);
			middle = new Vector2(float.NaN, float.NaN);
			Debug.Log ("Press Interacty");
			consistentTurnCount = 0;
		} else if (inputState == CURRENT_INPUT.INTERACT_UP) {
			oldest = new Vector2(float.NaN, float.NaN);
			middle = new Vector2(float.NaN, float.NaN);
			newest = new Vector2(float.NaN, float.NaN);
			consistentTurnCount = 0;
		} else if (inputState == CURRENT_INPUT.INTERACT_HELD) {
			// Detection of a circle gesture:
			// As seen on https://answers.unity.com/questions/219958/touch-gestures-recognising-a-circle.html
			// Without the power of math this would be very silly to do
			oldest = (!float.IsNaN(middle.x)) ? new Vector2 (middle.x, middle.y) : new Vector2(float.NaN, float.NaN);
			middle = new Vector2(newest.x, newest.y);
			newest = new Vector2 (newest.x + deltaX, newest.y + deltaY);


			// Mouse is not moving or it moves very slowly
			const float epsilon = 0.001f;
			if (Mathf.Abs (deltaX) < epsilon || Mathf.Abs (deltaY) < epsilon) {
				consistentTurnCount += consistentTurnCount < 0 ? 5 : consistentTurnCount > 0 ? -5 : 0;
			}


			if (float.IsNaN(oldest.x)) {
				return;
			}

			Vector2 olderDelta = (oldest - middle).normalized;
			Vector2 newerDelta = (newest - middle).normalized;

			Vector2 newPerpendicular = new Vector2 (newerDelta.y, -(newerDelta.x));
			float dotProduct = Vector2.Dot (olderDelta, newPerpendicular);


			// Now we see if player has been consistently turning clockwise or counterclockwise.

			// The following will allow for rapid switching between clockwise and counterclockwise.
			// Commenting it out will make it so there is more of a transition between clockwise and counterclockwise
			/*
			if (consistentTurnCount > 0) {
				if (dotProduct < 0) {
					consistentTurnCount = 0;
					return;
				}
			} else if (consistentTurnCount < 0) {
				if (dotProduct > 0) {
					consistentTurnCount = 0;
					return;
				}
			}
			*/

			consistentTurnCount += dotProduct > 0 ? 1 : -1;

			if (consistentTurnCount > CIRCLE_TURN_CONSISTENCY_MAX) {
				consistentTurnCount = CIRCLE_TURN_CONSISTENCY_MAX;
			} else if (consistentTurnCount < -CIRCLE_TURN_CONSISTENCY_MAX) {
				consistentTurnCount = -CIRCLE_TURN_CONSISTENCY_MAX;
			}

			if (consistentTurnCount > CIRCLE_TURN_CONSISTENCY_THRESHOLD) {
				if(currentCrankPivot != null) {
					currentCrankPivot.Rotate(Time.fixedDeltaTime * SUN_REVOLVE_SPEED);
				}
			} else if (consistentTurnCount < -CIRCLE_TURN_CONSISTENCY_THRESHOLD) {
				if(currentCrankPivot != null) {
					currentCrankPivot.Rotate(-Time.fixedDeltaTime * SUN_REVOLVE_SPEED);
				}
			}
		}
    }
		

    private void SetCursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

public class MouseLook
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -47F;
    public float MaximumX = 47F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;


    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    public void Init(Transform character, Transform camera)
    {
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;
        smooth = false;
    }


    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}
