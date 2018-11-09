using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    public Animator _animator;

    public int _currentAnimationState =0;
    public void ChangeState(int stateI)
    {
        if (_currentAnimationState == stateI)
        {
            return;
        }
        else
        {
            _animator.SetInteger("state", stateI);
            _currentAnimationState = stateI;
        }
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
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update () {
		
	}
}
