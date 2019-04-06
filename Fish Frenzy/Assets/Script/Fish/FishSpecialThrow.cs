using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialThrow : FishSpecial {

    [Header("Throw Seetting")]
    public MovingObject movingobject;
    protected MovingObject currentMovingObj;

    protected bool tSpecialing;
    public bool ThrowSpecialing
    {
        get { return tSpecialing; }
        set { tSpecialing = value; }
    }

    public int throwFrameDuration = 10;
    public int invicibilityFrame = 50;
    public int freezeFrame = 0;
    public bool launchingDamage = true;


    [Header("Channeling")]
    public int channelingFrameDuration = 10;
    public bool ignoreInput;


    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
    }

    public void SetUpFishSpecial()
    {
        SetUpGameVariable();
    }

    /// <summary>
    /// 
    /// </summary>
    void SetUpGameVariable()
    {
        movingobject.HitBox.FreezeFramesOnHit = freezeFrame;
        movingobject.HitBox.InvincibilityFrame = invicibilityFrame;
        movingobject.HitBox.DamageCaused = attack;
        if (_fish.sfx_Special.clip)
        {
            movingobject.HitBox._SFX = _fish.sfx_Special;
        }
        else
        {
            movingobject.HitBox._SFX = _playerFishSpecial.sfx_Special;
        }
    }

    protected override void Update()
    {

    }

    public virtual void SpecialThrowAttack(Player _player)
    {
        _player._cPlayerAnimator.ChangeAnimState((int)specialClip, SpeiclaClipFrameCount, true);
        StartCoroutine(ieSpecialThrowAttack(throwFrameDuration, channelingFrameDuration));
    }

    IEnumerator ieSpecialThrowAttack(int frameDuration, int channelFrameDuration)
    {
        ThrowSpecialing = true;

        IgnoreInputFor(channelFrameDuration);
        int frameCount = 0;
        while (frameCount < channelFrameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        
        OnThrowStart();

        IgnoreInputFor(frameDuration);
        frameCount = 0;
        while (frameCount < frameDuration && !currentMovingObj.MoveEnd)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }

        ThrowSpecialing = false;
        OnThrowEnd();
    }

    protected virtual void OnThrowStart()
    {
        currentMovingObj = Instantiate(movingobject, this.transform.position, Quaternion.identity);// add pos set lyr
        currentMovingObj.gameObject.layer = LayerMask.NameToLayer("Fish" + _player.playerID);
        currentMovingObj.HitBox.Owner = _player.gameObject;
        //currentMovingObj.HitBox._SFX = _playerFishSpecial.sfx_Special;
        currentMovingObj.direction = currentMovingObj.HitBox.OwnerPlayer.GetPart(Player.ePart.body).transform.TransformDirection(-Vector3.forward);
        currentMovingObj.transform.LookAt(currentMovingObj.direction + transform.position);
        _fish.MeshRenderer.enabled = false;
    }

    protected virtual void OnThrowEnd()
    {
        _fish.MeshRenderer.enabled = true;
        _fish.SnapTransform();
    }

    protected override void IgnoreInputFor(int ignoreFrame)
    {
        if (!ignoreInput)
        {
            return;
        }
        StartCoroutine(InvokeIgnoreInput(ignoreFrame));
    }

    IEnumerator InvokeIgnoreInput(int frameDuration)
    {
        int frameCount = 0;
        _player.AddAbilityInputIntercepter(this);
        if (currentMovingObj != null)
        {
            while (frameCount < frameDuration && !currentMovingObj.MoveEnd)
            {
                yield return new WaitForEndOfFrame();
                frameCount++;
            }
        }else
        {
            while (frameCount < frameDuration)
            {
                yield return new WaitForEndOfFrame();
                frameCount++;
            }
        }
       
        _player.RemoveAbilityInputIntercepter(this);
    }
}
