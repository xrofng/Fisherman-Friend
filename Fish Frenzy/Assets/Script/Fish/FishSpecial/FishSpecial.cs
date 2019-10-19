﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fish))]
public class FishSpecial : MonoBehaviour
{
    public enum FishSpecialActivatedState
    {
        DOWN,
        HOLD,
        UP
    }

    protected Fish fish;

    protected PlayerSpecial PlayerFishSpecial
    {
        get { return Player._cPlayerSpecial; }
    }
    protected Player Player
    {
        get { return fish.GetPlayerHolder; }
    }
    protected virtual void PlaySFX(SoundEffect SFXclip)
    {
        SoundManager.Instance.PlaySound(SFXclip, transform.position);
    }
    protected virtual void StopSFX(SoundEffect SFXclip)
    {
        SoundManager.Instance.StopSound(SFXclip);
    }
    [Header("Special")]
    public float attack;
    public PlayerAnimation.Anim specialClip;
    public int SpeiclaClipFrameCount
    {
        get { return Player.GetAnimator<PlayerAnimation>().AnimationFrame[(int)specialClip]; }
    }
    public int InputLagFrameDuration = 10;

    protected bool _isPerformingSpecial = false;
    public bool IsPerformingSpecial
    {
        set { _isPerformingSpecial = value; }
    }

    public Rigidbody _pRigid
    {
        get { return fish.Rigidbody; }
    }

    /// <summary>
    /// On Start(), we call the ability's intialization
    /// </summary>
    protected virtual void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Gets and stores components for further use
    /// </summary>
    protected virtual void Initialization()
    {
        fish = GetComponent<Fish>();
    }

    protected virtual void IgnoreInputFor(int ignoreFrame)
    {
        StartCoroutine(InvokeIgnoreInput(ignoreFrame));
    }

    IEnumerator InvokeIgnoreInput(int frameDuration)
    {
        int frameCount = 0;
        Player.AddAbilityInputIntercepter(this);
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        Player.RemoveAbilityInputIntercepter(this);
    }

    public void ActionForFrame(int frameDuration, System.Action begin, System.Action end)
    {
        StartCoroutine(ieActionForFrame(frameDuration, begin, end));
    }

    IEnumerator ieActionForFrame(int frameDuration, System.Action begin, System.Action end)
    {
        begin();
        int frameCount = 0;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        end();
    }

    public virtual void OnDehydrate()
    {
        if ( Player)
        {
            Player._cPlayerAnimator.ChangeAnimState((int)PlayerAnimation.Anim.Idle);
        }
    }

    public virtual void OnPlayerDeath()
    {
        if (Player)
        {
            Player.RemoveAbilityInputIntercepter(this);
        }
    }

    public virtual void OnPlayerHold()
    {

    }

    public virtual void OnSpecialActivated()
    {

    }

    public void TryPerformSpecial(FishSpecialActivatedState fishSpecialActivatedState)
    {
        if (CheckCanActivate(fishSpecialActivatedState))
        {
            PerformSpecial(fishSpecialActivatedState);
        }
    }

    private bool CheckCanActivate(FishSpecialActivatedState fishSpecialActivatedState)
    {
        if (fishSpecialActivatedState == FishSpecialActivatedState.DOWN)
        {
            return CheckCanActivateDown();
        }
        if (fishSpecialActivatedState == FishSpecialActivatedState.HOLD)
        {
            return CheckCanActivateHold();
        }
        if (fishSpecialActivatedState == FishSpecialActivatedState.UP)
        {
            return CheckCanActivateUp();
        }
        return false;
    }

    public virtual bool CheckPerformingSpecial()
    {
        return _isPerformingSpecial;
    }



    protected void PerformSpecial(FishSpecialActivatedState fishSpecialActivatedState)
    {
        if(fishSpecialActivatedState == FishSpecialActivatedState.DOWN)
        {
            PerformSpecialDown();
        }
        if (fishSpecialActivatedState == FishSpecialActivatedState.HOLD)
        {
            PerformSpecialHold();
        }
        if (fishSpecialActivatedState == FishSpecialActivatedState.UP)
        {
            PerformSpecialUp();
        }
    }

    protected virtual void PerformSpecialDown() { OnSpecialActivated(); }
    protected virtual void PerformSpecialHold() { }
    protected virtual void PerformSpecialUp() { }

    protected virtual bool CheckCanActivateDown() { return CheckValidForSpecial() && !CheckPerformingSpecial(); }
    protected virtual bool CheckCanActivateHold() { return CheckValidForSpecial() && !CheckPerformingSpecial(); }
    protected virtual bool CheckCanActivateUp() { return CheckValidForSpecial() && !CheckPerformingSpecial(); }

    /// <summary>
    /// validate checking for all 3 button state of Check Can activate
    /// not about performing special
    /// </summary>
    /// <returns></returns>
    protected virtual bool CheckValidForSpecial()
    {
        return true;
    }
}
