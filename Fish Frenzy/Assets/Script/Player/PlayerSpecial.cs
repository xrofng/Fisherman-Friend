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
            if (_player.IgnoreInputForAbilities || IgnoreInput)
            {
                return;
            }
            SpecialFish();
        }
    }

    void SpecialFish()
    {
        string special = "Fishing" + _player.playerID;
        if (_player.mainFish == null)
        {
            return;
        }

        if(_player.mainFish.state == Fish.fState.dehydrate || !_player.mainFish.GetComponent<FishSpecial>())
        {
            return;
        }

        SpecialMelee(special);
        SpecialThrow(special);
    }

    [Header("Melee")]
    public Transform hitBoxParent;
    public HitBoxMelee specialHitBox;
    public Animation specialTrail;

    void SpecialMelee(string special)
    {
        if (!_player.mainFish.GetComponent<FishSpecialMelee>() || _player.mainFish.GetComponent<FishSpecialMelee>().MeleeSpecialing)
        {
            return;
        }

        if (_pInput.GetButtonDown(_pInput.Special, _player.playerID - 1))
        {
            _player.mainFish._cSpecial.IgnoreInputFrameDuration = ignoreSpecialFrame;
            _player.mainFish._cSpecial.OnSpecialActivated();
            // Ignore Input
           
            // enable trail
            //ActionForFrame(FishSpecial<FishSpecialMelee>().SpeiclaClipFrameCount,
            //      () => { specialTrail.gameObject.SetActive(true); specialTrail.Play(); },
            //      () => { specialTrail.gameObject.SetActive(false); specialTrail.Stop(); });
        }
    }

    //[Header("Throw")]
    void SpecialThrow(string special)
    {
        if (!_player.mainFish.GetComponent<FishSpecialThrow>() || _player.mainFish.GetComponent<FishSpecialThrow>().ThrowSpecialing)
        {
            return;
        }
        if (GetCrossZComponent<PlayerState>().IsJumping)
        {
            return;
        }

        if (_pInput.GetButtonDown(_pInput.Special, _player.playerID - 1))
        {
            GetCrossZComponent<PlayerThrow>().OnButtonDown();
        }
        else if (_pInput.GetButton(_pInput.Special, _player.playerID - 1))
        {
            GetCrossZComponent<PlayerThrow>().OnButtonHold();
        }
        else if (_pInput.GetButtonUp(_pInput.Special, _player.playerID - 1))
        {
            _player.mainFish._cSpecial.OnSpecialActivated();
            GetCrossZComponent<PlayerThrow>().ChangeToUnAim();
        }        
    }
    
    [Header("SFX")]
    public SoundEffect sfx_Special;

    [Header("Debug")]
    public bool showHitBox;
}
