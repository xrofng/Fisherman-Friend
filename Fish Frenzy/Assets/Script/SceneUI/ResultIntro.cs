using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultIntro : CharacterAnimation {

    public GameObject characterModel;

    public Transform playerColorBackDrop;


    public void ChangeBackDropColor(int playerId)
    {
        foreach (SpriteRenderer backDrop in playerColorBackDrop.GetComponentsInChildren<SpriteRenderer>())
        {
            backDrop.color = PlayerData.Instance.playerColor[playerId];
        }
    }
}
