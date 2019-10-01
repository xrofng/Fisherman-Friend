using System.Collections;
using System.Collections.Generic;
using System; //This allows the IComparable Interface
using UnityEngine;


public class JoystickManager : PersistentSingleton<JoystickManager>
{
    public Dictionary<string, KeyCode> playerButton = new Dictionary<string, KeyCode>();

    public List<Dictionary<string, KeyCode>> ButtonList = new List<Dictionary<string, KeyCode>>();

    public List<Dictionary<string, KeyCode>> UnregisterButtonList = new List<Dictionary<string, KeyCode>>();

    [Header("Button Key")]
    public string Fishing = "Fishing";
    public string Jump = "Jump";
    public string Special = "Special";
    public string Throw = "Throw";
    public string Switch = "Switch";
    public string Block = "Block";
    public string Hori = "Hori";
    public string Verti = "Verti";
    public string Pause = "Pause";
    public string R2 = "R2";
    public string Dhori = "Dhori";
    public string Dverti = "Dverti";
    public string Dfail = "Dfail";

    /* /// <summary>
            X = 1
            O = 2
            Sq= 0
            Tri=3

            R1= 5
            R2= 7
            R3= 11
            L1= 4
            L2=6
            L3=10

            PSButton = 12
            TouchPadClick=13
            Options=9
            Share= 8

            LeftStickX = 1st XAxis = 18
            LeftStickY = 2nd Yaxis(must invert) =19

            L3=4th Axis(-1,1 not 0,1)
            R3=5th Axis(-1,1 not 0,1)

            DPadX = 6th Axis =16
            DPadY = 7th Axis =17

            RightStickX=8th Axis
            RightStickY=9th Axis(must invert)

            
        /// </summary> */

    /// <summary>
    /// On Start initialize our buttons
    /// </summary>
    protected virtual void Start()
    {
         InitializeInput();
         
         RemapButton();
    }

    protected virtual void InitializeInput()
    {
        // specify button of 1st joystick
        playerButton.Add(Fishing, KeyCode.Joystick1Button2);
        playerButton.Add(Jump, KeyCode.Joystick1Button1);
        playerButton.Add(Special, KeyCode.Joystick1Button0);
        playerButton.Add(Throw, KeyCode.Joystick1Button3);
        playerButton.Add(Switch, KeyCode.Joystick1Button4);
        playerButton.Add(Block, KeyCode.Joystick1Button5);
        playerButton.Add(Pause, KeyCode.Joystick1Button9);
        playerButton.Add(R2, KeyCode.Joystick1Button7);

        // substitute button to be axis
        playerButton.Add(Dhori, KeyCode.Joystick1Button18);
        playerButton.Add(Dverti, KeyCode.Joystick1Button19);
        playerButton.Add(Hori, KeyCode.Joystick1Button18);
        playerButton.Add(Verti, KeyCode.Joystick1Button19);
        playerButton.Add(Dfail, KeyCode.Joystick1Button17);

        ButtonList.Add(playerButton);
        UnregisterButtonList.Add(playerButton);

        // duplicate input from 1st to other but increase key code by offsetToNextPlayerKeycode
        
        int offsetToNextPlayerKeycode = KeyCode.Joystick2Button0 - KeyCode.Joystick1Button0;
        int maxJoystickDetected = 8;
        for (int i = 1; i <= maxJoystickDetected; i++)
        {
            Dictionary<string, KeyCode> newKeyDict = new Dictionary<string, KeyCode>();
            foreach (string key in playerButton.Keys)
            {
                newKeyDict.Add(key, playerButton[key] + offsetToNextPlayerKeycode * i);
            }
            ButtonList.Add(newKeyDict);
            UnregisterButtonList.Add(newKeyDict);
        }

        
    }

    /// <summary>
    /// swap some input according to Joystick brand(name)
    /// </summary>
    public void RemapButton()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            string joyName = Input.GetJoystickNames()[i];
            Debug.Log(i + joyName);
            if (joyName == "Wireless Controller")
            {
            }
            else 
            if(joyName == "Wireless Gamepad")
            {
                SwapButton(Jump, Fishing, i);
                SwapButton(Special, Jump, i);
                //SwapButton(Hori, Verti, i);
            }
            else
            if (joyName == ("Generic   USB  Joystick  "))
            {
                //SwapButton(Special, Throw, i);
                // SwapButton(Fishing, Jump, i);
            }
            else
            if (joyName.Contains("Controller"))
            {
                SwapButton(Special, Fishing, i);
                SwapButton(Jump, Fishing, i);
                SwapButton(R2, Pause, i);
            }
        }
    }
    void SwapButton(string buttonName1, string buttonName2, int playerID)
    {
        KeyCode temp = ButtonList[playerID][buttonName1];
        ButtonList[playerID][buttonName1] = ButtonList[playerID][buttonName2];
        ButtonList[playerID][buttonName2] = temp;
    }

    /// <summary>
    /// swap player button for first register joystick
    /// </summary>
    /// <param name="playerID_1"></param>
    /// <param name="playerID_2"></param>
    public void AssignPlayerButton(int playerID, int unregisJoyID)
    {
        ButtonList[playerID] = UnregisterButtonList[unregisJoyID];
    }

    /// <summary>
    /// Print methods for debuging
    /// </summary>
    public void PrintAllPlayerButton()
    {
        for (int i = 0; i < PlayerData.Instance.numPlayer; i++)
        {
            PrintPlayerButton(i);
        }
    }
    void PrintPlayerButton(int playerID)
    {
        foreach (string key in playerButton.Keys)
        {
            print(playerID + " " + key + "   " + ButtonList[playerID][key]);
        }
    }

    /// <summary>
    /// Get button/axis methods to return activity of button/axis
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public bool GetAnyButtonDown(int playerID , bool isSwapped = false)
    {
        List<Dictionary<string, KeyCode>> _buttonList = GetButtonList(isSwapped);
        foreach (string key in playerButton.Keys)
        {
            if (Input.GetKeyDown(_buttonList[playerID][key]))
            {
                return true;
            }
        }
        return false;
    }

    public bool GetButtonDown(string buttonName, int playerID, bool isUnregistered = false)
    {
        List<Dictionary<string, KeyCode>> _buttonList = GetButtonList(isUnregistered);
        return Input.GetKeyDown(_buttonList[playerID][buttonName]);
    }

    public bool GetAnyPlayerButtonDown(string buttonName, bool isUnregistered = false)
    {
        List<Dictionary<string, KeyCode>> _buttonList = GetButtonList(isUnregistered);
        for(int i = 0; i < _buttonList.Count; i++)
        {
            if (Input.GetKeyDown(_buttonList[i][buttonName]))
            {
                return true;
            }
        }
        return false;
    }

    public bool GetButton(string buttonName, int playerID, bool isUnregistered = false)
    {
        List<Dictionary<string, KeyCode>> _buttonList = GetButtonList(isUnregistered);
        return Input.GetKey(_buttonList[playerID][buttonName]);
    }

    public bool GetButtonUp(string buttonName, int playerID, bool isUnregistered = false)
    {
        List<Dictionary<string, KeyCode>> _buttonList = GetButtonList(isUnregistered);
        return Input.GetKeyUp(_buttonList[playerID][buttonName]);
    }

    public bool GetOneButtonsDown(string[] buttonName, int playerID, bool isUnregistered = false)
    {
        List<Dictionary<string, KeyCode>> _buttonList = GetButtonList(isUnregistered);
        for (int i = 0; i < buttonName.Length; i++)
        {
            if (Input.GetKeyDown(_buttonList[playerID][buttonName[i]]))
            {
                return true;
            }
        }
        return false;
    }

    public float GetAxisRaw(string buttonName, int playerID)
    {
        int playerIDfromButton = ((int)ButtonList[playerID][buttonName] - (int)KeyCode.Joystick1Button0) / 20;
        playerIDfromButton += 1;
        string axisName = null;
        if((int)ButtonList[playerID][buttonName] == (int)KeyCode.Joystick1Button18 + 20 * playerID)
        {
        }
        if ((int)ButtonList[playerID][buttonName] == (int)KeyCode.Joystick1Button17 + 20 * playerID)
        {
        }
        if (buttonName == Hori)
        {
            axisName = "Hori" + playerIDfromButton;
        }
        else if (buttonName == Verti)
        {
            axisName = "Verti" + playerIDfromButton;
        }
        else if (buttonName == Dhori)
        {
            axisName = "Dhori" + playerIDfromButton;
        }
        else if (buttonName == Dverti)
        {
            axisName = "Dverti" + playerIDfromButton;
        }
        return Input.GetAxisRaw(axisName);
    }

    public float GetAxis(string buttonName, int playerID)
    {
        int playerIDfromButton = ((int)ButtonList[playerID][buttonName] - (int)KeyCode.Joystick1Button0) / 20;
        playerIDfromButton += 1;
        string axisName = null;

        if (buttonName == Hori)
        {
            axisName = "Hori" + playerIDfromButton;
        }
        else if (buttonName == Verti)
        {
            axisName = "Verti" + playerIDfromButton;
        }
        return Input.GetAxis(axisName);
    }

    List<Dictionary<string, KeyCode>> GetButtonList(bool isSwapped)
    {
        if (isSwapped)
        {
             return UnregisterButtonList;
        }
        else
        {
            return ButtonList;
        }
    }
}

