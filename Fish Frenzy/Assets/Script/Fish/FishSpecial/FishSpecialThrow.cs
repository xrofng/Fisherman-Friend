using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialThrow: FishSpecialSpawn
{
    public string onStartChannelingTriggerName;
    public bool WaitForCurrentMovingObj = false;

    public bool HideFishOnThrow;

    public override void OnSpecialActivated()
    {
        base.OnSpecialActivated();
        Player._cPlayerAnimator.TriggerAnimation(onStartChannelingTriggerName);
        StartCoroutine(PerformThrow(InputLagFrameDuration,channelingFrameDuration));
    }

    protected IEnumerator PerformThrow(int frameDuration, int channelFrameDuration)
    {
        _isPerformingSpecial = true;
        // ignore input for channeling and inputlag
        IgnoreInputFor(channelFrameDuration + frameDuration);
        int frameCount = 0;
        while (frameCount < channelFrameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        
        OnThrowStart();

        while (WaitForCurrentMovingObj && !currentMovingObj.MoveEnd)
        {
            yield return new WaitForEndOfFrame();
        }
        _isPerformingSpecial = false;
        OnThrowEnd();
    }

    protected virtual void OnThrowStart()
    {
        SetFishVisibility(false);
        currentMovingObj = SpawnMovingObject(movingObjects,transform.position);
        currentMovingObj.Direction = currentMovingObj.HitBox.OwnerPlayer.GetPart(Player.ePart.body).transform.TransformDirection(-Vector3.forward);
        currentMovingObj.transform.LookAt(currentMovingObj.Direction + transform.position);
    }

    protected MovingObject SpawnMovingObject(MovingObject movingObjects,Vector3 pos)
    {
        MovingObject newMov = Instantiate(movingObjects, pos, Quaternion.identity);
        newMov.gameObject.layer = LayerMask.NameToLayer("Fish" + Player.playerID);
        newMov.HitBox.Owner = Player.gameObject;
        return newMov;
    }

    private void SetFishVisibility(bool isVisible)
    {
        if (!HideFishOnThrow)
        {
            return;
        }
        fish.MeshRenderer.enabled = isVisible;
    }

    protected virtual void OnThrowEnd()
    {
        SetFishVisibility(true);
        fish.SnapToHold();
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
