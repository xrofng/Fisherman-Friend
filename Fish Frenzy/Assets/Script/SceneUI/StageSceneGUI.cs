using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Xrofng;
using UnityEngine.PostProcessing;

[System.Serializable]
public class StageIdentifier
{
    public string sceneName;
    public Sprite stageImage;
    public Sprite stageName;
    public GameObject model;
    public PostProcessingProfile postProcessProfile;
    public Material Sky;
}

public class StageSceneGUI : GameSceneGUI
{
    public List<StageIdentifier> Stages = new List<StageIdentifier>();
    public Image StageImage;
    public Image StageName;

    private int _currentIndex = 0;
    public StageIdentifier CurrentStage
    {
        get { return Stages[_currentIndex]; }
    }

    [Header("SoundEffect")]
    public SoundEffect sfx_ready;
    public SoundEffect sfx_ready_fight;

    // Use this for initialization
    void Start ()
    {
		
	}

    public override void OnGameSceneActive()
    {
        base.OnGameSceneActive();
        SetStageIndex(0);
    }

    void Update ()
    {
        float axisRawX = JoystickManager.Instance.GetAnyPlayerAxisRaw("Dhori");
        float axisRawY = JoystickManager.Instance.GetAnyPlayerAxisRaw("Dverti");

        if (axisRawX == 0.0f) { axisRawX = JoystickManager.Instance.GetAnyPlayerAxisRaw("Hori"); }
        if (axisRawY == 0.0f) { axisRawY = JoystickManager.Instance.GetAnyPlayerAxisRaw("Verti"); }

        if (sClass.intervalCheck(axisRawX, -0.9f, 0.9f, true))
        {
            ChangeStageIndex(sClass.getSign(axisRawX, 0.015f));
        }

        if (JoystickManager.Instance.GetAnyPlayerButtonDown("Jump") || JoystickManager.Instance.GetAnyPlayerButtonDown("Pause"))
        {
            FFGameManager.Instance.CurrentStage = CurrentStage;
            Initiate.FadeToLoading(CurrentStage.sceneName,Color.white,2.0f);
        }
    }

    public void ChangeStageIndex(int increment)
    {
        int nexIndex = _currentIndex + increment;
        nexIndex = Mathf.Clamp(nexIndex, 0, Stages.Count-1);

        SetStageIndex(nexIndex);
    }

    private void SetStageIndex(int ind)
    {
        _currentIndex = ind;
        StageImage.sprite = CurrentStage.stageImage;
        StageName.sprite = CurrentStage.stageName;
    }
}
