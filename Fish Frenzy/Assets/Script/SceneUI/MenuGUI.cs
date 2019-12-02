using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGUI : Singleton<MenuGUI>
{
    public List<GameSceneGUI> SubSceneGUIs = new List<GameSceneGUI>();

    private int _currentSubScene;

    public GameSceneGUI CurrentSubScene
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

    public void ChangeSubSceneIndex(int increment)
    {
        int nexIndex = _currentSubScene + increment;
        nexIndex = Mathf.Clamp(nexIndex, 0, SubSceneGUIs.Count);

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
            SubSceneGUIs[ind].gameObject.SetActive(active);
            if (active)
            {
                CurrentSubScene.OnGameSceneActive();
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
