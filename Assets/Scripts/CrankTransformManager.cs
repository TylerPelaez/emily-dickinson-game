using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankTransformManager : MonoBehaviour {

	Transform cameraLerpTransform;
	Pivot controlledByCrank;
	public Pivot cloudControl;

	public Playback sun;
	public Playback clouds;
	public Playback effects;
	public GameObject stars;

	// Use this for initialization
	void Start () {
		Transform[] transforms = gameObject.GetComponentsInChildren<Transform> ();
		foreach (var t in transforms) {
			if (t.gameObject.name == "Camera Lock Target") {
				cameraLerpTransform = t;
			}
		}

		if(transform.CompareTag("cup")){
			controlledByCrank = transform.GetChild(0).GetComponent<Pivot>();
		}else{
			controlledByCrank = transform.GetChild(0).GetComponent<Pivot>();
			//cloudControl = transform.GetChild(1).GetComponent<Pivot>();
		}
	}
	
	public Transform getCameraLerpTransform () {
		// Very dirty, anyone can move this transform around !!
		// We will hope everyone is being nice
		// Ensure that if this function is ever used that the value does not change !!
		return cameraLerpTransform;
	}

	public Playback getSun() {
		return sun;
	}

	public Pivot getControlledPivot() {
		return controlledByCrank;
	}

	public Pivot getCloudPivot(){
		return cloudControl;
	}
		
	public Playback getClouds() {
		return clouds;
	}

	public Playback getEffects() {
		return effects;
	}

	public GameObject getStars() {
		return stars;
	}


}
