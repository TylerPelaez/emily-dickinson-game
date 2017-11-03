﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankTransformManager : MonoBehaviour {

	Transform cameraLerpTransform;

	// Use this for initialization
	void Start () {
		Transform[] transforms = gameObject.GetComponentsInChildren<Transform> ();
		foreach (var t in transforms) {
			if (t.gameObject.name == "Camera Lock Target") {
				cameraLerpTransform = t;
			}
		}

	}
	
	public Transform getCameraLerpTransform () {
		// Very dirty, anyone can move this transform around !!
		// We will hope everyone is being nice
		// Ensure that if this function is ever used that the value does not change !!
		return cameraLerpTransform;
	}
}
