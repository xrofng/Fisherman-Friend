using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlap : PlayerAbility {

    public HitBoxMelee hitBox;
    public Animation slapTrail;
    //public ParticleSystem slapParticle;
    protected bool attacking;

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
            slapTrail.gameObject.SetActive(value);

            hitBox.GetCollider<BoxCollider>().enabled = value;
            if (showHitBox)
            {
                hitBox.GetMeshRenderer().enabled = value;
            }
            slapTrail.Play();
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
    void SlapFish() {
        string slap = "Slap" + _player.playerID;
        if (_player.mainFish == null)
        {
            return;
        }
        if (_pInput.GetButtonDown(_pInput.Slap, _player.playerID - 1))
        {
            //Assign fish stat to hitbox
            //hitBox.center = _player.mainFish.hitboxCenter;
            //hitBox.size = _player.mainFish.hitboxSize;
            hitBox.InvincibilityFrame = _player.mainFish.s_invicibilityFrame;
            hitBox.DamageCaused = _player.mainFish.attack;
            hitBox._SFXclip = sfx_Slap;
            if (!Attacking)
            {
                RunParticle();
                StartCoroutine(HitBoxEnable(_player.mainFish.hitBoxStayFrame));
                ChangeAnimState(PlayerAnimation.State.H_Slap, frameAnimation, true , PlayerAnimation.State.HoldFish);
            }
        }
    }

    void RunParticle()
    {
        //if (slapParticle)
        //{
        //    slapParticle.Play();
        //}
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
