using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Rotate(float amt) {
		transform.RotateAround(transform.position, transform.up, amt);
		

	}
}
