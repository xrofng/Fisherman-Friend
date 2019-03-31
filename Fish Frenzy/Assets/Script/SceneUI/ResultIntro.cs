using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultIntro : CharacterAnimation {

    public GameObject characterModel;

    public Transform playerColorBackDrop;
    [Range(0.0f, 1.0f)]
    public float backDropAlpha = 1.0f;

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
