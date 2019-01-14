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

    public enum Anim
    {
        Idle=0,
        Walk,
        NormalSlap,
        HammerDown,
        Throw,
        HoldFish,
        NormalThrust,
        Damaged,
        Damaged_HoldFish,
        Eel_Hook,
        Eel_Slap,
        Star_Throw,
        Shocked,
        Eaten,
        Spin,
        Spinning,
        FishingStart,
        Fishing,
        FishingEnd
    };
    public int[] AnimationFrame;
    
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        ChangeAnimState((int)Anim.Idle);
    }

    protected override void Update()
    {
    }

    public override void ChangeAnimState(int i, bool revert, int revetTo)
    {
        base.ChangeAnimState(i, AnimationFrame[i],revert, revetTo);
    }

    public Anim GetIdleAnimation()
    {
        if (!Player.holdingFish)
        {
            return Anim.Idle;
        }
        else
        {
            return Anim.HoldFish;
        }
    }
}
