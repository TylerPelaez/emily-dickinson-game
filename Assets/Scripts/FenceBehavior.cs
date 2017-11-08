using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class RotationRangeObject {
	private float beginRotation;
	private float endRotation;

	public RotationRangeObject(float begin, float end) {
		beginRotation = begin;
		endRotation = end;
	}

	public float getBeginRotation() {
		return beginRotation;
	}

	public float getEndRotation() {
		return endRotation;
	}

	public bool inRange(float rot) {
		if (beginRotation > endRotation) {
			return rot >= beginRotation || rot <= endRotation;
		} else {
			return rot >= beginRotation && rot <= endRotation;
		}
	}

}

public class FenceBehavior : MonoBehaviour {

	public GameObject sunPivotObject;
	public GameObject birdPrefab;

	private RotationRangeObject[] ranges;
	private GameObject[] birds;

	private int[,] indices = new int[3,5] { { 0, 1, 2, 3, 4 }, { 5, 6, 7, 8, 9 }, { 10, 11, 12, 13, 14 } };

	private int current_range;


	// Use this for initialization
	void Start () {
		current_range = -1;
		ranges = new RotationRangeObject[3];
		ranges[0] = new RotationRangeObject (320, 340);
		ranges[1] = new RotationRangeObject (290, 310);
		ranges[2] = new RotationRangeObject (350, 10);

		birds = new GameObject[15];

		for (int i = 0; i < 15; i++) {
			birds [i] = Instantiate (birdPrefab, transform.position , transform.rotation);

			birds [i].GetComponent<BirdBehavior>().setFencePosition(new Vector3(transform.position.x - 1.5f + ((i % 5) * 0.5f) , transform.position.y - 0.3f + ((i % 3) * 0.5f), transform.position.z));
			birds [i].GetComponent<BirdBehavior> ().setRowAndColumn (i % 3, i % 5);
			birds [i].SetActive (false);

		}
	}
	
	// Update is called once per physics update
	void FixedUpdate () {

		float pivotRot = sunPivotObject.transform.rotation.eulerAngles.z;
		if (current_range == -1) {
			
			for (int i = 0; i < ranges.Length; i++) {
				if (ranges [i].inRange (pivotRot)) {
					current_range = i;
					for (int j = 0; j < 5; j++) {
						birds [indices [current_range, j]].SetActive (true);
						birds [indices [current_range, j]].GetComponent<BirdBehavior>().setToFencePosition ();
					}
					break;
				}
			}
		} else {
			if (!ranges [current_range].inRange (pivotRot)) {
				for (int i = 0; i < 5; i++) {
					birds [indices [current_range, i]].SetActive (false);
				}
				current_range = -1;

			}

		}
	}
}
