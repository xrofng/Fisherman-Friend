using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VictoryAnimation
{
    Random = 0,
    Generic,
    Starfish,
    Hammerhead,
    Swordfish,
    Eel
}

public class ResultIntroVictory : ResultIntro
{
    public override void PlayResult()
    {
        base.PlayResult();
        // TODO real random
        int vicId = PlayerData.Instance.victoryId[playerId];
        if(vicId == (int)VictoryAnimation.Random)
        {
            vicId = (int)VictoryAnimation.Hammerhead;
        }
        ChangeState(vicId);
    }

    protected override void Update()
    {
        base.Update();
    }
}
