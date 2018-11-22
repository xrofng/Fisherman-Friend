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
        F_Stab
    };
    
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        ChangeState((int)State.Idle);
    }

    protected override void Update()
    {
    }

    public void ChangeAnimState(int i, int ignoreFrame, bool revert, PlayerAnimation.State revetTo)
    {
        StartCoroutine(InvokeChangeAnimState(i, ignoreFrame, revert, revetTo));
    }
    public void ChangeAnimState(PlayerAnimation.State s, int ignoreFrame, bool revert, PlayerAnimation.State revetTo)
    {
        ChangeAnimState((int)s, ignoreFrame, revert, revetTo);
    }
    public void ChangeAnimState(Fish.MeleeAnimation s, int ignoreFrame, bool revert, PlayerAnimation.State revetTo)
    {
        ChangeAnimState((int)s, ignoreFrame, revert, revetTo);
    }

    public void ChangeAnimState(PlayerAnimation.State s)
    {
        ChangeState((int)s);
    }

    IEnumerator InvokeChangeAnimState(int iState, int frameDuration, bool revert, PlayerAnimation.State revetTo)
    {
        int frameCount = 0;
        ChangeState(iState);
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        if (revert) { ChangeState((int)revetTo); }
    }
}
