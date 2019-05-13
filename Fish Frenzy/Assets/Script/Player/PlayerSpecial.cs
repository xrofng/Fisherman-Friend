using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecial : PlayerAbility
{
    public int ignoreSpecialFrame = 4;
    
    public T FishSpecial<T>() where T : FishSpecial
    {
        return _player.mainFish._cSpecial as T;
    }

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        inputName = _pInput.Special;
    }

    /// <summary>
    /// is player performing any fish special ability
    /// </summary>
    /// <returns></returns>
    public bool GetSpecialing()
    {
        if (_player.mainFish)
        {
            return _player.mainFish._cSpecial.GetSpecialing();
        }
        return false;
    }

    public bool GetSpecialing<T>() where T : FishSpecial
    {
        if (!_player.mainFish)
        {
            return false;
        }
        T specialComponent = _player.mainFish.GetComponent<T>();
        if (specialComponent != null)
        {
            specialComponent.GetSpecialing();
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.state == Player.eState.ground)
        {
            HandleInput();
        }
    }

    bool IsValidForSpecial()
    {
        if (_player.mainFish == null)
        {
            return false;
        }

        if (_player.mainFish.state == Fish.fState.dehydrate || !_player.mainFish.GetComponent<FishSpecial>())
        {
            return false;
        }
        return true;
    }

    bool IsValidForSpecialThrow()
    {
        return _player.mainFish.GetComponent<FishSpecialThrow>()
            && !_player.mainFish.GetComponent<FishSpecialThrow>().ThrowSpecialing
            && !GetCrossZComponent<PlayerState>().IsJumping;
    }

    bool IsValidForSpecialMelee()
    {
        return _player.mainFish.GetComponent<FishSpecialMelee>() && !_player.mainFish.GetComponent<FishSpecialMelee>().MeleeSpecialing;
    }

    protected override void OnInputDown()
    {
        base.OnInputDown();
        if (!IsValidForSpecial())
        {
            return;
        }

        if (IsValidForSpecialMelee())
        {
            SpecialMelee();
        }
        
        if (IsValidForSpecialThrow())
        {
            GetCrossZComponent<PlayerThrow>().OnButtonDown();
        }
    }

    protected override void OnInputHold()
    {
        base.OnInputHold();
        if (!IsValidForSpecial())
        {
            return;
        }

        if (IsValidForSpecialThrow())
        {
            GetCrossZComponent<PlayerThrow>().OnButtonHold();
        }
    }

    protected override void OnInputUp()
    {
        base.OnInputUp();
        if (!IsValidForSpecial())
        {
            return;
        }

        if (IsValidForSpecialThrow())
        {
            _player.mainFish._cSpecial.OnSpecialActivated();
            GetCrossZComponent<PlayerThrow>().ChangeToUnAim();
        }
    }


    [Header("Melee")]
    public Transform hitBoxParent;
    public HitBoxMelee specialHitBox;
    public Animation specialTrail;

    void SpecialMelee()
    {
        _player.mainFish._cSpecial.IgnoreInputFrameDuration = ignoreSpecialFrame;
        _player.mainFish._cSpecial.OnSpecialActivated();
        // Ignore Input
           
        // enable trail
        //ActionForFrame(FishSpecial<FishSpecialMelee>().SpeiclaClipFrameCount,
        //      () => { specialTrail.gameObject.SetActive(true); specialTrail.Play(); },
        //      () => { specialTrail.gameObject.SetActive(false); specialTrail.Stop(); });
    }

    [Header("SFX")]
    public SoundEffect sfx_Special;

    [Header("Debug")]
    public bool showHitBox;
}
