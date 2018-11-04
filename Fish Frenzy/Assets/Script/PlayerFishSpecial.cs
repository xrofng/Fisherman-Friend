using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishSpecial : PlayerAbility
{
    public enum SpecialType
    {
        Melee = 0,
        Range,
        Other
    }
    public SpecialType type;


    public HitBoxMelee specialHitBox;
    public Animation specialTrail;
    protected bool attacking;

    [Header("SFX")]
    public AudioClip sfx_Special;

    public bool Attacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
            specialTrail.gameObject.SetActive(value);
            specialHitBox.GetCollider<MeshCollider>().enabled = value;  
            specialTrail.Play();
        }
    }

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        specialHitBox.gameObject.layer = LayerMask.NameToLayer("Fish" + _player.playerID);

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
        if (_player.mainFish == null)
        {
            return;
        }
        if (Input.GetButtonDown(special))
        { 
            specialHitBox.InvincibilityFrame = _player.mainFish.s_invicibilityFrame;
            specialHitBox.DamageCaused = _player.mainFish.attack;
            if (!Attacking)
            {
                StartCoroutine(HitBoxEnable(_player.mainFish.hitBoxStayFrame));
            }
        }
    }

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
