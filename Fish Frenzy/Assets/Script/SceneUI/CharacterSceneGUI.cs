using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSceneGUI : GameSceneGUI
{
    public int maxPlayer;
    public GameObject[] playersLayout;
    public GameObject addPlayerLayout;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (JoystickManager.Instance.GetButtonDown("Pause", i) || Input.GetKeyDown(KeyCode.G))
            {
                Initiate.Fade("Gameplay", Color.white, 2.0f);
            }
        }
    }

    public override void OnJoystickRegister(int currentPlayerNumber)
    {
        base.OnJoystickRegister(currentPlayerNumber);
        ShowPlayerLayout(currentPlayerNumber);
    }

    void ShowPlayerLayout(int currentPlayerNumber)
    {
        playersLayout[currentPlayerNumber - 1].SetActive(true);
        if(currentPlayerNumber == maxPlayer)
        {
            addPlayerLayout.SetActive(false);
        }
    }
}
