using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneGUI : MonoBehaviour
{
    public List<GameObject> ActiveOnSceneActive = new List<GameObject>();
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnGameSceneActive()
    {
        foreach(GameObject ac in ActiveOnSceneActive)
        {
            ac.SetActive(true);
        }
    }

    public virtual void OnJoystickRegister(int currentPlayerNumber)
    {

    }
}
