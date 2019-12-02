using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFGameManager : PersistentSingleton<FFGameManager>
{
    protected MultiPlayerCamera _multiplayerCamera;
    public MultiPlayerCamera MultiplayerCamera
    {
        get
        {
            if (_multiplayerCamera == null)
            {
                _multiplayerCamera = FindObjectOfType<MultiPlayerCamera>();
            }
            return _multiplayerCamera;
        }
    }

    protected FocusCamera _focusCamera;
    public FocusCamera FocusCamera
    {
        get
        {
            if (_focusCamera == null)
            {
                _focusCamera = FindObjectOfType<FocusCamera>();
            }
            return _focusCamera;
        }
    }

    protected GameLoop _gameLoop;
    public GameLoop GameLoop
    {
        get
        {
            if (_gameLoop == null)
            {
                _gameLoop = FindObjectOfType<GameLoop>();
            }
            return _gameLoop;
        }
    }

    protected PortRoyal _portRoyal;
    public PortRoyal PortRoyal
    {
        get
        {
            if (_portRoyal == null)
            {
                _portRoyal = FindObjectOfType<PortRoyal>();
            }
            return _portRoyal;
        }
    }

    protected KnockData _knockData;
    public KnockData KnockData
    {
        get
        {
            if (_knockData == null)
            {
                _knockData = FindObjectOfType<KnockData>();
            }
            return _knockData;
        }
    }

    protected PathManager _pathManager;
    public PathManager PathManager
    {
        get
        {
            if (_pathManager == null)
            {
                _pathManager = FindObjectOfType<PathManager>();
            }
            return _pathManager;
        }
    }

    protected GUIManager _guiManager;
    public GUIManager GUIManager
    {
        get
        {
            if (_guiManager == null)
            {
                _guiManager = FindObjectOfType<GUIManager>();
            }
            return _guiManager;
        }
    }

    public StageIdentifier CurrentStage { get; set; }
}

