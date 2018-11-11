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

    public List<Sprite> playerPanel = new List<Sprite>();
    public List<Sprite> rankSpriteList = new List<Sprite>();


    protected int maxNumPlayer;
    protected int numPlayer;
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
            thisPanel.panel.sprite = playerPanel[i];
        }
        Destroy(resultPanelRef.gameObject);
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
