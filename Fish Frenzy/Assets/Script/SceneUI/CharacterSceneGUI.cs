﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSceneGUI : GameSceneGUI
{
    public int maxPlayer;
    public CustomizationMenu[] playersCustomizationMenu;
    public GameObject addPlayerLayout;
    public List<int> takenSkinColorId;
    public GameObject allReadyBanner;
    private bool allPlayerReady;

    // Use this for initialization
    void Start () {
		
	}
	
	void Update ()
    {
        if (!allPlayerReady)
        {
            return;
        }
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (JoystickManager.Instance.GetButtonDown("Pause", i, true) || Input.GetKeyDown(KeyCode.G))
            {
                Initiate.FadeToLoading("Gameplay", Color.white, 2.0f);
            }
        }

        
    }

    public override void OnJoystickRegister(int currentPlayerNumber)
    {
        base.OnJoystickRegister(currentPlayerNumber);
        ShowPlayerLayout(currentPlayerNumber);
        CheckAllPlayerReady();
    }

    void ShowPlayerLayout(int currentPlayerNumber)
    {
        playersCustomizationMenu[currentPlayerNumber - 1].gameObject.SetActive(true);
        playersCustomizationMenu[currentPlayerNumber - 1].SetSkinColorIndex(currentPlayerNumber - 1,1);
        if (currentPlayerNumber == maxPlayer)
        {
            addPlayerLayout.SetActive(false);
        }
    }

    public void RemoveTakenId(int remove)
    {
        if (takenSkinColorId.Contains(remove))
        {
            takenSkinColorId.Remove(remove);
        }
    }

    public bool CheckAllPlayerReady()
    {
        int readyCount = 0;
        foreach(CustomizationMenu cus in playersCustomizationMenu)
        {
            if (cus.playerReady)
            {
                readyCount += 1;
            }
        }
        allPlayerReady = readyCount >= PlayerData.Instance.numPlayer;
        allReadyBanner.SetActive(allPlayerReady);
        return allPlayerReady;
    }
  
}
