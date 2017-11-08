using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {
	public Transform sel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		sel.position = Vector3.Lerp (sel.position, transform.position, 14* Time.deltaTime);
		sel.rotation = Quaternion.Lerp (sel.rotation, transform.rotation, 14 * Time.deltaTime);

	}
}
