using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_GM : MonoBehaviour {
	public Playback sun;
	public GameObject player;
	public GameObject trees;
	public GameObject treecollider;
	public AudioSource end;
	public GameObject mug;
	public GameObject glowMug;

	bool turning;
	// Use this for initialization
	void Start () {
		turning = false;
		end = end.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
			if(!trees.GetComponent<Animator> ().GetBool ("finished")) {
				mug.SetActive (false);
				GameObject g = (GameObject)(Instantiate(glowMug));
				g.transform.position=mug.transform.position;
				g.transform.rotation = mug.transform.rotation;
			}
			trees.GetComponent<Animator> ().SetBool ("finished", true);
			treecollider.GetComponent<Collider> ().enabled = false;
			if(!end.isPlaying){
				end.Play();
			}
		}
	}
}
