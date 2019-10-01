using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialMelee : FishSpecial
{
    protected HitBoxMelee damageHitbox;
    public string hitBoxName;
    public int invicibilityFrame = 50;
    public int freezeFrame = 10;
    public bool launchingDamage = true;

    [Header("Prefab Ref")]
    public Transform hitBoxRef;

    [Header("Sound Effect")]
    public SoundEffect sfx_startMelee;

    public override void OnPlayerHold()
    {
        base.OnPlayerHold();
        BindHitBox();
        SetUpGameVariable();
    }

    protected void SetUpSpecialHitBox()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    void SetUpGameVariable()
    {   
        damageHitbox.FreezeFramesOnHit = freezeFrame;
        damageHitbox.InvincibilityFrame = invicibilityFrame;
        damageHitbox.DamageCaused = attack;
        damageHitbox.isLauncher = launchingDamage;

        if (fish.sfx_Special.clip)
        {
            damageHitbox._SFX = fish.sfx_Special;
        }
        else
        {
            damageHitbox._SFX = PlayerFishSpecial.sfx_Special;
        }
    }

    private void BindHitBox()
    {
        damageHitbox = Player._cPlayerDamageHitBox.GetHitBox(hitBoxName);
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
    }
}
