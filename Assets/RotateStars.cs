using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStars : MonoBehaviour {
    public Vector3 rotationSpeed;
   public Playback sky;
    Material starMat;
	// Use this for initialization
	void Start () {
        starMat = transform.GetChild(1).GetComponent<Renderer>().material;

    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation *= Quaternion.Euler( rotationSpeed);
        if(sky.progress> .7f||sky.progress<.3)
        {
			starMat.SetColor("_TintColor", Color.Lerp(starMat.GetColor("_TintColor"), new Color(.85f, .85f, .85f, 1), 8 * Time.deltaTime));
        }
        else
        {
			starMat.SetColor("_TintColor", Color.Lerp(starMat.GetColor("_TintColor"), new Color(.85f, .85f, .85f, 0), 8 * Time.deltaTime));
        }
    }
}
