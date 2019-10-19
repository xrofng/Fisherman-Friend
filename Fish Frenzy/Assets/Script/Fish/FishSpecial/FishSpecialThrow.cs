using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialThrow: FishSpecialSpawn
{
    public override void OnSpecialActivated()
    {
        base.OnSpecialActivated();
        Player._cPlayerAnimator.ChangeAnimState((int)specialClip, SpeiclaClipFrameCount, true);
        StartCoroutine(ieSpecialThrowAttack(throwingFrameDuration, channelingFrameDuration));
    }

    protected IEnumerator ieSpecialThrowAttack(int frameDuration, int channelFrameDuration)
    {
        _isPerformingSpecial = true;
        IgnoreInputFor(channelFrameDuration);
        int frameCount = 0;
        while (frameCount < channelFrameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        
        OnThrowStart();

        while (!currentMovingObj.MoveEnd)
        {
            yield return new WaitForEndOfFrame();
        }
        _isPerformingSpecial = false;
        OnThrowEnd();
    }

    protected virtual void OnThrowStart()
    {
        currentMovingObj = Instantiate(movingObjects, this.transform.position, Quaternion.identity);// add pos set lyr
        currentMovingObj.gameObject.layer = LayerMask.NameToLayer("Fish" + Player.playerID);
        currentMovingObj.HitBox.Owner = Player.gameObject;
        currentMovingObj.direction = currentMovingObj.HitBox.OwnerPlayer.GetPart(Player.ePart.body).transform.TransformDirection(-Vector3.forward);
        currentMovingObj.transform.LookAt(currentMovingObj.direction + transform.position);
        fish.MeshRenderer.enabled = false;
    }

    protected virtual void OnThrowEnd()
    {
        fish.MeshRenderer.enabled = true;
        fish.SnapTransform();
    }

    IEnumerator ieIgnoreInput(int frameDuration)
    {
        int frameCount = 0;
        Player.AddAbilityInputIntercepter(this);
        if (currentMovingObj != null)
        {
            while (frameCount < frameDuration && !currentMovingObj.MoveEnd)
            {
                yield return new WaitForEndOfFrame();
                frameCount++;
            }
        }
        else
        {
            while (frameCount < frameDuration)
            {
                yield return new WaitForEndOfFrame();
                frameCount++;
            }
        }
       
        Player.RemoveAbilityInputIntercepter(this);
    }

    protected override void PerformSpecialDown()
    {
        PlayerFishSpecial.GetCrossZComponent<PlayerThrow>().OnButtonDown();
    }

    protected override void PerformSpecialHold()
    {
        PlayerFishSpecial.GetCrossZComponent<PlayerThrow>().OnButtonHold();
    }

    protected override void PerformSpecialUp()
    {
        OnSpecialActivated();
        PlayerFishSpecial.GetCrossZComponent<PlayerThrow>().ChangeToUnAim();
    }

    protected override bool CheckValidForSpecial()
    {
        bool valid = !PlayerFishSpecial.GetCrossZComponent<PlayerState>().IsJumping &&
            !SpawnedExist();
        return valid;
    }
}
