using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehavior : MonoBehaviour {

	Vector3 fencePosition;
	private int row;
	private int col;
	bool stay;

	AudioSource chirp;
	public AudioClip[] peeps;

	// Use this for initialization
	void Start () {
		stay = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setFencePosition(Vector3 pos) {
		fencePosition = new Vector3 (pos.x, pos.y, pos.z);
	}

	public void setRowAndColumn(int row_, int column_) {
		chirp = gameObject.GetComponent<AudioSource>();
		row = row_;
		col = column_;
		setClip(row);
	}

	public void setClip(int i){
		chirp.clip = peeps[row];
	}

	public int getRow() {
		return row;
	}

	public int getCol() {
		return col;
	}

	public void setToFencePosition() {
		transform.position = new Vector3 (fencePosition.x, fencePosition.y, fencePosition.z);
	}

	public void toggleStay() {
		stay = !stay;
	}

	public bool getStay() {
		return stay;
	}

	public void Deactivate() {		
		gameObject.SetActive (false);
	}
}
