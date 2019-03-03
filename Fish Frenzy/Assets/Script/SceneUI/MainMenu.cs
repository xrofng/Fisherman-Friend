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
        for(int i= 0 ; i < PlayerData.Instance.maxNumPlayer; i++)
        {
            if (JoystickManager.Instance.GetButtonDown("Pause",i))
            {
                GetComponent<AudioSource>().Play();
                Initiate.Fade("CharacterSelect", Color.white, 2.0f);
            }
        }
        
    }
    
}
