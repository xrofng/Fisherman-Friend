using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Agent : MonoBehaviour
{

    public int onTickRate;

	// Use this for initialization
	void Awake ()
    {
        Initialize();
	}

    protected virtual void Initialize()
    {
        InitFuzzy();
        InitPlanner();
        InitPahtFinder();
    }

    protected AgentFuzzy _fuzzy;
    public AgentFuzzy Fuzzy
    {
        get
        {
            if (!_fuzzy)
            {
                InitFuzzy();
            }
            return _fuzzy;
        }
    }

    protected AgentPlanner _planner;
    public AgentPlanner Planner
    {
        get
        {
            if (!_planner)
            {
                InitPlanner();
                Initialize();
            }
            return _planner;
        }
    }

    protected AgentPahtFinder _pathFinder;
    public AgentPahtFinder PathFinder
    {
        get
        {
            if (!_pathFinder)
            {
                InitPahtFinder();
                Initialize();
            }
            return _pathFinder;
        }
    }
    
    protected virtual void InitPlanner()
    {
        _planner = GetComponent<AgentPlanner>();
        _planner.agent = this;
    }

    protected virtual void InitFuzzy()
    {
        _fuzzy = GetComponent<AgentFuzzy>();
        _fuzzy.agent = this;
    }

    protected virtual void InitPahtFinder()
    {
        _pathFinder = GetComponent<AgentPahtFinder>();
        _pathFinder.agent = this;
    }

    void Update()
    {
        if(onTickRate>0 && Time.frameCount % onTickRate == 0)
        {
            Tick();
        }
        else
        {
            Tick();
        }
    }

    protected virtual void Tick()
    {
        
    }

    public T CastAgent<T>() where T : Agent
    {
        return this as T;
    }
}
