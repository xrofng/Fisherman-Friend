using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]

public class PlayerAbility : MonoBehaviour
{
    protected Player _player;

    // private ignore Input for specific ability
    protected bool ignoreInput;
    public bool IgnoreInput { get { return ignoreInput; } }

    public JoystickManager _pInput
    {
        get { return _player.LinkedInputManager; }

    }

    public Rigidbody _pRigid
    {
        get { return _player.rigid; }

    }
    public PlayerAnimation  _pAnimator
    {
        get { return _player.animator; }
    }
    public int frameAnimation;

    protected AudioSource _SFX;

    protected virtual void PlaySFX(AudioClip SFXclip)
    { 
        if (_SFX.isPlaying) { return; }
        _SFX.clip = SFXclip;
        _SFX.Play();
    }


    /// <summary>
    /// On Start(), we call the ability's intialization
    /// </summary>
    protected virtual void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Gets and stores components for further use
    /// </summary>
    protected virtual void Initialization()
    {
        _player = GetComponent<Player>();
        _SFX = GetComponent<AudioSource>();
    }




    public void ChangeAnimState(PlayerAnimation.State s, int ignoreFrame, bool revert)
    {
        StartCoroutine(InvokeChangeAnimState(s,ignoreFrame,revert));
    }


    IEnumerator InvokeChangeAnimState(PlayerAnimation.State s,int frameDuration,bool revert)
    {
        int frameCount = 0;
        _pAnimator.ChangeState((int)s);
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        if (revert) { _pAnimator.ChangeState((int)PlayerAnimation.State.Idle); }
    }

    public void IgnoreInputFor(int ignoreFrame)
    {
        StartCoroutine(InvokeIgnoreInput(ignoreFrame));
    }
    

    IEnumerator InvokeIgnoreInput(int frameDuration)
    {
        int frameCount = 0;
        ignoreInput = true;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        ignoreInput = false;
    }
    
    public T GetCrossZComponent<T>() where T : PlayerAbility
    {
        if (typeof(T) == typeof(PlayerMovement)) { return _player._cPlayerMovement as T; }
        if (typeof(T) == typeof(PlayerThrow)) { return _player._cPlayerThrow as T; }
        if (typeof(T) == typeof(PlayerFishing)) { return _player._cPlayerFishing as T; }
        if (typeof(T) == typeof(PlayerSlap)) { return _player._cPlayerSlap as T; }
        if (typeof(T) == typeof(PlayerSwitchFish)) { return _player._cPlayerSwitch as T; }
        if (typeof(T) == typeof(PlayerInvincibility)) { return _player._cPlayerInvincibility as T; }
        if (typeof(T) == typeof(PlayerState)) { return _player._cPlayerState as T; }
        if (typeof(T) == typeof(PlayerFishInteraction)) { return _player._cPlayerFishInteraction as T; }
        if (typeof(T) == typeof(PlayerFishSpecial)) { return _player._cPlayerFishSpecial as T; }

        return this as T;
    }
}
