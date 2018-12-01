using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fish))]
public class FishSpecial : MonoBehaviour
{
    protected Fish _fish;

    protected PlayerSpecial _playerFishSpecial
    {
        get { return _fish.GetPlayerHolder._cPlayerFishSpecial; }
    }

    [Header("Special")]
    public float attack;
    public enum MeleeAnimation
    {
        LightHorizontal = 2,
        HammerDown = 3,
        LightStab = 6
    }
    public int[] AnimationFrame = { 0, 0, 20, 50, 0, 0, 35 };

    public MeleeAnimation specialClip;
    public int SpeiclaClipFrameCount
    {
        get { return AnimationFrame[(int)specialClip]; }
    }

    // private ignore Input for specific ability
    protected bool ignoreInput;
    public bool IgnoreInput { get { return ignoreInput; } }

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

}
