using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControlForDayNight : MonoBehaviour {
    public Playback Sun;
    public Playback clouds;
    public GameObject stars;

	bool toggle;

    // Use this for initialization
    void Start () {
		toggle = true;
	}
	
	// Update is called once per frame
	void Update () {
        DynamicGI.UpdateEnvironment();
        transform.rotation = Quaternion.Euler(630 + ((Sun.progress / 1f) * 360), 0, 0);
    }
}
