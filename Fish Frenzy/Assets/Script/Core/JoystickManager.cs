using System.Collections;
using System.Collections.Generic;
using System; //This allows the IComparable Interface
using UnityEngine;

public class JoystickManager : Singleton<JoystickManager>
{
    int numPlayer = 4;

    public Dictionary<string, KeyCode> Player1Button = new Dictionary<string, KeyCode>();
    public Dictionary<string, KeyCode> Player2Button = new Dictionary<string, KeyCode>();
    public Dictionary<string, KeyCode> Player3Button = new Dictionary<string, KeyCode>();
    public Dictionary<string, KeyCode> Player4Button = new Dictionary<string, KeyCode>();

    public List<Dictionary<string, KeyCode>> ButtonList = new List<Dictionary<string, KeyCode>>();

    [Header("Button Key")]
    public string Fishing  = "Fishing";
    public string Jump     = "Jump"   ;
    public string Special     = "Special"   ;
    public string Throw    = "Throw"  ;
    public string Switch   = "Switch" ;
    public string Hori     = "Hori"   ;
    public string Verti    = "Verti"  ;

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

         DPadX = 7th Axis
         DPadY = 8th Axis

         LeftStickX=XAxis - 18
         LeftStickY = Yaxis(must invert) -19

         RightStickX=3rd Axis
         RightStickY=6th Axis(must invert)

         L3=4th Axis(-1,1 not 0,1)
         R3=5th Axis(-1,1 not 0,1)
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
        ButtonList.Add(Player1Button);
        ButtonList.Add(Player2Button);
        ButtonList.Add(Player3Button);
        ButtonList.Add(Player4Button);

        // specify button of 1st joystick
        Player1Button.Add(Fishing, KeyCode.Joystick1Button2);
        Player1Button.Add(Jump, KeyCode.Joystick1Button1);
        Player1Button.Add(Special, KeyCode.Joystick1Button3);
        Player1Button.Add(Throw, KeyCode.Joystick1Button0);
        Player1Button.Add(Switch, KeyCode.Joystick1Button5);
        Player1Button.Add(Hori, KeyCode.Joystick1Button18);
        Player1Button.Add(Verti, KeyCode.Joystick1Button19);
        // duplicate input from 1st to other but increase key code by offsetToNextPlayerKeycode

        int offsetToNextPlayerKeycode = KeyCode.Joystick2Button0 - KeyCode.Joystick1Button0;
        for (int i = 1; i < numPlayer; i++)
        {
            Dictionary<string, KeyCode> newKeyDict = new Dictionary<string, KeyCode>();
            foreach (string key in Player1Button.Keys)
            {
                newKeyDict.Add(key, Player1Button[key] + offsetToNextPlayerKeycode * i);
            }
            ButtonList[i] = newKeyDict;
        }
    }

    /// <summary>
    /// swap some input according to Joystick brand(name)
    /// </summary>
    public void RemapButton()
    {
        for (int i = 0; i < Input.GetJoystickNames().Length ; i++)
        {
            string joyName = Input.GetJoystickNames()[i];
            Debug.Log(i+ joyName);
            if(joyName == "Wireless Controller")
            {

            } else 
            if (joyName == ("Generic   USB  Joystick  "))
            {
                //SwapButton(Special, Throw, i);
               // SwapButton(Fishing, Jump, i);
            }else
            if (joyName.Contains("Controller"))
            {
                SwapButton(Jump, Throw, i);
                SwapButton(Throw, Fishing, i);
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
    /// Print methods for debuging
    /// </summary>
    void PrintAllPlayerButton()
    {
        for (int i = 0; i < numPlayer; i++)
        {
            PrintPlayerButton(i);
        }
    }
    void PrintPlayerButton(int playerID)
    {
        foreach (string key in Player1Button.Keys)
        {
            print(playerID+" "+key+"   "+ButtonList[playerID][key]);
        }
    }

    /// <summary>
    /// Get button/axis methods to return activity of button/axis
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public bool GetButtonDown(string buttonName , int playerID)
    {
        return Input.GetKeyDown(ButtonList[playerID][buttonName]);
    }

    public bool GetButton(string buttonName, int playerID)
    {
        return Input.GetKey(ButtonList[playerID][buttonName]);
    }

    public bool GetButtonUp(string buttonName, int playerID)
    {
        return Input.GetKeyUp(ButtonList[playerID][buttonName]);
    }

    public bool GetOneButtonsDown(string[] buttonName, int playerID)
    {
        for (int i = 0; i < buttonName.Length; i++)
        {
            if (Input.GetKeyDown(ButtonList[playerID][buttonName[i]]))
            {
                return true;
            }
        }
        return false;
    }

    public float GetAxisRaw(string buttonName, int playerID)
    {
        int playerIDfromButton = ((int)ButtonList[playerID][buttonName] - (int)KeyCode.Joystick1Button0 )/ 20;
        playerIDfromButton += 1;
        string axisName = null;

        if (buttonName == Hori)
        {
            axisName = "Hori" + playerIDfromButton;
        }
        else if(buttonName == Verti)
        {
            axisName = "Verti" + playerIDfromButton;
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
}