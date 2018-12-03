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
    protected bool mSpecialing;
    public bool MeleeSpecialing
    {
        get { return mSpecialing; }
        set { mSpecialing = value; }
    }
    void SpecialMelee(string special)
    {
        if (!_player.mainFish.GetComponent<FishSpecialMelee>() || MeleeSpecialing)
        {
            return;
        }

        if (_pInput.GetButtonDown(_pInput.Special, _player.playerID - 1))
        {
            // Ignore Input
            ActionForFrame(FishSpecial<FishSpecialMelee>().SpeiclaClipFrameCount + ignoreSpecialFrame,
                  () => { MeleeSpecialing = true;  },
                  () => { MeleeSpecialing = false;  });
            // enable trail
            //ActionForFrame(FishSpecial<FishSpecialMelee>().SpeiclaClipFrameCount,
            //      () => { specialTrail.gameObject.SetActive(true); specialTrail.Play(); },
            //      () => { specialTrail.gameObject.SetActive(false); specialTrail.Stop(); });

            int specialClip = (int)FishSpecial<FishSpecial>().specialClip;
            _pAnimator.ChangeAnimState(specialClip, FishSpecial<FishSpecial>().SpeiclaClipFrameCount, true, (int)PlayerAnimation.State.HoldFish);             
        }   
    }

    //[Header("Throw")]
    void SpecialThrow(string special)
    {
        if (!_player.mainFish.GetComponent<FishSpecialThrow>() || _player.mainFish.GetComponent<FishSpecialThrow>().ThrowSpecialing)
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
            _player.mainFish.GetComponent<FishSpecialThrow>().SpecialThrowAttack();
            GetCrossZComponent<PlayerThrow>().ChangeToUnAim();

            int specialClip = (int)FishSpecial<FishSpecial>().specialClip;
            _pAnimator.ChangeAnimState(specialClip, FishSpecial<FishSpecial>().SpeiclaClipFrameCount, true, (int)PlayerAnimation.State.HoldFish);
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
