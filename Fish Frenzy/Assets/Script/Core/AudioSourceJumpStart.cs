using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceJumpStart : MonoBehaviour {
    public float skipTime = 0.0f;
    // Use this for initialization
    void Start () {
        GetComponent<AudioSource>().time = skipTime;
	}
	
	// Update is called once per frame
	void Update () {

    }
}
