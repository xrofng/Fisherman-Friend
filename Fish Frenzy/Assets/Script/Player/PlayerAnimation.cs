using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : CharacterAnimation {

    protected Player _player;
    public Player Player
    {
        get
        {
            if (!_player) { _player = GetComponent<Player>(); }
            return _player;
        }
    }

    public enum State
    {
        Idle=0,
        Walk,
        H_Slap,
        V_Slap,
        Throw,
        HoldFish,
        F_Stab,
        Damaged,
        Damaged_HoldFish,
        Eel_Hook,
        Eel_Slap,
        Star_Throw
    };
    
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        ChangeAnimState((int)State.Idle);
    }

    protected override void Update()
    {
    }

 
}
