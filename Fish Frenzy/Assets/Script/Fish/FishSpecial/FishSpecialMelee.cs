using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialMelee : FishSpecial {

    protected HitBoxMelee playerHitBox;
    public HitBoxMelee thisSpecialHitBox;
    public int invicibilityFrame = 50;
    public int freezeFrame = 10;
    public bool launchingDamage = true;

    [Header("Prefab Ref")]
    public Transform hitBoxRef;

    [Header("Sound Effect")]
    public SoundEffect sfx_startMelee;

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
    }

    public override void SetUpFishSpecial()
    {
        base.SetUpFishSpecial();
        //SetUpSpecialHitBox();
        SetUpGameVariable();
    }

    //protected void SetUpSpecialHitBox()
    //{
    //    foreach (Transform hb in fish.GetPlayerHolder.GetPart(Player.ePart.hitBox))
    //    {
    //        if(hb.gameObject.name == thisSpecialHitBox.gameObject.name)
    //        {
    //            _playerFishSpecial.specialHitBox = hb.GetComponent<HitBoxMelee>();
    //        }
    //    }
    //    if(_playerFishSpecial.specialHitBox == null) { Debug.LogError("Can't find hitbox"); }

    //    _playerFishSpecial.specialHitBox.transform.SetParent(_playerFishSpecial.hitBoxParent);
    //    _playerFishSpecial.specialHitBox.isLauncher = launchingDamage;
    //}

    /// <summary>
    /// 
    /// </summary>
    void SetUpGameVariable()
    {
        //_playerFishSpecial.specialHitBox.gameObject.layer = LayerMask.NameToLayer("Fish" + fish.GetPlayerHolder.playerID);
        //playerHitBox = _playerFishSpecial.specialHitBox;
        playerHitBox.FreezeFramesOnHit = freezeFrame;
        playerHitBox.InvincibilityFrame = invicibilityFrame;
        playerHitBox.DamageCaused = attack;
        if (fish.sfx_Special.clip)
        {
            playerHitBox._SFX = fish.sfx_Special;
        }
        else
        {
            playerHitBox._SFX = PlayerFishSpecial.sfx_Special;
        }
    }

    public override void OnSpecialActivated()
    {
        base.OnSpecialActivated();
        PlaySFX(sfx_startMelee);
        ActionForFrame(SpeiclaClipFrameCount + IgnoreInputFrameDuration,
                 () => { IsPerformingSpecial = true; },
                 () => { IsPerformingSpecial = false; });

        Player._cPlayerAnimator.ChangeAnimState((int)specialClip, SpeiclaClipFrameCount, true);
    }

    protected override void PerformSpecialDown()
    {
        base.PerformSpecialDown();
        OnSpecialActivated();
    }
}
