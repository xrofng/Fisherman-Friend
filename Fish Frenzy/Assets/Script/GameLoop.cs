using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : PersistentSingleton<GameLoop>
{
    public float Round_Time_Limit = 360;
    private float timeCountDown;
    public float Time_Minute
    {
        get { return timeCountDown / 60; }
    }
    public float Time_Second
    {
        get { return timeCountDown % 60; }
    }
    // Use this for initialization
    void Start () {
        timeCountDown = Round_Time_Limit;
	}
	
	// Update is called once per frame
	void Update () {
        timeCountDown -= Time.deltaTime;
        //if()
	}
}
