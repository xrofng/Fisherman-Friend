﻿using System.Collections;
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

    public Image panel;
    public Image rank;
    public int matchScore = 0;

    public int[] koCount = new int[3];
    public int[] deathCount = new int[3];
    public int[] stageKoCount = new int[3];

    public Text[] ko = new Text[3];
    public Text[] death = new Text[3];
    public Text[] stage = new Text[3];

    public Text totalKoText;
    public Text totalDeathText;
    public Text totalStageText;
    public Text totalScoreText;

    void Start ()
    {

    }
	
	void Update () {
		
	}

    public void UpdateText()
    {
        int totalKo = 0;
        int totalDeath = 0;
        int totalSko = 0;

        for (int i = 0; i < stage.Length; i++)
        {
            stage[i].text += stageKoCount[i];
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
            death[i].text += deathCount[i];
            ko[i].text += koCount[i];
        }
        matchScore = totalKo - totalDeath - totalSko;
        totalKoText.text = "+" + totalKo;
        totalDeathText.text = "-" + totalDeath;
        totalStageText.text = "-" + totalSko;
        totalScoreText.text = "" + matchScore;
    }
}
