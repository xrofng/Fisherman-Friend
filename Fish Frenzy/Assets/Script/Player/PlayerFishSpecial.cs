using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishSpecial : PlayerAbility
{
    [Header("Melee")]
    public Transform hitBoxParent;
    public HitBoxMelee specialHitBox;
    public Animation specialTrail;
    protected bool attacking;

    [Header("SFX")]
    public AudioClip sfx_Special;

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
            specialHitBox.GetCollider<MeshCollider>().enabled = value;  
            specialTrail.Play();
            specialTrail.gameObject.SetActive(value);

            if (showHitBox)
            {
                specialHitBox.GetMeshRenderer().enabled = value;
            }
        }
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
        if (Input.GetButtonDown(special))
        {
            if (_player.mainFish.GetComponent<FishSpecialMelee>())
            {
                if (!Attacking)
                {
                    StartCoroutine(HitBoxEnable(_player.mainFish.hitBoxStayFrame));
                }
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
