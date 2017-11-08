using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehavior : MonoBehaviour {

	Vector3 fencePosition;
	private int row;
	private int col;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setFencePosition(Vector3 pos) {
		fencePosition = new Vector3 (pos.x, pos.y, pos.z);
	}

	public void setRowAndColumn(int row_, int column_) {
		row = row_;
		col = column_;
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
}
