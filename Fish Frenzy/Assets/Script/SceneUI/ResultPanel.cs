using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    public int playerId;
    private RectTransform _rect;
    public RectTransform Rect
    {
        get
        {
            if(_rect == null)
            {
                return _rect = GetComponent<RectTransform>();
            }
            return _rect;
        }
    }

    public Image panelTopic;
    public Sprite[] panelTopicSprites = new Sprite[4];
    public GUIRecolorer recolorUi;
    public Image rankImage;
    public Image hatImage;
    public Image readyImage;
    public Sprite pressXSprites;
    public Sprite readySprites;
    public bool playerReady;

    [Header("Score")]
    public int matchScore = 0;
    public int[] koCount = new int[3];
    public int[] deathCount = new int[3];
    public int[] stageKoCount = new int[3];

    private Text[] ko = new Text[3];
    private Text[] death = new Text[3];
    private Text[] stage = new Text[3];

    [Header("Score Text")]
    public Text totalKoText;
    public Text totalDeathText;
    public Text totalStageKoText;
    public Text totalScoreText;

    void Start ()
    {

    }
	
	void Update ()
    {
        if (JoystickManager.Instance.GetButtonDown("Jump",playerId))
        {
            playerReady = !playerReady;
            readyImage.sprite = playerReady == true ? readySprites : pressXSprites;
        }
	}

    public void UpdateText()
    {
        int totalKo = 0;
        int totalDeath = 0;
        int totalSko = 0;

        panelTopic.sprite = panelTopicSprites[playerId];
        recolorUi.Recolor(PlayerData.Instance.GetColor(playerId));
        hatImage.sprite = PlayerData.Instance.GetHatSprite(playerId);

        for (int i = 0; i < stage.Length; i++)
        {
            //stage[i].text += stageKoCount[i];
            totalSko += stageKoCount[i];
        }
        if(death.Length != ko.Length || deathCount.Length != koCount.Length)
        {
            Debug.LogWarning("player in ko and death is not equal");
        }
      
        for (int i = 0; i < death.Length; i++)
        {
            totalDeath += deathCount[i];
            totalKo += koCount[i];
            //death[i].text += deathCount[i];
            //ko[i].text += koCount[i];
        }
        matchScore = totalKo - totalDeath - totalSko;
        totalKoText.text = "+" + totalKo;
        totalDeathText.text = "-" + totalDeath;
        totalStageKoText.text = "-" + totalSko;
        //totalScoreText.text = "" + matchScore;
    }
}
