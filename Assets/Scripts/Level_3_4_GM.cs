using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_3_4_GM : MonoBehaviour {
	public GameObject cloud;
	public Playback sun;
	public GameObject key;

	public GameObject photo;
	public GameObject cup;

	bool level3_state;
	bool cloud_state;
	bool sun_state;

	bool level4_state;
	bool photo_state;
	bool cup_state;//the end


	// Use this for initialization
	void Start () {
		level3_state = false;
		cloud_state = false;
		sun_state = false;

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

	void check_sun(){
		if(sun.transform.eulerAngles.z < 180f && sun.transform.eulerAngles.z > 0f){
			sun_state = true;
		}
	}

	void check_cloud(){
		if(cloud.transform.eulerAngles.y < 180f && cloud.transform.eulerAngles.y > 0f){
			cloud_state = true;
		}
	}

	void check_level3(){
		if(!sun_state){
			check_sun();
		}
		if(!cloud_state){
			check_cloud();
		}

		if(sun_state && cloud_state){
			level3_state = true;
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
