using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentFuzzy_Shark : AgentFuzzy, MMEventListener<PlayerSpawnedEvent>
{

    protected override void Initialize()
    {
        base.Initialize();
        this.MMEventStartListening<PlayerSpawnedEvent>();
    }

    public void OnMMEvent(PlayerSpawnedEvent eventType)
    {

        foreach (Player player in FFGameManager.Instance.PortRoyal.Player)
        {
            //Desirable_TargetPlayer desire_target = AddDesirable<Desirable_TargetPlayer_Shark>();
            //desire_target.targetPlayer = player;
        }
        InitFuzzy();
    }

    // Update is called once per frame
    void Update ()
    {

    }
}
