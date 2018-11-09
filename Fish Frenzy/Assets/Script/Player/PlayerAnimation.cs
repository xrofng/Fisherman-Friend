using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : CharacterAnimation {

    public enum State
    {
        Idle=0,
        Walk,
        Slap,
        Throw
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
}
