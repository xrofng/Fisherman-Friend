using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialSpin : FishSpecialMelee {

    [Header("Spin")]
    public float speed = 10;
    protected float speedDet = 100;
    protected float _speed=0;
    public float Speed
    {
        get { if (_speed == 0) { _speed = speed / speedDet; } return _speed;  }
    }

    public float floorOffset;
    protected float floorRatio = 10;
    protected float playerPositionY;
    public int spiningFrameDuration = 60;
    public float fishSpinSpeed = 10;
    public PlayerAnimation.Anim spiningClip;

    [Header("SoundEffect")]
    public SoundEffect sfx_spining;

    protected override void ChangeToSpecialAnimation()
    {
        // inorder to change to looping animation
        // not call base.this preventing change to holdfish after finish 1st animation clip
        //Player._cPlayerAnimator.ChangeAnimState((int)specialClip, true, (int)spiningClip);
    }

    protected override void OnSpecialStart()
    {
        base.OnSpecialStart();
        Player.AddAbilityInputIntercepter(this);
        playerPositionY = Player.transform.position.y;
        Player._cPlayerAnimator.TriggerAnimation("s_startspin");
        Debug.Log("start melee");
    }

    protected override void OnSpecialEnd()
    {
        base.OnSpecialEnd();
        Player.transform.position = sClass.SetVector3(Player.transform.position, VectorComponent.y, playerPositionY);
        //Player._cPlayerAnimator.ChangeAnimState((int)Player._cPlayerAnimator.GetIdleAnimation());
        Player._cPlayerAnimator.TriggerAnimation("s_endspin");
        Player.RemoveAbilityInputIntercepter(this);
        fish.SnapTransform();
        StopSFX(sfx_spining);
        Debug.Log("end melee");
    }

    protected override void OnSpecialProcess()
    {
        base.OnSpecialProcess();
        PlaySFX(sfx_spining);
        Player.transform.Translate(Player.PlayerForward * Speed);
        Player.transform.position = sClass.SetVector3(Player.transform.position, VectorComponent.y, playerPositionY + floorOffset);
        fish.transform.Rotate(Vector3.forward * fishSpinSpeed, Space.Self);
        Debug.Log("process melee");
    }

    protected override int GetSpecialFrameDuration(bool includeLag = false)
    {
        int c = spiningFrameDuration + base.GetSpecialFrameDuration();
        if (includeLag)
        {
            c += InputLagFrameDuration;
        }
        Debug.Log("frame dura" + c);
        return c;
    }

    public override void OnDehydrate()
    {
        base.OnDehydrate();
        if (Player)
        {
            Player.RemoveAbilityInputIntercepter(this);
        }
    }

}
