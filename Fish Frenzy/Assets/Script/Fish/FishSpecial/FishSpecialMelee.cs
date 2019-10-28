using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialMelee : FishSpecial
{
    protected HitBoxMelee damageHitbox;
    public bool freezeRotation = true;
    public bool hasInvincibility = false;
    public string onStartTriggerName = "";

    [Header("Sound Effect")]
    public SoundEffect sfx_startMelee;

    public override void OnPlayerHold()
    {
        base.OnPlayerHold();
        BindHitBox();
        if (fish.sfx_Special.clip)
        {
            damageHitbox.HitSFX = fish.sfx_Special;
        }
        else
        {
            damageHitbox.HitSFX = PlayerFishSpecial.sfx_Special;
        }
        damageHitbox.SetDamage(damage);
    }

    private void BindHitBox()
    {
        damageHitbox = Player._cPlayerDamageHitBox.GetHitBox(damage.hitBoxName);
    }

    protected override void OnSpecialStart()
    {
        Player._cPlayerAnimator.TriggerAnimation(onStartTriggerName);
        SpecialStart();
        if (hasInvincibility)
        {
            Player._cPlayerInvincibility.IsInvincible = true;
        }
        if (freezeRotation)
        {
            Player._cPlayerMovement.FreezeRotation = true;
        }
    }

    protected override void OnSpecialEnd()
    {
        base.OnSpecialEnd();
        SpecialEnd();
    }

    public override void OnSpecialActivated()
    {
        base.OnSpecialActivated();
        PlaySFX(sfx_startMelee);

        ActionForFrame(GetSpecialFrameDuration(),
                 // before acitvate
                 () => {
                     OnSpecialStart();
                 },
                 // middle acitvate
                 () => {
                     OnSpecialProcess();
                 },
                 // after acitvate
                 () => {
                     OnSpecialEnd();
                 });

        ActionForFrame(GetSpecialFrameDuration(true),
            // before acitvate
                 () => {
                     IsPerformingSpecial = true;
                     OnControlSteal();
                 },
            // middle acitvate
                 () => {
                 },
            // after acitvate
                 () => {
                     IsPerformingSpecial = false;
                     OnControlGive();
                 });

        //ChangeToSpecialAnimation();
    }

    protected virtual void OnControlSteal()
    {
    }

    protected virtual void OnControlGive()
    {
    }

    protected override bool CheckValidForSpecial()
    {
        return !Player.Aiming;
    }

    public override void SpecialEnd()
    {
        base.SpecialEnd();
        if (!Player)
        {
            return;
        }
        if (hasInvincibility)
        {
            Player._cPlayerInvincibility.IsInvincible = false;
        }
        if (freezeRotation)
        {
            Player._cPlayerMovement.FreezeRotation = false;
        }
    }
}
