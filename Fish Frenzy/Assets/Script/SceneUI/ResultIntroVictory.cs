using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultIntroVictory : ResultIntro
{
    public enum VictoryAnimation
    {
        Generic = 0,
        Starfish,
        Hammerhead,
        Swordfish,
        Eel
    }

    public VictoryAnimation vicIntro;


    protected override void Start()
    {
        base.Start();
        ChangeAnimState((int)vicIntro);
    }

    protected override void Update()
    {
        base.Update();
    }
}
