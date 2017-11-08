using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlaySkyVideo : MonoBehaviour {
    public VideoPlayer v;
	// Use this for initialization
	void Start () {
        v.Prepare();
        v.prepareCompleted += MoveSky;
        v.errorReceived += ErrorReceived;

    }
    void ErrorReceived(UnityEngine.Video.VideoPlayer e, string s)
    {
        Debug.Log("error completed " + s);

    }
    // Update is called once per frame
    void Update () {
        Debug.Log("is playing "+v.isPlaying+" is prepared "+v.time);

    }
    void MoveSky(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("prepare completed");
        v.Play();
    }

}
