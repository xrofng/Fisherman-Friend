using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimator : MonoBehaviour
{
    protected Animator _animator;
    public Animator Animator
    {
        get
        {
            if(_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    public string stateVarName = "State";
    protected int currentState = 0;
    public void ChangeState(int newState)
    {
        if (currentState == newState)
        {
            return;
        }
        else
        {
            Animator.SetInteger(stateVarName, newState);
            currentState = newState;
        }
    }

    public void ChangeStateOnce(int newState, int frameDuration)
    {
        StartCoroutine(ChangeStateForFrame(  newState, frameDuration));
    }

    IEnumerator ChangeStateForFrame(int newState,int frameDuration)
    {
        ChangeState(newState);
        int frameCount = 0;
        while(frameCount < frameDuration)
        {
            frameCount += 1;
            yield return new WaitForEndOfFrame();
        }
        ChangeState(0);
    }
}
