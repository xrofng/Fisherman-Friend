using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickRegister : MonoBehaviour
{
    public GameSceneGUI gameSceneGui;

    public int numPlayerInMatch;
    private List<string> registeredJoystick = new List<string>();

    // Use this for initialization
    void Start()
    {
        numPlayerInMatch = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckFirsyJoystickInput();
        if (Input.GetKeyDown(KeyCode.H))
        {
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                Debug.Log(i + Input.GetJoystickNames()[i]);
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            JoystickManager.Instance.PrintAllPlayerButton();
        }

    }

    void CheckFirsyJoystickInput()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            string joyName = Input.GetJoystickNames()[i];

            if (JoystickManager.Instance.GetButtonDown("Jump", i, true))
            {
                if (!registeredJoystick.Contains(joyName))
                {
                    registeredJoystick.Add(joyName);
                    JoystickManager.Instance.AssignPlayerButton(numPlayerInMatch, i);
                    AddInMatchPlayerNum(1);
                    gameSceneGui.OnJoystickRegister(numPlayerInMatch);
                }
                else
                {
                    Debug.Log("contain" + i + joyName);
                }
                //PrintRegisJoySticksName();
            }
        }
    }

    public void PrintRegisJoySticksName()
    {
        for (int i = 0; i < registeredJoystick.Count; i++)
        {
            string joyName = registeredJoystick[i];
            Debug.Log(i + joyName);
        }
    }

    public void AddInMatchPlayerNum(int increment)
    {
        numPlayerInMatch += increment;
        PlayerData.Instance.numPlayer = numPlayerInMatch;
    }
}

