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

    public override void SetUpFishSpecial()
    {
        base.SetUpFishSpecial();
    }

    public override void SpecialMeleeAttack(Player _player)
    {
        // dont call base.SpecialMeleeAttack to not change to holdfish after finish 1st animation clip
        _player._cPlayerAnimator.ChangeAnimState((int)specialClip, SpeiclaClipFrameCount, true, (int)PlayerAnimation.Anim.Spinning);
        // _player.Rigidbody.AddForce(, ForceMode.Impulse);
        playerPositionY = _player.transform.position.y;
         StartCoroutine(Spining());
    }

    IEnumerator Spining()
    {
//        _player.GetCollider<BoxCollider>().size = playerColliderSize + Vector3.up* floorOffset/ floorRatio;
        
        _player.AddAbilityInputIntercepter(this);
        int frameCount = 0;
        while (frameCount < spiningFrameDuration + SpeiclaClipFrameCount)
        {
            yield return new WaitForEndOfFrame();
            _player.transform.Translate(_player.playerForward * Speed);
            _player.transform.position = sClass.setVector3(_player.transform.position, sClass.vectorComponent.y, playerPositionY + floorOffset);
            _fish.transform.Rotate(Vector3.forward * fishSpinSpeed, Space.Self);
            frameCount += 1;
        }
        _player.transform.position = sClass.setVector3(_player.transform.position, sClass.vectorComponent.y, playerPositionY);
        _player._cPlayerAnimator.ChangeAnimState((int)_player._cPlayerAnimator.GetIdleAnimation());
        _player.RemoveAbilityInputIntercepter(this);
        _fish.SnapTransform();
    }


    public override void OnDehydrate()
    {
        base.OnDehydrate();
        if (_player)
        {
            _player.RemoveAbilityInputIntercepter(this);
        }
    }

}
