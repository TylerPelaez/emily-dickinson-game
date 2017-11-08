using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	bool lerping;

	public GameObject dest;
	Vector3 origin;

	float startTime;
	float totalTime = 5f;

	public void beginLerp() {
		origin = gameObject.transform.position;
		startTime = Time.time;
		lerping = true;
		gameObject.GetComponent<Collider> ().enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (lerping) {
			float percent = (Time.time - startTime) / totalTime;
			gameObject.transform.position = Vector3.Lerp (origin, dest.transform.position, percent);
			if (percent >= 1.0f) {
				lerping = false;
			}
		}
	}
}
