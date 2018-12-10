using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fish))]
public class FishSpecial : MonoBehaviour
{
    protected Fish _fish;

    protected PlayerSpecial _playerFishSpecial
    {
        get { return _player._cPlayerFishSpecial; }
    }
    protected Player _player
    {
        get { return _fish.GetPlayerHolder; }
    }

    [Header("Special")]
    public float attack;
    public PlayerAnimation.Anim specialClip;
    public int SpeiclaClipFrameCount
    {
        get { return _player.GetAnimator<PlayerAnimation>().AnimationFrame[(int)specialClip]; }
    }

    public Rigidbody _pRigid
    {
        get { return _fish.Rigidbody; }
    }

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
    protected virtual void Update()
    {

    }

    /// <summary>
    /// Gets and stores components for further use
    /// </summary>
    protected virtual void Initialization()
    {
        _fish = GetComponent<Fish>();
        _SFX = GetComponent<AudioSource>();
    }

    protected virtual void IgnoreInputFor(int ignoreFrame)
    {
        StartCoroutine(InvokeIgnoreInput(ignoreFrame));
    }

    IEnumerator InvokeIgnoreInput(int frameDuration)
    {
        int frameCount = 0;
        _player.AddAbilityInputIntercepter(this);
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        _player.RemoveAbilityInputIntercepter(this);
    }

    public void ActionForFrame(int frameDuration, System.Action begin, System.Action end)
    {
        StartCoroutine(ieActionForFrame(frameDuration, begin, end));
    }

    IEnumerator ieActionForFrame(int frameDuration, System.Action begin, System.Action end)
    {
        begin();
        int frameCount = 0;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        end();
    }

    public virtual void OnDehydrate()
    {
        if ( _player )
        {
            _player.animator.ChangeAnimState((int)PlayerAnimation.Anim.Idle);
        }
    }

    public virtual void OnPlayerDeath()
    {
        if (_player)
        {
            _player.RemoveAbilityInputIntercepter(this);
        }
    }
}
