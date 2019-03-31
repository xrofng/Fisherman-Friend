using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoserIntro
{
    None = 0,
    OscarClap,
    NormalClap,
    DontWantClap
}

public class ResultIntroLose : ResultIntro
{
    public LoserIntro loseIntro;

    public override void Initialize()
    {
        base.Initialize();
        ChangeAnimState((int)loseIntro);
    }

    protected override void Update()
    {
        base.Update();
    }

}