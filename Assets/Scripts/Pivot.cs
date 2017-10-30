using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour {

	Transform playerTransform;

	// Use this for initialization
	void Start () {
		playerTransform = GameObject.Find ("FPSController").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
	}

	public void Rotate(float amt) {
		transform.RotateAround(transform.position, transform.forward, amt);
	}
}
