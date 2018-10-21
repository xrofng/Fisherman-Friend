using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]

public class PlayerAbility : MonoBehaviour
{
    protected Player _player;
    protected bool ignoreInput;
    public bool IgnoreInput { get { return ignoreInput; } }

    public Rigidbody _pRigid
    {
        get { return _player.rigid; }

    }

    public PlayerState _pState
    {
        get { return _player._cPlayerState; }

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

}
