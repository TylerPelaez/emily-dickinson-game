using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_GM : MonoBehaviour {
	public Playback sun;
	public GameObject player;

	bool turning;
	// Use this for initialization
	void Start () {
		turning = false;
	}
	
	// Update is called once per frame
	void Update () {
		check_turning();
	}

	void check_turning(){
		int dir = player.GetComponent<PlayerCam>().turn;
		if(dir == 0){
			turning = true;
		}else{
			turning = false;
		}
	}
}
