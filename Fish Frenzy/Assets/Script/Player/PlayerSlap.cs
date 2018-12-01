using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlap : PlayerAbility
{
    public HitBoxMelee hitBox;
    public Animation slapTrail;
    //public ParticleSystem slapParticle;
    protected bool attacking;
    public int ignoreSlapFrame = 4;

    [Header("SFX")]
    public AudioClip sfx_Slap;

    [Header("Debug")]
    public bool showHitBox;

    public bool Attacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
        }
    }

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        hitBox.gameObject.layer = LayerMask.NameToLayer("Fish" + _player.playerID);

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
            SlapFish();
        }
    }

    // Update is called once per frame
    void SlapFish()
    {
        if (_player.mainFish == null)
        {
            return;
        }
        if (_pInput.GetButtonDown(_pInput.Special, _player.playerID - 1))
        {
            //hitBox.InvincibilityFrame = _player.mainFish.s_invicibilityFrame;
            //hitBox.DamageCaused = _player.mainFish.attack;
            //hitBox.isLauncher = _player.mainFish.s_launchingDamage;
            //hitBox._SFXclip = sfx_Slap;
            //if (!Attacking)
            //{
            //    // Ignore Input
            //    ActionForFrame(_player.mainFish.SlapClipFrameCount + ignoreSlapFrame,
            //          () => { attacking = true;  },
            //          () => { attacking = false; });

            //    // enable trail
            //    ActionForFrame(_player.mainFish.SlapClipFrameCount,
            //          () => { slapTrail.gameObject.SetActive(true); slapTrail.Play(); },
            //          () => { slapTrail.gameObject.SetActive(false); slapTrail.Stop(); } );

            //    _pAnimator.ChangeAnimState((int)_player.mainFish.slapClip, _player.mainFish.SlapClipFrameCount, true, (int)PlayerAnimation.State.HoldFish);
            //} else
            //{
            //    print(Attacking);
            //}
        }
    }

    public void PlaySlapSFX()
    {
        if (_player.mainFish.sfx_Slap)
        {
            PlaySFX(_player.mainFish.sfx_Slap);
        }
        else
        {
            PlaySFX(sfx_Slap);
        }
    }

    IEnumerator HitBoxEnable(int frameDuration)
    {
        Attacking = true;
        int frameCount = 0;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        Attacking = false;
    }

   
}