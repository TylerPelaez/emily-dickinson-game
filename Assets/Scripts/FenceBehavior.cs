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

	public Playback sun;
	public GameObject birdPrefab;

	private RotationRangeObject[] ranges;
	private GameObject[] birds;

	private int[,] indices = new int[3,5] { { 0, 1, 2, 3, 4 }, { 5, 6, 7, 8, 9 }, { 10, 11, 12, 13, 14 } };
	private GameObject[] current_birds;

	private int current_range;

	private int[] win_rows = new int[] { 1, 1, 1, 1, 1};


	// Use this for initialization
	void Start () {
		current_range = -1;
		ranges = new RotationRangeObject[3];
		ranges[0] = new RotationRangeObject (.2f, .3f);
		ranges[1] = new RotationRangeObject (.4f, .6f);
		ranges[2] = new RotationRangeObject (.8f, .95f);

		birds = new GameObject[15];
		current_birds = new GameObject[5];
		for (int i = 0; i < 5; i++) {
			current_birds [i] = null;
		}

		for (int i = 0; i < 15; i++) {
			birds [i] = Instantiate (birdPrefab, transform.position , transform.rotation);

			birds [i].GetComponent<BirdBehavior>().setFencePosition(new Vector3(transform.position.x - 1.5f + ((i % 5) * 0.5f) , transform.position.y - 0.3f + ((i % 3) * 0.5f), transform.position.z));
			birds [i].GetComponent<BirdBehavior> ().setRowAndColumn (i % 3, i % 5);
			birds [i].SetActive (false);

		}
	}
	
	// Update is called once per physics update
	void FixedUpdate () {

		float sunProgress = sun.progress;
		if (current_range == -1) {
			for (int i = 0; i < ranges.Length; i++) {
				if (ranges [i].inRange (sunProgress)) {
					current_range = i;
					Debug.Log (i);

					bool win = true;
					for (int j = 0; j < 5; j++) {
						if (current_birds [j] == null) {
							birds [indices [current_range, j]].SetActive (true);
							birds [indices [current_range, j]].GetComponent<BirdBehavior> ().setToFencePosition ();
							current_birds [j] = birds [indices [current_range, j]];

						}

						if (current_birds [j].GetComponent<BirdBehavior> ().getRow () != win_rows [j]) {
							win = false;
						}
					}

					if (win) {
						Debug.Log ("A Winer is You!");
					}

					break;
				}
			}
		} else {
			if (!ranges [current_range].inRange (sunProgress)) {
				for (int i = 0; i < current_birds.Length; i++) {
					if (current_birds[i] != null && !current_birds [i].GetComponent<BirdBehavior> ().getStay ()) {
						current_birds [i].SetActive (false);
						current_birds [i] = null;

					}
				}
				current_range = -1;
			}

		}
	}
}
