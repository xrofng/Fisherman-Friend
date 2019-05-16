using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class Agent_Shark : Agent, MMEventListener<PlayerSpawnedEvent>
{

    private bool startFindPlayer;

    protected override void InitFuzzy()
    {
        base.InitFuzzy();
    }

    protected override void InitPlanner()
    {
        base.InitPlanner();
    }

    protected override void InitPahtFinder()
    {
        base.InitPahtFinder();
    }

    protected override void Initialize()
    {
        InitPahtFinder();
    }

    private SI_Shark _shark;
    public SI_Shark Shark
    {
        get
        {
            if (!_shark)
            {
                _shark = GetComponent<SI_Shark>();
            }
            return _shark;
        }
    }

    protected override void Tick()
    {
        if (!startFindPlayer)
        {
            return;
        }
        PathFinder.SetTarget(Shark.NearestPlayer);
    }

    public void OnMMEvent(PlayerSpawnedEvent eventType)
    {
        startFindPlayer = true;
        PathFinder.StartPathFinding();
    }
}
