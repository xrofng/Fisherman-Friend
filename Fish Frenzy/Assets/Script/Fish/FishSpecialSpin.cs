using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialSpin : FishSpecialMelee {

    [Header("Spin")]
    public float speed = 10;
    public float floorOffset;
    protected Vector3 playerColliderSize;
    public int spiningFrameDuration = 60;
    public float fishSpinSpeed = 10;
    public SpecialAnimation spiningClip;

    public override void SetUpFishSpecial()
    {
        base.SetUpFishSpecial();
        playerColliderSize = _player.GetCollider<BoxCollider>().size;
    }

    public override void SpecialMeleeAttack(Player _player)
    {
        // dont call base.SpecialMeleeAttack to not change to holdfish after finish 1st animation clip
        _player.animator.ChangeAnimState((int)specialClip, SpeiclaClipFrameCount, true, (int)PlayerAnimation.State.Spinning);
        _player.Rigidbody.AddForce(_player.playerForward * speed, ForceMode.Impulse);
        StartCoroutine(Spining());
    }

    IEnumerator Spining()
    {
        playerColliderSize = playerColliderSize + Vector3.up* floorOffset;
        _player.AddAbilityInputIntercepter(this);
        int frameCount = 0;
        while (frameCount < spiningFrameDuration + SpeiclaClipFrameCount)
        {
            yield return new WaitForEndOfFrame();
            _fish.transform.Rotate(Vector3.forward * fishSpinSpeed, Space.Self);
            frameCount += 1;
        }
        _player.animator.ChangeAnimState((int)PlayerAnimation.State.HoldFish);
        _player.RemoveAbilityInputIntercepter(this);
        _fish.SnapTransform();
        playerColliderSize = playerColliderSize - Vector3.up * floorOffset;
    }


    public override void OnDehydrate()
    {
        base.OnDehydrate();
       
    }

}
