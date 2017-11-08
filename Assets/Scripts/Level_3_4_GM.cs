using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_3_4_GM : MonoBehaviour {
	public GameObject cloudPivot;
	public Playback sun;
	public GameObject key;

	public GameObject photo;
	public GameObject cup;

	public GameObject water;

	bool level3_state;
	bool level3_stage1;

	bool level4_state;
	bool photo_state;
	bool cup_state;//the end


	// Use this for initialization
	void Start () {
		level3_state = false;
		level3_stage1 = false;

		level4_state = false;
		photo_state = false;
		cup_state = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!level3_state){
			check_level3();
		}else{
			if(!level4_state){
				check_level4();
			}else{
				Debug.Log("done");
			}
		}
	}

	void check_level3(){
		if (!level3_stage1) {
			if ((
				(sun.progress >= 0.58f && sun.progress <= 0.64f) && (cloudPivot.transform.rotation.eulerAngles.y >= 280 && cloudPivot.transform.rotation.eulerAngles.y <= 300)
			        ) || 
				((sun.progress >= 0.49f && sun.progress <= 0.52f) && (cloudPivot.transform.rotation.eulerAngles.y >= 115 && cloudPivot.transform.rotation.eulerAngles.y <= 140))) {
				water.GetComponent<Water> ().beginLerp ();
				level3_stage1 = true;
			}
		}
	}

	void check_photo(){
		
	}

	void check_cup(){
		
	}

	void check_level4(){
		if(!photo_state){
			check_photo();
		}
		if(!cup_state){
			check_cup();
		}
	}
}
