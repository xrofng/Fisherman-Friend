using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public ResultPanel resultPanelRef;
    //private List<ResultPanel> panelList = new List<ResultPanel>();
    public Canvas mainCanvas;

    public float panelDistance;
    //
    public List<Sprite> panelSprite = new List<Sprite>();
    public List<Sprite> rankSpriteList = new List<Sprite>();
    public List<ResultPanel> playerPanel = new List<ResultPanel>();


    protected int maxNumPlayer;
    protected int numPlayer;
    public List<string> stageEnvironmentName = new List<string>();
    // Use this for initialization
    void Start () {
        maxNumPlayer = MatchResult.Instance.maxNumPlayer;
        numPlayer = MatchResult.Instance.numPlayer;

        float offsetX = (panelDistance / 2) * (maxNumPlayer - numPlayer);
        for(int i = 0; i < numPlayer; i++)
        {
            ResultPanel thisPanel = Instantiate(resultPanelRef, mainCanvas.transform) as ResultPanel;
            thisPanel.myRect.anchoredPosition += Vector2.right * offsetX;
            thisPanel.myRect.anchoredPosition += Vector2.right * panelDistance * i;
            thisPanel.panel.sprite = panelSprite[i];

            playerPanel.Add(thisPanel);
        }
        Destroy(resultPanelRef.gameObject);

        ComputeResultValue();

        UpdateAllResultText();
    }

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
	void Update () {
        if (Input.anyKey)
        {
            GetComponent<AudioSource>().Play();
            Initiate.Fade("Start Menu", Color.white, 2.0f);
        }
    }
    
}
