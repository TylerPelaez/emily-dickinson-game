using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_3_4_GM : MonoBehaviour {
	public GameObject[] clouds;
	public GameObject sun;
	public GameObject key;


	bool level3_state;
	bool cloud_state;
	bool sun_state;
	bool rain_state;
	bool key_state; //transitions to level 4

	bool level4_state;
	bool photo_state;
	bool cup_state;//the end


	// Use this for initialization
	void Start () {
		level3_state = false;
		cloud_state = false;
		sun_state = false;
		rain_state = false;
		key_state = false;

		level4_state = false;
		photo_state = false;
		cup_state = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
