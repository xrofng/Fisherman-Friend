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
    private float timeCount = 0.0f;
    private bool resultShow;

    [Header("Result Panel")]
    public ResultPanel resultPanelPrefab;
    public RectTransform resultPanelHolder;
    public Canvas resultCanvas;
    public float panelDistance;
    public List<Sprite> panelSprite = new List<Sprite>();
    public List<Sprite> rankSpriteList = new List<Sprite>();
    public List<ResultPanel> resultPanels = new List<ResultPanel>();


    [Header("Decorate")]
    public List<ResultIntro> playerIntro = new List<ResultIntro>();
    public List<Text> playerTextIndencator = new List<Text>();
    protected int winnerId;
    public Image winnerTabImage;
    public List<ResultIntroLose> loserObjMoves = new List<ResultIntroLose>();
    public List<GameObjectMovement> decorateObjMoves = new List<GameObjectMovement>();
    public GameObject pressAny;

    [Header("SoundEffect")]
    public SoundEffect sfx_next;
    public SoundEffect sfx_endScreen;


    public enum ResultState
    {
        highlight = 0,
        finishHighlight,
        viewingResult
    }


    [Header("General For Result Screen")]
    public ResultState resultSceneState;
    public List<string> stageEnvironmentName = new List<string>();
    private List<int> playerIdByRank = new List<int>();
    private bool showLoser;
    protected int maxNumPlayer;
    protected int numPlayer;

    // Use this for initialization
    void Start ()
    {
        maxNumPlayer = PlayerData.Instance.maxNumPlayer;
        numPlayer = PlayerData.Instance.numPlayer;

        float offsetX = (panelDistance / 2) * (maxNumPlayer - numPlayer);

        for (int i = 0; i < numPlayer; i++)
        {
            ResultPanel thisPanel = Instantiate(resultPanelPrefab, resultPanelHolder.transform) as ResultPanel;
            thisPanel.playerId = i;
            resultPanels.Add(thisPanel);
        }

        ComputeResultValue();

        UpdateAllResultText();

        EvaluateScore();

        PlayerData playerData = PlayerData.Instance;

        MaterialManager.Instance.GetChangedHatPlayer(playerIntro[0].characterModel, playerData.hatId[playerIdByRank[0]],170);
        MaterialManager.Instance.GetChangedColorPlayer(playerIntro[0].characterModel.gameObject, playerData.playerSkinId[playerIdByRank[0]]);

        for (int i = 0; i < numPlayer; i++)
        {
            MaterialManager.Instance.GetChangedHatPlayer(playerIntro[i].characterModel, playerData.hatId[playerIdByRank[i]],170);
            MaterialManager.Instance.GetChangedColorPlayer(playerIntro[i].characterModel.gameObject, playerData.playerSkinId[playerIdByRank[i]]);
            playerIntro[i].ChangeBackDropColor(playerIdByRank[i]);
        }
        // winner update
        winnerTabImage.color = PlayerData.Instance.GetColor(playerIdByRank[0]);
        int winnerNumber = playerIdByRank[0] + 1;
        playerTextIndencator[0].text = "Player " + winnerNumber;
        playerIntro[0].SetPlayerId(playerIdByRank[0]);
        playerIntro[0].PlayResult();
        
        resultCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// store id
    /// </summary>
    void ComputeResultValue()
    {
        // loop through panel check KnockBy list of each player
        for (int pId = 0; pId < resultPanels.Count; pId++)
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
        List<ResultPanel> sortPanel = resultPanels;
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
            playerIdByRank.Add(sortPanel[pId].playerId);
            sortPanel[pId].rankImage.sprite = rankSpriteList[pId];
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
                resultPanels[pId].stageKoCount[sId] += 1;
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
                    resultPanels[pId].deathCount[IndexInMyPanel(pId, kId)] += 1;
                }
                else
                {
                    resultPanels[pId].deathCount[IndexInMyPanel(pId, kId)] += 1;
                    resultPanels[kId].koCount[IndexInMyPanel(kId, pId)] += 1;
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
        for (int pId = 0; pId < resultPanels.Count; pId++)
        {
            resultPanels[pId].UpdateText();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeCount += Time.deltaTime;
        if (timeCount <= ignoreInputDuration || timeCount <= victoryAnimDuration)
        {
            return;
        }

        if (resultSceneState == ResultState.highlight)
        {
            if (timeCount > hideLoserDuration)
            {
                if (!showLoser)
                {
                    ShowAllResultIntro();
                    ShowResultDecoratedHud();
                    showLoser = true;
                    resultSceneState = ResultState.finishHighlight;
                }
            }
        }

        if (resultSceneState == ResultState.finishHighlight)
        {
            if (JoystickManager.Instance.GetAnyPlayerButtonDown("Jump"))
            {
                pressAny.SetActive(false);
                resultSceneState = ResultState.viewingResult;

                ignoreInputDuration = 0.75f;
                resultShow = true;
                resultCanvas.gameObject.SetActive(true);
                SoundManager.Instance.PlaySound(sfx_next,this.transform.position);
            }
        }

        if (resultSceneState == ResultState.viewingResult)
        {
            if (AllPlayerReady)
            {
                ignoreInputDuration = 10000;
                GetComponent<AudioSource>().Play();
                SoundManager.Instance.PlaySound(sfx_endScreen, this.transform.position);
                Initiate.FadeToLoading("Menu", Color.white, 2.0f);
            }
        }

    }

    private bool AllPlayerReady
    {
        get
        {
            foreach (ResultPanel resultPanel in resultPanels)
            {
                if (!resultPanel.playerReady)
                {
                    return false;
                }
            }
            return true;
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
            playerIntro[i].SetPlayerId(playerIdByRank[i]);
            playerIntro[i].PlayResult();
        }
    }

    void ShowResultDecoratedHud()
    {
        // Losers fanfare
        for (int i = 0; i < loserObjMoves.Count; i++)
        {
            if (i+1< numPlayer)
            {
                loserObjMoves[i].StartShowResultIntro();
            }
        }

        foreach (GameObjectMovement decorateObjMove in decorateObjMoves)
        {
            decorateObjMove.StartMove();
        }
        
    }
}
