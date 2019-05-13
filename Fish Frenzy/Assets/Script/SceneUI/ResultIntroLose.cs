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

    public GameObjectMovement subCamera;
    public GameObjectMovement frame;

    public override void PlayResult()
    {
        base.PlayResult();
        ChangeAnimState((int)loseIntro);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void StartShowResultIntro()
    {
        subCamera.StartMove();
        frame.StartMove();
    }
}