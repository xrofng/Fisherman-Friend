using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialHook : FishSpecialThrow {

    [Header("Hook")]
    public int damageFrameDuration = 10;
    public int releaseFrameDuration = 10;
    public int finalBlowForce = 1;
    protected Player hookedPlayer;

    public PlayerAnimation.Anim hookSlapClip;

    protected override void OnThrowStart()
    {
        base.OnThrowStart();
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
            _player._cPlayerAnimator.ChangeAnimState((int)_player._cPlayerAnimator.GetIdleAnimation());
        }

        Destroy(currentMovingObj.gameObject);
    }

    IEnumerator FinalBlow()
    {
        _player.AddAbilityInputIntercepter(this);

        hookedPlayer.AddAbilityInputIntercepter(this);

        int specialClip = (int)hookSlapClip;
        _playerFishSpecial._pAnimator.ChangeAnimState(specialClip, damageFrameDuration, true);

        int frameCount = 0;
        while (frameCount < damageFrameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount += 1;
        }

        PlaySFX(_fish.sfx_Special);
        hookedPlayer.recieveDamage(attack, _player.gameObject, hookedPlayer.transform.position + Vector3.up, invicibilityFrame, true , finalBlowForce);

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
        if (_player)
        {
            _player.RemoveAbilityInputIntercepter(this);
        }
    }

    public override void OnDehydrate()
    {
        base.OnDehydrate();
        ReleaseHook();
    }

}
