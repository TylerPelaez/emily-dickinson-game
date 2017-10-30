using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSun : MonoBehaviour {

	Pivot sunObjectPivot;
	const float SUN_ROTATE_SPEED = 30f;

	// Use this for initialization
	void Start () {
		sunObjectPivot = GameObject.Find ("Sun Pivot").GetComponent<Pivot> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1")) {
			if(sunObjectPivot != null) {
				sunObjectPivot.Rotate(Time.deltaTime * SUN_ROTATE_SPEED);
			}
		} else if (Input.GetButton("Fire2")) {
			if(sunObjectPivot != null) {
				sunObjectPivot.Rotate(- Time.deltaTime * SUN_ROTATE_SPEED);
			}
		}
	}
}
