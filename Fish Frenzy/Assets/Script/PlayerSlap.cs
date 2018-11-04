using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlap : PlayerAbility {

    public HitBoxMelee hitBox;
    //public ParticleSystem slapParticle;
    public float upMultiplier;
    protected bool attacking;

    [Header("SFX")]
    public AudioClip sfx_Slap;

    public bool Attacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
            hitBox.gameObject.SetActive(value);
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
        if (Input.GetButtonDown(slap) )
        {
            //Assign fish stat to hitbox
            //hitBox.center = _player.mainFish.hitboxCenter;
            //hitBox.size = _player.mainFish.hitboxSize;
            hitBox.InvincibilityFrame = _player.mainFish.s_invicibilityFrame;
            hitBox.DamageCaused = _player.mainFish.attack;
            if (!Attacking)
            {
                RunParticle();
                StartCoroutine(HitBoxEnable(_player.mainFish.hitBoxStayFrame));
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
        if (_player.mainFish.sfx_Throw)
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
