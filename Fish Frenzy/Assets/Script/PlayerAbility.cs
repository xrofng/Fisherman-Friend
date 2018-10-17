using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]

public class PlayerAbility : MonoBehaviour
{
    protected Player _player;

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


}
