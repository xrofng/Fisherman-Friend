using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Agent : MonoBehaviour {
    
	// Use this for initialization
	void Start ()
    {
        Initialize();
	}

    protected virtual void Initialize()
    {
        InitFuzzy();
        InitPlanner();
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

    public T CastAgent<T>() where T : Agent
    {
        return this as T;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
