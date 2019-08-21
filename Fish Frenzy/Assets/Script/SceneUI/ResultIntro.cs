using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultIntro : CharacterAnimation
{

    public PlayerModel characterModel;

    public Transform playerColorBackDrop;

    public int playerId = -99;

    [Range(0.0f, 1.0f)]
    public float backDropAlpha = 1.0f;

    public virtual void PlayResult()
    {

    }

    public void SetPlayerId(int id)
    {
        playerId = id;
    }

    public void ChangeBackDropColor(int playerId)
    {
        foreach (SpriteRenderer backDrop in playerColorBackDrop.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = PlayerData.Instance.playerColor[PlayerData.Instance.playerSkinId[playerId]];
            c.a = backDropAlpha;
            backDrop.color = c;
        }
    }
}
