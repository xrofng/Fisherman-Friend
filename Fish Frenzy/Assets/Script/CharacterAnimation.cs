using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterAnimation : MonoBehaviour {
    [HideInInspector]
    private Animator _animator;
    public Animator Animator
    {
        get
        {
            if (_animator==null) { _animator = GetComponent<Animator>(); }
            return _animator;
        }
    }

    public int _currentAnimationState =0;
    private void ChangeState(int stateI)
    {
        if (_currentAnimationState == stateI)
        {
            return;
        }
        else
        {
            Animator.SetInteger("State", stateI);
            _currentAnimationState = stateI;
        }
    }

    public List<AnimationClip> animClipList = new List<AnimationClip>();
    public AnimationClip GetClip(int i)
    {
        return animClipList[i];
    }
    public int GetClipFrame(AnimationClip clip, float percent)
    {
       // int f = (int)(clip.frameRate * percent / 100.0f);
       // print("get:"+f);
        return (int)(clip.frameRate * percent / 100.0f);
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
        
    }

    // Update is called once per frame
    protected virtual void Update () {

	}

    public void UpdateAnimatorBool(string parameterName, bool value)
    {        
        Animator.SetBool(parameterName, value);        
    }

    public void ChangeAnimState(int i, int frameDuration, bool revert, int revetTo)
    {
        StartCoroutine(InvokeChangeAnimState(i, frameDuration, revert, revetTo));
    }
    public void ChangeAnimState(int i, bool revert, int revetTo)
    {
        StartCoroutine(InvokeChangeAnimState(i, GetClipFrame(GetClip(1) , 70), revert, revetTo));
    }
    public void ChangeAnimState(int i)
    {
        ChangeState(i);
    }
    
    IEnumerator InvokeChangeAnimState(int iState, int frameDuration, bool revert, int revetTo)
    {
        int frameCount = 0;
        ChangeState(iState);
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        if (revert) { ChangeState(revetTo); }
    }

}
