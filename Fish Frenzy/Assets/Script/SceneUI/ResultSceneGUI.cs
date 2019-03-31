using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneGUI : MonoBehaviour
{
    [Header("UI_Timing")]
    public float ignoreInputDuration;
    public float victoryAnimDuration;
    public float hideLoserDuration;
    private bool resultShow;

    public ResultPanel resultPanelRef;
    //private List<ResultPanel> panelList = new List<ResultPanel>();
    public Canvas resultCanvas;

    public float panelDistance;
    //
    public List<Sprite> panelSprite = new List<Sprite>();
    public List<Sprite> rankSpriteList = new List<Sprite>();
    public List<ResultPanel> playerPanel = new List<ResultPanel>();

    protected int winnerId;

    protected int maxNumPlayer;
    protected int numPlayer;
    public GameObject playerPrefab;
    public List<string> stageEnvironmentName = new List<string>();

    [Header("Intro")]
    public List<ResultIntro> playerIntro = new List<ResultIntro>();
    private List<int> playerIdByRank = new List<int>();
    private bool showLoser;
    public List<Text> playerTextIndencator = new List<Text>();

    // Use this for initialization
    void Start () {
        maxNumPlayer = PlayerData.Instance.maxNumPlayer;
        numPlayer = PlayerData.Instance.numPlayer;
        
        float offsetX = (panelDistance / 2) * (maxNumPlayer - numPlayer);
        for(int i = 0; i < numPlayer; i++)
        {
            ResultPanel thisPanel = Instantiate(resultPanelRef, resultCanvas.transform) as ResultPanel;
            thisPanel.Rect.anchoredPosition += Vector2.right * offsetX;
            thisPanel.Rect.anchoredPosition += Vector2.right * panelDistance * i;
            thisPanel.panel.sprite = panelSprite[i];
            thisPanel.playerId = i+1;
            playerPanel.Add(thisPanel);
        }

        ComputeResultValue();

        UpdateAllResultText();

        EvaluateScore();

        //MaterialManager.Instance.GetChangedColorPlayer(playerIntro[0].characterModel, PlayerData.Instance.playerSkinId[playerIdByRank[0]]);

        for (int i = 0; i < numPlayer; i++)
        {
            MaterialManager.Instance.GetChangedColorPlayer(playerIntro[i].characterModel, PlayerData.Instance.playerSkinId[playerIdByRank[i]]);
            playerIntro[i].ChangeBackDropColor(playerIdByRank[i]);
        }
        
        resultCanvas.gameObject.SetActive(false);
    }
    /// <summary>
    /// store id
    /// </summary>
    void ComputeResultValue()
    {
        // loop through panel check KnockBy list of each player
        for (int pId = 0; pId < playerPanel.Count; pId++)
        {
            List<string> knockBy = MatchResult.Instance.KnockByList_Name[pId];
            foreach (string knocker in knockBy)
            {
                if(knocker[0] == 'P')
                {
                    PlayerKnockerCheck(pId, knocker);
                }
                StageEnviKnockerCheck(pId, knocker);
            }
        }
    }

    /// <summary>
    /// change rank image for each player, store id of winner
    /// </summary>
    void EvaluateScore()
    {
        List<ResultPanel> sortPanel = playerPanel;
        //sort from max to min
        sortPanel.Sort(delegate (ResultPanel x, ResultPanel y)
        {
            if (y.matchScore.CompareTo(x.matchScore) == 0)
            {
                // if equal rank cchange TODO
            }
            return y.matchScore.CompareTo(x.matchScore);
        });
        // loop through panel check KnockBy list of each player
        for (int pId = 0; pId < numPlayer; pId++)
        {
            playerIdByRank.Add(sortPanel[pId].playerId-1);
            sortPanel[pId].rank.sprite = rankSpriteList[pId];
        }

        winnerId = playerIdByRank[0];
        
    }

    void StageEnviKnockerCheck(int pId, string knockerName)
    {
        // loop through all stage envi
        for (int sId = 0; sId < stageEnvironmentName.Count; sId++)
        {
            if (knockerName == stageEnvironmentName[sId])
            {
                playerPanel[pId].stageKoCount[sId] += 1;
            }
        }
    }

    void PlayerKnockerCheck(int pId, string knockerName)
    {
        // loop through all player
        for (int kId = 0; kId < numPlayer; kId++)
        {
            int KID = kId + 1;
            if (knockerName == "Player" + KID)
            {
                // find kill self
                if (pId == kId)
                {
                    playerPanel[pId].deathCount[IndexInMyPanel(pId, kId)] += 1;
                }
                else
                {
                    playerPanel[pId].deathCount[IndexInMyPanel(pId, kId)] += 1;
                    playerPanel[kId].koCount[IndexInMyPanel(kId, pId)] += 1;
                }
            }
        }
    }

    // convert index when othreId is in panel of myId
    int IndexInMyPanel(int myId , int otherId)
    {
        if (otherId > myId)
        {
            return otherId - 1;
        }
        return otherId;
    }

    void UpdateAllResultText()
    {
        for (int pId = 0; pId < playerPanel.Count; pId++)
        {
            playerPanel[pId].UpdateText();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        hideLoserDuration -= Time.deltaTime;
        if (hideLoserDuration <= 0)
        {
            if (!showLoser)
            {
                ShowAllResultIntro();
                ShowResultDecoratedHud();
                showLoser = true;
            }
        }

        victoryAnimDuration -= Time.deltaTime;
        ignoreInputDuration -= Time.deltaTime;
        if (ignoreInputDuration > 0 || victoryAnimDuration >0)
        {
            return;
        }

        if (!JoystickManager.Instance.GetAnyPlayerButtonDown("Jump"))
        {
            return;
        }
        if (!resultShow)
        {
            ignoreInputDuration = 0.75f;
            resultShow = true;
            resultCanvas.gameObject.SetActive(true);
        }
        else
        {
            ignoreInputDuration = 10000;
            GetComponent<AudioSource>().Play();
            Initiate.FadeToLoading("CharacterSelect", Color.white, 2.0f);
        }
    }

    private void ShowAllResultIntro()
    {
        for (int i = 0; i < numPlayer; i++)
        {
            int pNumber = playerIdByRank[i] + 1;
            string prefix = "P";
            if ( i == 0)
            {
                prefix = "Player ";
            }
            playerTextIndencator[i].text = prefix + pNumber;
        }

        for (int i = 0; i < numPlayer; i++)
        {
            playerIntro[i].gameObject.SetActive(true);
        }
    }

    void ShowResultDecoratedHud()
    {
        foreach (GameObjectMovement moveiObj in FindObjectsOfType<GameObjectMovement>())
        {
            moveiObj.StartMove();
        }
    }
}
