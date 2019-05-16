using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData : PersistentSingleton<PlayerData> {

    public int numPlayer = 4;
    public int numBot = 0;
    public int maxNumPlayer = 4;

    public Player[] player = new Player[4];
    public Color[] playerColor = new Color[8];

    [Header("Customization")]
    public int[] playerSkinId = new int[4];
    public int[] hatId = new int[4];
    public int[] victoryId = new int[4];

    public Sprite[] playerIndicator = new Sprite[4];
    public Sprite[] playerIndicatorBorder = new Sprite[4];
    public Material[] playerMaterial = new Material[4];

    public Hat[] hats;

    public Color GetColor(int playerId)
    {
        return playerColor[playerSkinId[ playerId]];
    }

    public Sprite GetHatSprite(int playerId)
    {
        return hats[hatId[playerId]].hatSprite;
    }

    [Serializable]
    public class Hat
    {
        public string name;
        public Sprite hatSprite;
        public GameObject hat;
    }
}
