using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_Whale : Creature
{

    public enum WhaleState
    {
        Wait = 0,
        Coming,
        Splash
    }

    public WhaleState _currentWhaleState;
    public WhaleState CurrentWhaleState
    {
        get { return _currentWhaleState; }
        set
        {
            OnStateEnd(_currentWhaleState);
            _currentWhaleState = value;
            OnStateEnter(_currentWhaleState);
        }
    }

    // The time frenzy start including animation duration
    public float FirstFrenzyTime = 30;
    private float TimeFrenzyStarted;
    public float waitNextWaveDuration = 6;
    public float comingDuration = 9;
    private float waitNextWaveCountDown;
    private float comingCountDown;

    private FrenzySpawner _frenzySpawner;
    public FrenzySpawner FrenzySpawner
    {
        get
        {
            if(_frenzySpawner == null)
            {
                _frenzySpawner = FindObjectOfType<FrenzySpawner>();
            }
            return _frenzySpawner;
        }
    }

    public Animator whaleAnim;
    public SoundEffect sfx_Whale;
    public int whaleAnimFrame = 718;



    // Use this for initialization
    void Start ()
    {
        TimeFrenzyStarted = FirstFrenzyTime + comingDuration + waitNextWaveDuration;
        CurrentWhaleState = WhaleState.Wait;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((FFGameManager.Instance.GameLoop.timeCountDown > TimeFrenzyStarted))
        {
            return;
        }


        switch (CurrentWhaleState)
        {
            case WhaleState.Wait: IdleUpdate();  break;
            case WhaleState.Coming: ComingUpdate(); break;
            case WhaleState.Splash: SplashUpdate(); break;
        }
    }

    private void SplashUpdate()
    {
        throw new NotImplementedException();
    }

    private void ComingUpdate()
    {
        comingCountDown -= Time.deltaTime;
        if (comingCountDown <= 0)
        {
            CurrentWhaleState = WhaleState.Splash;
        }
    }

    private void IdleUpdate()
    {
        waitNextWaveCountDown -= Time.deltaTime;
        if(waitNextWaveCountDown <= 0)
        {
            CurrentWhaleState = WhaleState.Coming;
        }
    }

    IEnumerator WhaleAnimPlay(int frameDuration)
    {
        int frameCount = 0;
        SoundManager.Instance.PlaySound(sfx_Whale, this.transform.position);
        whaleAnim.SetBool("Jump",true);
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        whaleAnim.SetBool("Jump",false);
    }

    void OnStateEnter(WhaleState enterState)
    {
        switch (enterState)
        {
            case WhaleState.Wait: waitNextWaveCountDown = waitNextWaveDuration; break;
            case WhaleState.Coming:
                StartCoroutine(WhaleAnimPlay(whaleAnimFrame));
                comingCountDown = comingDuration; break;
            case WhaleState.Splash:
                FrenzySpawner.FrenzySpawnFish();
                CurrentWhaleState = WhaleState.Wait;
                break;
        }
    }

    void OnStateEnd(WhaleState endState)
    {
        switch (endState)
        {
            case WhaleState.Wait: break;
            case WhaleState.Coming: break;
            case WhaleState.Splash: break;
        }
    }


}
