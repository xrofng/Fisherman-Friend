using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialHook : FishSpecialThrow {

    [Header("Hook")]
    public int damageFrameDuration = 10;
    public int releaseFrameDuration = 10;
    public int finalBlowForce = 1;
    protected Player hookedPlayer;
    private MovingObjHook _hookObject;

    [Header("SoundEffect")]
    public SoundEffect sfx_attach;
    public SoundEffect sfx_swing;

    public PlayerAnimation.Anim hookSlapClip;

    protected override void OnThrowStart()
    {
        base.OnThrowStart();
        PlaySFX(sfx_swing);
        _hookObject = currentMovingObj as MovingObjHook;
        _hookObject.sfx_attach = sfx_attach;
    }

    protected override void OnThrowEnd()
    {
        hookedPlayer = currentMovingObj.GetComponent<MovingObjHook>().HookedPlayer;
        base.OnThrowEnd();
        if (hookedPlayer != null)
        {
            StartCoroutine(FinalBlow());
        }else
        {
            Player._cPlayerAnimator.ChangeAnimState((int)Player._cPlayerAnimator.GetIdleAnimation());
        }
        DestroyMovingObject(currentMovingObj);
    }

    IEnumerator FinalBlow()
    {
        Player.AddAbilityInputIntercepter(this);

        hookedPlayer.AddAbilityInputIntercepter(this);

        int specialClip = (int)hookSlapClip;
        PlayerFishSpecial._pAnimator.ChangeAnimState(specialClip, damageFrameDuration, true);

        int frameCount = 0;
        while (frameCount < damageFrameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount += 1;
        }

        PlaySFX(fish.sfx_Special);
        hookedPlayer.recieveDamage(damage.damage, Player.gameObject, hookedPlayer.transform.position + Vector3.up, 
            damage.invicibilityFrame, damage.isLauncher, finalBlowForce);

        frameCount = 0;
        while (frameCount < releaseFrameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount += 1;
        }
        ReleaseHook();
    }

    public void ReleaseHook()
    {
        if (hookedPlayer)
        {
            hookedPlayer.RemoveAbilityInputIntercepter(this);
            hookedPlayer._cPlayerFishInteraction.SetPlayerCollideEverything(true);
        }
        if (Player)
        {
            Player.RemoveAbilityInputIntercepter(this);
            Player._cPlayerAnimator.ChangeAnimState((int)Player._cPlayerAnimator.GetIdleAnimation());
        }
    }

    public override void OnDehydrate()
    {
        base.OnDehydrate();
        ReleaseHook();
    }

}
