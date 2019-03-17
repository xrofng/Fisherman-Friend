using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultIntroLose : ResultIntro
{
    public enum LoserIntro
    {
        OscarClap = 0,
    }

    public LoserIntro loseIntro;

    protected override void Start()
    {
        base.Start();
        ChangeAnimState((int)loseIntro);
    }

    protected override void Update()
    {
        base.Update();
    }
}
