using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_Obj_Behavior : MonoBehaviour {
	GameObject snap_obj;
	public bool held {get; set;}

	// Use this for initialization
	void Start () {
		snap_obj = null;
		held = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SnapTo(){
		if(snap_obj != null){
			transform.position = snap_obj.transform.position;
		}else{
			transform.position = transform.position;
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
}
