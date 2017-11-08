using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControlForDayNight : MonoBehaviour {
    public Playback Sun;
    public Playback clouds;
    public GameObject stars;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
           // Sun.Pause(true);
          //  clouds.Pause(true);

        }
        else
        {
          //  Sun.Play();
          //  clouds.Play();

        }
        DynamicGI.UpdateEnvironment();
        transform.rotation = Quaternion.Euler(630 + ((Sun.progress / 1f) * 360), 0, 0);
    }
}
