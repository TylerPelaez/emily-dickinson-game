using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_GM : MonoBehaviour {
	public Playback sun;
	public GameObject player;
	public GameObject trees;
	public GameObject treecollider;

	bool turning;
	// Use this for initialization
	void Start () {
		turning = false;
	}
	
	// Update is called once per frame
	void Update () {
		check_turning();
		check_state();
	}

	void check_turning(){
		int dir = player.GetComponent<PlayerCam>().turn;
		if(dir == 0){
			turning = true;
		}else{
			turning = false;
		}
	}

	void check_state(){
		if(turning && sun.progress > 0.20f && sun.progress < 0.30f){
			Debug.Log ("deadtree");
			trees.GetComponent<Animator> ().SetBool ("finished", true);
			treecollider.GetComponent<Collider> ().enabled = false;
		}
	}
}
