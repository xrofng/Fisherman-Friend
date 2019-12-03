using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGUI : Singleton<MenuGUI>
{
    [System.Serializable]
    public class SubSceneSUI
    {
        public string name;
        public GameSceneGUI GameSceneGUI;
    }

    public List<SubSceneSUI> SubSceneGUIs = new List<SubSceneSUI>();
    public Animator BackgroundCameraAnimator;
    private int _currentSubScene;
    private bool _moveCam;

    public SubSceneSUI CurrentSubScene
    {
        get
        {
            return SubSceneGUIs[_currentSubScene];
        }
    }

    // Use this for initialization
    void Start ()
    {
        _currentSubScene = 0;
        SetSubSceneIndex(0);
    }

    public void ChangeSubSceneIndex(int increment, bool moveCam = true)
    {
        int nexIndex = _currentSubScene + increment;
        nexIndex = Mathf.Clamp(nexIndex, 0, SubSceneGUIs.Count);
        _moveCam = moveCam;
        SetSubSceneIndex(nexIndex);
    }

    private void SetSubSceneIndex(int ind)
    {
        SetSubSceneActive(false, _currentSubScene);
        _currentSubScene = ind;
        SetSubSceneActive(true, _currentSubScene);
    }

    private void SetSubSceneActive(bool active,int ind)
    {
        if (ind < SubSceneGUIs.Count)
        {
            SubSceneGUIs[ind].GameSceneGUI.gameObject.SetActive(active);
            if (active)
            {
                if (_moveCam)
                {
                    BackgroundCameraAnimator.SetTrigger(SubSceneGUIs[ind].name);
                }
                SubSceneGUIs[ind].GameSceneGUI.OnGameSceneActive();
            }
        }
    }
}
