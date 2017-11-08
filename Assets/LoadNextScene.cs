using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour {

	public GameObject blackFade;
	Material mat;
	// Use this for initialization
	void Start () {
		mat = blackFade.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.other.gameObject.name == "Player") {
			StartCoroutine("newScene");
			Debug.Log("loading scene");
		}
	}

	IEnumerator newScene() {
		float startTime = Time.time;
		float duration = .5f;

		while(Time.time-startTime<duration)
		{
			mat.color  = Color.Lerp(new Color(0,0,0,0), new Color (0,0,0,1),(Time.time-startTime)/duration );
			yield return new WaitForEndOfFrame();

		}
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);

		yield return new WaitForEndOfFrame();
	}
}
