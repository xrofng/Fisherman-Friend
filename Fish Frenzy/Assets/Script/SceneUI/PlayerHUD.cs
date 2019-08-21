using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public enum FaceExpresstion
    {
        Idle,
        Death,
        Hurt
    }
    public FaceExpresstion previousFaceExpression = FaceExpresstion.Idle;
    public FaceExpresstion currentFaceExpression = FaceExpresstion.Idle;
    public Image faceImage;
    public List<Sprite> faceList = new List<Sprite>();
    public Image hatImage;
    //public Animator faceAnim;
    /*
    
    */

    public Text percentText;
    public Image durabilityImage;

    public Image fishNameImage;
    public Image fishIconImage;
    public Image fishStoreIconImage;
    public Sprite transparentSprite;


    public VerticalLayoutGroup scoreChangeLayout;
    protected List<GameObject> scoreChangeList = new List<GameObject>();

    private Player _hudOwner;
    public Player HudOwner
    {
        get
        {
            if(_hudOwner == null)
            {
                _hudOwner = FFGameManager.Instance.PortRoyal.Player[playerID];
            }
            return _hudOwner;
        }
    }
    private int playerID = -99;

    private PlayerData playerData;
    private KnockData knockData;


    public void Initialize(int playerId)
    {
        playerData = PlayerData.Instance;
        knockData = FFGameManager.Instance.KnockData;
        playerID = playerId;

        // durability
        float a = durabilityImage.color.a;
        Color pColor = playerData.playerColor[playerData.playerSkinId[playerID]];
        durabilityImage.color = new Color(pColor.r, pColor.g, pColor.b, a);

        //meter
        GetComponent<FFgui>().GUIRecolorer.Recolor(pColor);

        //face
        hatImage.sprite = playerData.GetHatSprite(playerId);
    }

    public void UpdatePlayerHUD()
    {
        if(HudOwner == null)
        {
            return;
        }
        UpdateDamagePercent();
        UpdateFaceSprite();
        UpdateFishDurability();
        UpdateFishIcon();
    }

    /// <summary>
    /// update damage percent text and color
    /// </summary>
    void UpdateDamagePercent()
    {
        percentText.text = HudOwner.damagePercent + "%";
        percentText.color = knockData.GetColor(HudOwner.damagePercent);
    }

    /// <summary>
    /// Change face expression due to player state
    /// </summary>
    void UpdateFaceSprite()
    {
        if (HudOwner._cPlayerState.IsDeath)
        {
            currentFaceExpression = FaceExpresstion.Death;
        }
        else if (HudOwner._cPlayerState.IsDamaged)
        {
            currentFaceExpression = FaceExpresstion.Hurt;
        }
        else
        {
            currentFaceExpression = FaceExpresstion.Idle;
        }
        if (currentFaceExpression != previousFaceExpression)
        {
            faceImage.sprite = faceList[(int)currentFaceExpression];
            previousFaceExpression = currentFaceExpression;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void UpdateFishDurability()
    {
        if (HudOwner.holdingFish)
        {
            durabilityImage.fillAmount = HudOwner.mainFish.GetDurabilityRatio;
            return;
        }
        durabilityImage.fillAmount = 0;
    }

    /// <summary>
    /// show main/sub fish icon, if player have them . If not show transparent
    /// </summary>
    void UpdateFishIcon()
    {
        if (HudOwner.subFish)
        {
            fishStoreIconImage.sprite = HudOwner.subFish.fishStored;
        }
        else
        {
            fishStoreIconImage.sprite = transparentSprite;
        }
        if (!HudOwner.holdingFish)
        {
            fishIconImage.sprite = transparentSprite;
            fishNameImage.sprite = transparentSprite;
            return;
        }
        fishIconImage.sprite = HudOwner.mainFish.fishIcon;
        fishNameImage.sprite = HudOwner.mainFish.fishName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scoreChangeObject"></param>
    /// <param name="scoreChangeFrameDuration"></param>
    public void UpdateScoreChange(GameObject scoreChangeObject, int scoreChangeFrameDuration)
    {
        StartCoroutine(ieShowScoreChange(scoreChangeObject, scoreChangeFrameDuration));
    }

    IEnumerator ieShowScoreChange(GameObject scoreChangeObject, int frameDuration)
    {
        GameObject newScoreChange = Instantiate(scoreChangeObject, scoreChangeLayout.transform);

        scoreChangeList.Add(newScoreChange);
        int frameCount = 0;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        if (scoreChangeList.Contains(newScoreChange))
        {
            scoreChangeList.Remove(newScoreChange);
            Destroy(newScoreChange.gameObject, 2.0f);
        }
    }
}
