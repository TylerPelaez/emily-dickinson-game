﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove : MonoBehaviour
{
    private float xMove;
    private float zMove;
    public float moveSpeed = .5f;
    public float maxSpeed = .5f;
	private bool canMove;

    void Start()
    {
        xMove = 0f;
        zMove = 0f;
		canMove = true;
    }

    private void FixedUpdate()
    {
		if (!canMove) {
			return;
		}
        if (CrossPlatformInputManager.GetAxis("Vertical") == 1f)
        {
            zMove += moveSpeed * Time.deltaTime;
            if (zMove > maxSpeed)
                zMove = maxSpeed;
        }
        else if (zMove > 0 && Mathf.Abs(zMove) >= .005f)
        {
            zMove -= moveSpeed * Time.deltaTime;
        }
        if (CrossPlatformInputManager.GetAxis("Horizontal") == 1f)
        {
            xMove += moveSpeed * Time.deltaTime;
            if (xMove > maxSpeed)
                xMove = maxSpeed;
        }
        else if (xMove > 0 && Mathf.Abs(xMove) >= .005f)
        {
            xMove -= moveSpeed * Time.deltaTime;
        }
        if (CrossPlatformInputManager.GetAxis("Vertical") == -1f)
        {
            zMove -= moveSpeed * Time.deltaTime;
            if (zMove < -maxSpeed)
                zMove = -maxSpeed;
        }
        else if (zMove < 0 && Mathf.Abs(zMove) >= .005f)
        {
            zMove += moveSpeed * Time.deltaTime;
        }
        if (CrossPlatformInputManager.GetAxis("Horizontal") == -1f)
        {
            xMove -= moveSpeed * Time.deltaTime;
            if (xMove < -maxSpeed)
                xMove = -maxSpeed;
        }
        else if (xMove < 0 && Mathf.Abs(xMove) >= .005f)
        {
            xMove += moveSpeed * Time.deltaTime;
        }
        if (Mathf.Abs(xMove) < .005f)
        {
            xMove = Mathf.Lerp(xMove, 0f, .5f);
        }
        if (Mathf.Abs(zMove) < .005f)
        {
            zMove = Mathf.Lerp(zMove, 0f, .5f);
        }
        if (Mathf.Abs(CrossPlatformInputManager.GetAxis("Horizontal")) == 1 && Mathf.Abs(CrossPlatformInputManager.GetAxis("Vertical")) == 1)
            gameObject.transform.Translate(xMove / Mathf.Sqrt(Mathf.Sqrt(2)), 0, zMove / Mathf.Sqrt(Mathf.Sqrt(2)));
        else
            gameObject.transform.Translate(xMove, 0, zMove);
    }

	public void lockMovement() {
		canMove = false;
	}

	public void unlockMovement() {
		canMove = true;
	}
}