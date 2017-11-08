using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForKey : MonoBehaviour {

	public GameObject Wall;
	public GameObject pictureSnap;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "key") {
			Debug.Log ("Hi, Key!");
			Wall.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
			Wall.GetComponent<Rigidbody> ().AddExplosionForce (20f, Wall.transform.position + Wall.transform.forward, 35f);
			gameObject.GetComponent<Collider> ().enabled = false;
			pictureSnap.GetComponent<Collider> ().enabled = true;
		}
	}
}
