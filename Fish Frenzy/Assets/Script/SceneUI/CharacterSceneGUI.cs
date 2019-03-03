using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSceneGUI : GameSceneGUI
{
    public int maxPlayer;
    public CustomizationMenu[] playersCustomizationMenu;
    public GameObject addPlayerLayout;
    public List<int> takenSkinColorId;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (JoystickManager.Instance.GetButtonDown("Pause", i, true) || Input.GetKeyDown(KeyCode.G))
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
}
