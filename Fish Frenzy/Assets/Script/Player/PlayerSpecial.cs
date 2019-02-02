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

    // Update is called once per frame
    void SpecialFish()
    {
        string special = "Fishing" + _player.playerID;
        if (_player.mainFish == null || !_player.mainFish.GetComponent<FishSpecial>())
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
    public bool MeleeSpecialing
    {
        get
        {
            if (_player.mainFish && _player.mainFish.GetComponent<FishSpecialMelee>())
            {
                return _player.mainFish.GetComponent<FishSpecialMelee>().MeleeSpecialing;
            }
            return false;
        }
    }
    public bool ThrowSpecialing
    {
        get
        {
            if (_player.mainFish && _player.mainFish.GetComponent<FishSpecialThrow>())
            {
                return _player.mainFish.GetComponent<FishSpecialThrow>().ThrowSpecialing;
            }
            return false;
        }
    }
    public bool Specialing
    {
        get
        {
            return  MeleeSpecialing || ThrowSpecialing;
        }
    }

    void SpecialMelee(string special)
    {
        if (!_player.mainFish.GetComponent<FishSpecialMelee>() || _player.mainFish.GetComponent<FishSpecialMelee>().MeleeSpecialing)
        {
            return;
        }

        if (_pInput.GetButtonDown(_pInput.Special, _player.playerID - 1))
        {
            // Ignore Input
            ActionForFrame(FishSpecial<FishSpecialMelee>().SpeiclaClipFrameCount + ignoreSpecialFrame,
                  () => { _player.mainFish.GetComponent<FishSpecialMelee>().MeleeSpecialing = true;  },
                  () => { _player.mainFish.GetComponent<FishSpecialMelee>().MeleeSpecialing = false;  });
            // enable trail
            //ActionForFrame(FishSpecial<FishSpecialMelee>().SpeiclaClipFrameCount,
            //      () => { specialTrail.gameObject.SetActive(true); specialTrail.Play(); },
            //      () => { specialTrail.gameObject.SetActive(false); specialTrail.Stop(); });

            _player.mainFish.GetComponent<FishSpecialMelee>().SpecialMeleeAttack(_player);

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
            //PlayThrowSFX();
            _player.mainFish.GetComponent<FishSpecialThrow>().SpecialThrowAttack(_player);
            GetCrossZComponent<PlayerThrow>().ChangeToUnAim();
        }        
    }
    
    [Header("SFX")]
    public AudioClip sfx_Special;
    public void PlaySlapSFX()
    {
        if (_player.mainFish.sfx_Special)
        {
            PlaySFX(_player.mainFish.sfx_Special);
        }
        else
        {
            PlaySFX(sfx_Special);
        }
    }

    [Header("Debug")]
    public bool showHitBox;
}
