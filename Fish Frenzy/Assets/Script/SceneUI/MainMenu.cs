using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public float ignoreInputDuration = 1.5f;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {

        ignoreInputDuration -= Time.deltaTime;
        if (ignoreInputDuration > 0)
        {
            return;
        }

        for(int i= 0 ; i < PlayerData.Instance.maxNumPlayer; i++)
        {
            if (JoystickManager.Instance.GetAnyButtonDown(i))
            {
                GetComponent<AudioSource>().Play();
                Debug.Log("s");
                Initiate.FadeToLoading("CharacterSelect", Color.white, 2.0f);
            }
        }
        
    }
    
}
