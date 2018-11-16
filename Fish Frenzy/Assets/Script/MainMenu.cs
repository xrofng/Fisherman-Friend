using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
   
    // Use this for initialization
    void Start () {
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            GetComponent<AudioSource>().Play();
            Initiate.Fade("Gameplay", Color.white, 2.0f);
            //GameLoop.Instance.Reset();

        }
    }
    
}
