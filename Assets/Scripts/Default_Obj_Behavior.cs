using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_Obj_Behavior : MonoBehaviour {
	private GameObject snap_obj;
	public bool held {get; set;}

	// Use this for initialization
	void Start () {
		snap_obj = null;
		held = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public CrankTransformManager SnapTo(){
		// Returns the CrankTransformManager of the object this object snapped to
		if(snap_obj != null){
			transform.position = snap_obj.transform.position;
			return snap_obj.GetComponent<CrankTransformManager> ();
		}else{
			transform.position = transform.position;
			return null;
		}
	}
	
	void OnTriggerEnter(Collider col){
		Debug.Log("entered");
		if(col.gameObject.tag == gameObject.tag){
			snap_obj = col.transform.gameObject;

		}else{
			snap_obj = null;
			Debug.Log("need correct obj");
		}
	}

	void OnTriggerExit(Collider col){
		snap_obj = null;
	}
	void OnCollisionEnter(Collision col) {
		Debug.Log (col.gameObject.name);
	}
}
