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
    /// the big display in the middle of scene
    public Text GrandText;
    /// players round time left
    public Text TimeText;

    [Header("Player UI")]
    /// players damage percent
    public List<Text> PercentText;
    /// players damage percent
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

    [Header("Player UI")]
    public Sprite transparentSprite;
    public RectTransform Player1_GUI;
    public RectTransform Player2_GUI;
    public RectTransform Player3_GUI;
    public RectTransform Player4_GUI;
    List<RectTransform> PlayerUIList = new List<RectTransform>();
    /// UI image
    private List<Image> DurabilityImage = new List<Image>();
    private List<Image> IconImage = new List<Image>();
    private List<Image> NameImage = new List<Image>();
    private List<Image> StoreIconImage = new List<Image>();
    private List<RectTransform> ScoreChangeRect = new List<RectTransform>();

    /// 
    protected int[] currentFaceIndex = new int[4];
    protected int[] previousFaceIndex = new int[4];
    protected List<List<RectTransform>> scoreChangeList = new List<List<RectTransform>>();
    [Header("Score Change")]
    public RectTransform scoreChange_Decrease;
    public RectTransform scoreChange_Increase;
    public int scoreChangeFrameDuration = 150;
    public Vector3 scoreChangeOffsetDistance;

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
        InitImageList();
        portroyal = PortRoyal.Instance;
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
        for (int playerID = 0; playerID < 4; playerID++)
        {
            float a = DurabilityImage[playerID].color.a;
            Color pColor= PortRoyal.Instance.startupPlayer.playerColor[playerID];
            DurabilityImage[playerID].color = new Color(pColor.r, pColor.g, pColor.b, a);
        }
    }

    void InitImageList()
    {
        PlayerUIList.Add(Player1_GUI);
        PlayerUIList.Add(Player2_GUI);
        PlayerUIList.Add(Player3_GUI);
        PlayerUIList.Add(Player4_GUI);

        for(int i=0;i<4; i++)
        {
            scoreChangeList.Add(new List<RectTransform>());
        }

        for (int i = 0; i < PlayerUIList.Count; i++)
        {
            RectTransform[] rectInSet = PlayerUIList[i].gameObject.GetComponentsInChildren<RectTransform>();
            foreach (RectTransform rt in rectInSet)
            {
                if (rt.gameObject.name.Contains("FishDurability"))
                {
                    DurabilityImage.Add(rt.GetComponent<Image>());
                }
                if (rt.gameObject.name.Contains("Icon"))
                {
                    IconImage.Add(rt.GetComponent<Image>());
                }
                if (rt.gameObject.name.Contains("Name"))
                {
                    NameImage.Add(rt.GetComponent<Image>());
                }
                if (rt.gameObject.name.Contains("Store"))
                {
                    StoreIconImage.Add(rt.GetComponent<Image>());
                }
                if (rt.gameObject.name.Contains("ScoreChange"))
                {
                    ScoreChangeRect.Add(rt);
                }
            }
        }
    }

    protected virtual void Update()
    {
        if (GameLoop.Instance.state == GameLoop.GameState.playing)
        {
            for (int playerID = 0; playerID < 4; playerID++)
            {
                UpdateDamagePercent(playerID);
                UpdateFaceSprite(playerID);
                UpdateFishDurability(playerID);
                UpdateFishIcon(playerID);
                
            }
               
            UpdateGameTime();
            
        }
        if (GameLoop.Instance.state == GameLoop.GameState.beforeStart ||
            GameLoop.Instance.state == GameLoop.GameState.gameEnd)
        {
            UpdateGrandText();
            UpdateGameTime();
        }
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

    public virtual void SetUpSpriteSet()
    {
        PlayerSpriteSet.Add(NormaleSprite);
        PlayerSpriteSet.Add(DeathSprite);
        PlayerSpriteSet.Add(DamagedSprite);
    }

    /// <summary>
    /// Change face expression due to player state
    /// </summary>
    /// <param name="playerID"></param>
    public virtual void UpdateFaceSprite(int playerID)
    {
        Player _player = portroyal.Player[playerID];
        if (_player._cPlayerState.IsDeath)
        {
            currentFaceIndex[playerID] = 1;
        }
        else if (_player._cPlayerState.IsDamaged)
        {
            currentFaceIndex[playerID] = 2;
        }
        else
        {
            currentFaceIndex[playerID] = 0;
        }
        if (currentFaceIndex[playerID] != previousFaceIndex[playerID])
        {
            PlayerImage[playerID].sprite = PlayerSpriteSet[currentFaceIndex[playerID]][playerID];
            previousFaceIndex[playerID] = currentFaceIndex[playerID];
        }
    }

    /// <summary>
    /// show main/sub fish icon, if player have them . If not show transparent
    /// </summary>
    /// <param name="playerID"></param>
    public virtual void UpdateFishIcon(int playerID)
    {
        Player _player = portroyal.Player[playerID];
        if (_player.subFish)
        {
            StoreIconImage[playerID].sprite = _player.subFish.fishStored;
        }else
        {
            StoreIconImage[playerID].sprite = transparentSprite;
        }
        if (!_player.holdingFish)
        {
            IconImage[playerID].sprite = transparentSprite;
            NameImage[playerID].sprite = transparentSprite;           
            return;
        }
        IconImage[playerID].sprite = _player.mainFish.fishIcon;
        NameImage[playerID].sprite = _player.mainFish.fishName;
      
    }

    /// <summary>
    /// update damage percent text and color
    /// </summary>
    /// <param name="playerID"></param>
    public virtual void UpdateDamagePercent(int playerID)
    {
        PercentText[playerID].text = portroyal.Player[playerID].dPercent + "%";
        PercentText[playerID].color = KnockData.Instance.GetColor(portroyal.Player[playerID].dPercent);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public virtual void UpdateFishDurability(int playerID)
    {
        Player _player = portroyal.Player[playerID];
        if (_player.holdingFish)
        {
            DurabilityImage[playerID].fillAmount = _player.mainFish.GetDurabilityRatio;
            return;
        }
        DurabilityImage[playerID].fillAmount = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    public virtual void AddScoreChange(int playerID , int changeValue)
    {
        RectTransform prompt = scoreChange_Decrease;
        if (changeValue > 0)
        {
            prompt = scoreChange_Increase;
        }
        StartCoroutine(ieShowScoreChange(playerID, prompt, scoreChangeFrameDuration, scoreChangeList[playerID].Count));
    }

    IEnumerator ieShowScoreChange(int playerID,RectTransform promptObj , int frameDuration , int queueIndex)
    {
        RectTransform spawnPrompt = Instantiate(promptObj, ScoreChangeRect[playerID].transform);

        scoreChangeList[playerID].Add(spawnPrompt);
        int frameCount = 0;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        if (scoreChangeList[playerID].Contains(spawnPrompt))
        {
            scoreChangeList[playerID].Remove(spawnPrompt);
            Destroy(spawnPrompt.gameObject, 2.0f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void UpdateGrandText()
    {
        if (GameLoop.Instance.startCountDown <= 1)
        {
            GrandText.text = "GO";
        }
        else if(GameLoop.Instance.startCountDown<=4)
        {
            GrandText.text = (int)GameLoop.Instance.startCountDown + "";
        }
        if (GameLoop.Instance.TimeInSecond <= 0)
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
        if(GameLoop.Instance.state == GameLoop.GameState.gameEnd)
        {
            return;
        }
        int second = (int)GameLoop.Instance.Time_Second;
        int minute = (int)GameLoop.Instance.Time_Minute;
        TimeText.text = minute.ToString("00") + ":" + second.ToString("00");
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
        MashButtonIndicators[playerID - 1].position = portroyal.mainCamera.WorldToScreenPoint(fishingPosition) + FishingIndicatorOffset;
    }

    public virtual void UpdatePickUpButtonIndicator(Vector3 fishPosition , Image buttonImage, bool isActive)
    {
        float alphaIncrease = 0.1f;
        if (!isActive)
        {
            alphaIncrease *= -1;
        }
        buttonImage.color = sClass.ChangeColorAlpha(buttonImage.color, buttonImage.color.a +alphaIncrease);
        buttonImage.rectTransform.position = portroyal.mainCamera.WorldToScreenPoint(fishPosition + PickUpIndicatorOffset);
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