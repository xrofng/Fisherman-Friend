using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Handles all GUI effects and changes
/// </summary>
public class GUIManager : Singleton<GUIManager>
{
    [Header("Bindings")]
    /// the game object that contains the heads up display (avatar, health, points...)
    public GameObject HUD;
    /// the pause screen game object
    public GameObject PauseScreen;
    /// the time splash gameobject
    public GameObject TimeSplash;
    /// The mobile buttons
    public CanvasGroup Buttons;
    /// The mobile arrows
    public CanvasGroup Arrows;
    /// The mobile movement joystick
    public CanvasGroup Joystick;
    /// the points counter
    public Text PointsText;
    /// the level display
    public Text LevelText;

    [Header("Player UI")]
    /// players damage percent
    public List<Text> PercentText;
    /// players damage percent
    public List<RectTransform> ButtonIndicators;
    /// players damage percent
    public List<RectTransform> MashButtonIndicators;

    /// players round time left
    public Text TimeText;

    /// main game manager
    private PortRoyal portroyal;

    [Header("Settings")]
    /// the pattern to apply when displaying the score
    public string PointsPattern = "000000";

    protected float _initialJoystickAlpha;
    protected float _initialButtonsAlpha;

    /// <summary>
    /// Initialization
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        portroyal = PortRoyal.Instance;
        if (Joystick != null)
        {
            _initialJoystickAlpha = Joystick.alpha;
        }
        if (Buttons != null)
        {
            _initialButtonsAlpha = Buttons.alpha;
        }
    }

    /// <summary>
    /// Initialization
    /// </summary>
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        UpdateDamagePercent();
        UpdateGameTime();
    }

    /// <summary>
    /// Sets the HUD active or inactive
    /// </summary>
    /// <param name="state">If set to <c>true</c> turns the HUD active, turns it off otherwise.</param>
    public virtual void SetHUDActive(bool state)
    {
        if (HUD != null)
        {
            HUD.SetActive(state);
        }
        if (PointsText != null)
        {
            PointsText.enabled = state;
        }
        if (LevelText != null)
        {
            LevelText.enabled = state;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public virtual void UpdateDamagePercent()
    {
        for(int playerID = 0; playerID < 4; playerID++)
        {
            PercentText[playerID].text = portroyal.player[playerID].dPercent + "%";
        }      
        //PercentText[playerID].color = colorScale(portroyal.player[i].dPercent, lowWhite, superRed_P);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public virtual void UpdateGameTime()
    {
        TimeText.text = (int)GameLoop.Instance.Time_Minute + ":" + (int)GameLoop.Instance.Time_Second;
    }


    public virtual void UpdateFishButtonIndicator(int playerID, Vector3 fishingPosition,bool isActive)
    {
        ButtonIndicators[playerID - 1].gameObject.SetActive(isActive);
        if (!isActive)
        {
            return;
        }
        ButtonIndicators[playerID - 1].position = portroyal.mainCamera.WorldToScreenPoint(fishingPosition);
    }
    public virtual void UpdateMashFishingButtonIndicator(int playerID, Vector3 fishingPosition, bool isActive)
    {
        UpdateFishButtonIndicator(playerID, fishingPosition, false);
        MashButtonIndicators[playerID - 1].gameObject.SetActive(isActive);
        if (!isActive)
        {
            return;
        }
        MashButtonIndicators[playerID - 1].position = portroyal.mainCamera.WorldToScreenPoint(fishingPosition);
    }

    /// <summary>
    /// Sets the avatar active or inactive
    /// </summary>
    /// <param name="state">If set to <c>true</c> turns the HUD active, turns it off otherwise.</param>
    public virtual void SetAvatarActive(bool state)
    {
        if (HUD != null)
        {
            HUD.SetActive(state);
        }
    }

    /// <summary>
    /// Sets the pause.
    /// </summary>
    /// <param name="state">If set to <c>true</c>, sets the pause.</param>
    public virtual void SetPause(bool state)
    {
        if (PauseScreen != null)
        {
            PauseScreen.SetActive(state);
            EventSystem.current.sendNavigationEvents = true;
        }
    }


    /// <summary>
    /// Sets the time splash.
    /// </summary>
    /// <param name="state">If set to <c>true</c>, turns the timesplash on.</param>
    public virtual void SetTimeSplash(bool state)
    {
        if (TimeSplash != null)
        {
            TimeSplash.SetActive(state);
        }
    }

    /// <summary>
    /// Sets the level name in the HUD
    /// </summary>
    public virtual void SetLevelName(string name)
    {
        if (LevelText != null)
        {
            LevelText.text = name;
        }
    }


}