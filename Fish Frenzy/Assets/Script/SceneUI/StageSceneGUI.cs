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
    public PercentFloat IgnoreTime = new PercentFloat(0, 0.5f);

    private int _currentIndex = 0;
    public StageIdentifier CurrentStage
    {
        get { return Stages[_currentIndex]; }
    }

    [Header("SoundEffect")]
    public SoundEffect sfx_nav;
    public SoundEffect sfx_fight;

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
        if (!IgnoreTime.IsAtMax)
        {
            IgnoreTime.AddValue(Time.deltaTime);
            return;
        }
        float axisRawX = JoystickManager.Instance.GetAnyPlayerAxisRaw("Dhori");
        float axisRawY = JoystickManager.Instance.GetAnyPlayerAxisRaw("Dverti");

        if (axisRawX == 0.0f) { axisRawX = JoystickManager.Instance.GetAnyPlayerAxisRaw("Hori"); }
        if (axisRawY == 0.0f) { axisRawY = JoystickManager.Instance.GetAnyPlayerAxisRaw("Verti"); }

        if (sClass.intervalCheck(axisRawX, -0.9f, 0.9f, true))
        {
            ChangeStageIndex(sClass.getSign(axisRawX, 0.015f));
            SoundManager.Instance.PlaySound(sfx_nav,transform.position);
        }

        if (JoystickManager.Instance.GetAnyPlayerButtonDown("Fishing"))
        {
            MenuGUI.Instance.ChangeSubSceneIndex(-1,false);
            SoundManager.Instance.PlaySound(sfx_fight,transform.position);
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
        nexIndex = (nexIndex + Stages.Count) % Stages.Count;
        IgnoreTime.SetValue01(0);

        SetStageIndex(nexIndex);
    }

    private void SetStageIndex(int ind)
    {
        _currentIndex = ind;
        StageImage.sprite = CurrentStage.stageImage;
        StageName.sprite = CurrentStage.stageName;
    }
}
