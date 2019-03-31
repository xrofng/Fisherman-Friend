using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VictoryAnimation
{
    None = 0,
    Generic,
    Starfish,
    Hammerhead,
    Swordfish,
    Eel
}

public class ResultIntroVictory : ResultIntro
{
    public VictoryAnimation vicIntro;

    public override void Initialize()
    {
        base.Initialize();
        ChangeAnimState((int)vicIntro);
    }

    protected override void Update()
    {
        base.Update();
    }
}
