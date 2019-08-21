﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fish))]
public class FishSpecial : MonoBehaviour
{
    protected Fish _fish;

    protected PlayerSpecial _playerFishSpecial
    {
        get { return _player._cPlayerSpecial; }
    }
    protected Player _player
    {
        get { return _fish.GetPlayerHolder; }
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
        get { return _player.GetAnimator<PlayerAnimation>().AnimationFrame[(int)specialClip]; }
    }
    public int IgnoreInputFrameDuration
    {
        get;
        set;
    }

    public Rigidbody _pRigid
    {
        get { return _fish.Rigidbody; }
    }

    /// <summary>
    /// On Start(), we call the ability's intialization
    /// </summary>
    protected virtual void Start()
    {
        Initialization();
    }
    protected virtual void Update()
    {

    }

    /// <summary>
    /// Gets and stores components for further use
    /// </summary>
    protected virtual void Initialization()
    {
        _fish = GetComponent<Fish>();
    }

    protected virtual void IgnoreInputFor(int ignoreFrame)
    {
        StartCoroutine(InvokeIgnoreInput(ignoreFrame));
    }

    IEnumerator InvokeIgnoreInput(int frameDuration)
    {
        int frameCount = 0;
        _player.AddAbilityInputIntercepter(this);
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        _player.RemoveAbilityInputIntercepter(this);
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
        if ( _player)
        {
            _player._cPlayerAnimator.ChangeAnimState((int)PlayerAnimation.Anim.Idle);
        }
    }

    public virtual void OnPlayerDeath()
    {
        if (_player)
        {
            _player.RemoveAbilityInputIntercepter(this);
        }
    }

    public virtual void OnSpecialActivated()
    {

    }

    public virtual bool GetSpecialing()
    {
        return false;
    }
}
