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

    public override void OnSpecialActivated()
    {
        base.OnSpecialActivated();

        playerPositionY = Player.transform.position.y;
        StartCoroutine(Spining());
    }

    protected override void ChangeToSpecialAnimation()
    {
        // inorder to change to looping animation
        // not call base.this preventing change to holdfish after finish 1st animation clip
    }

    IEnumerator Spining()
    {
        Player.AddAbilityInputIntercepter(this);
        int frameCount = 0;
        while (frameCount < spiningFrameDuration + SpeiclaClipFrameCount)
        {
            PlaySFX(sfx_spining);
            yield return new WaitForEndOfFrame();
            Player.transform.Translate(Player.PlayerForward * Speed);
            Player.transform.position = sClass.SetVector3(Player.transform.position, VectorComponent.y, playerPositionY + floorOffset);
            fish.transform.Rotate(Vector3.forward * fishSpinSpeed, Space.Self);
            frameCount += 1;
        }
        Player.transform.position = sClass.SetVector3(Player.transform.position, VectorComponent.y, playerPositionY);
        Player._cPlayerAnimator.ChangeAnimState((int)Player._cPlayerAnimator.GetIdleAnimation());
        Player.RemoveAbilityInputIntercepter(this);
        fish.SnapTransform();
        StopSFX(sfx_spining);
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
