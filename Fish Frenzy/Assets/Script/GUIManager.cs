using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Handles all GUI effects and changes
/// </summary>
public class GUIManager : MonoBehaviour
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
    /// the big display in the middle of scene
    public Text GrandText;
    /// players round time left
    public Text TimeText;

    [Header("Player UI")]
    /// button indicator of fishing and pickup
    public List<RectTransform> ButtonIndicators;
    /// players damage percent
    public List<RectTransform> MashButtonIndicators;
    /// Indicator position from fish
    public Vector3 FishingIndicatorOffset;
    /// Indicator position from fish
    public Vector3 PickUpIndicatorOffset;
    /// player image
    public List<Image> PlayerImage;
    /// player normal face sprite
    public List<Sprite> NormaleSprite;
    /// player death face sprite
    public List<Sprite> DeathSprite;
    /// player damaged face sprite
    public List<Sprite> DamagedSprite;
    /// list of list of sprite 0=normal 1=death 2=damaged
    protected List<List<Sprite>> PlayerSpriteSet = new List<List<Sprite>>();
    ///
    protected int numPlayer;

    [Header("Player HUD")]
    public PlayerHUD Player1_HUD;
    public PlayerHUD Player2_HUD;
    public PlayerHUD Player3_HUD;
    public PlayerHUD Player4_HUD;
    private List<PlayerHUD> PlayerHudList = new List<PlayerHUD>();

    [Header("Score Change")]
    public GameObject scoreChange_Decrease;
    public GameObject scoreChange_Increase;
    public int scoreChangeFrameDuration = 150;
    public Vector3 scoreChangeOffsetDistance;

    [Header("Settings")]
    /// the pattern to apply when displaying the score
    public string PointsPattern = "000000";

    protected float _initialJoystickAlpha;
    protected float _initialButtonsAlpha;

    [Header("Other Class Ref")]
    protected GameLoop gameLoop;
    protected PortRoyal portRoyal;
    protected KnockData knockData;
    protected PlayerData playerData;
    /// <summary>
    /// Initialization
    /// </summary>
    protected void Awake()
    {
        playerData = PlayerData.Instance;
        numPlayer = playerData.numPlayer;

        InitImageList();

        gameLoop = FFGameManager.Instance.GameLoop;
        portRoyal = FFGameManager.Instance.PortRoyal;
        knockData = FFGameManager.Instance.KnockData;

        if (Joystick != null)
        {
            _initialJoystickAlpha = Joystick.alpha;
        }
        if (Buttons != null)
        {
            _initialButtonsAlpha = Buttons.alpha;
        }
        SetUpSpriteSet();
    }

    /// <summary>
    /// Initialization
    /// </summary>
    protected virtual void Start()
    {
        for (int i = 0; i < PlayerHudList.Count; i++)
        {
            PlayerHudList[i].gameObject.SetActive(false);
        }
        for (int pId = 0; pId < numPlayer; pId++)
        {
            PlayerHudList[pId].gameObject.SetActive(true);
            PlayerHudList[pId].Initialize(pId);
        }
    }

    void InitImageList()
    {
        PlayerHudList.Add(Player1_HUD);
        PlayerHudList.Add(Player2_HUD);
        PlayerHudList.Add(Player3_HUD);
        PlayerHudList.Add(Player4_HUD);
    }

    protected virtual void Update()
    {
        if (gameLoop.state == GameLoop.GameState.playing)
        {
            for (int pId = 0; pId < numPlayer; pId++)
            {
                PlayerHudList[pId].UpdatePlayerHUD();
            }
               
            UpdateGameTime();
        }
        if (gameLoop.state == GameLoop.GameState.beforeStart ||
            gameLoop.state == GameLoop.GameState.gameEnd)
        {
            UpdateGrandText();
            UpdateGameTime();
        }
    }

    public virtual void SetUpSpriteSet()
    {
        PlayerSpriteSet.Add(NormaleSprite);
        PlayerSpriteSet.Add(DeathSprite);
        PlayerSpriteSet.Add(DamagedSprite);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public virtual void AddScoreChange(int playerID , int changeValue)
    {
        GameObject scoreChange = scoreChange_Decrease;
        if (changeValue > 0)
        {
            scoreChange = scoreChange_Increase;
        }
        PlayerHudList[playerID].UpdateScoreChange(scoreChange, scoreChangeFrameDuration);
    }


    /// <summary>
    /// 
    /// </summary>
    public virtual void UpdateGrandText()
    {
        if (gameLoop.startCountDown <= 1)
        {
            GrandText.text = "GO";
        }
        else if(gameLoop.startCountDown<=4)
        {
            GrandText.text = (int)gameLoop.startCountDown + "";
        }
        if (gameLoop.TimeInSecond <= 0)
        {
            GrandText.text = "Game";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public virtual void UpdateGameTime()
    {
        if(gameLoop.state == GameLoop.GameState.gameEnd)
        {
            return;
        }
        int second = (int)gameLoop.Time_Second;
        int minute = (int)gameLoop.Time_Minute;
        TimeText.text = minute.ToString("00") + ":" + second.ToString("00");
    }


    public virtual void UpdateFishButtonIndicator(int playerID, Vector3 fishingPosition,bool isActive)
    {
        ButtonIndicators[playerID - 1].gameObject.SetActive(isActive);
        if (!isActive)
        {
            return;
        }
        ButtonIndicators[playerID - 1].position = portRoyal.mainCamera.WorldToScreenPoint(fishingPosition);
    }

    public virtual void UpdateMashFishingButtonIndicator(int playerID, Vector3 fishingPosition, bool isActive)
    {
        UpdateFishButtonIndicator(playerID, fishingPosition, false);
        MashButtonIndicators[playerID - 1].gameObject.SetActive(isActive);
        if (!isActive)
        {
            return;
        }
        MashButtonIndicators[playerID - 1].position = portRoyal.mainCamera.WorldToScreenPoint(fishingPosition) + FishingIndicatorOffset;
    }

    public virtual void UpdatePickUpButtonIndicator(Vector3 fishPosition , Image buttonImage, bool isActive)
    {
        float alphaIncrease = 0.1f;
        if (!isActive)
        {
            alphaIncrease *= -1;
        }
        buttonImage.color = sClass.ChangeColorAlpha(buttonImage.color, buttonImage.color.a +alphaIncrease);
        buttonImage.rectTransform.position = portRoyal.mainCamera.WorldToScreenPoint(fishPosition + PickUpIndicatorOffset);
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

    public virtual T InstantiateUI<T>(MaskableGraphic g) where T : MaskableGraphic
    {
        return Instantiate(g, this.transform) as T;
    }
}