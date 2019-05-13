using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Agent_Fisherman : Agent
{
    protected override void InitFuzzy()
    {
        base.InitFuzzy();
        _fuzzy = GetComponent<Agent_Fisherman_Fuzzy>();
    }

    protected override void InitPlanner()
    {
        base.InitPlanner();
        _planner = GetComponent<Agent_Fisherman_Planner>();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    private Player _ownerPlayer;
    public Player OwnerPlayer
    {
        get
        {
            if (!_ownerPlayer)
            {
                _ownerPlayer = GetComponent<Player>();
            }
            return _ownerPlayer;
        }
    }

    private double iq;
    public double GetIQ()
    {
        return iq;
    }
    
}
