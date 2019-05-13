using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : PersistentSingleton<PlayerData> {

    public int numPlayer = 4;
    public int numBot = 0;
    public int maxNumPlayer = 4;


    public Player[] player = new Player[4];
    public Color[] playerColor = new Color[8];
    public int[] playerSkinId = new int[4];
    public Sprite[] playerIndicator = new Sprite[4];
    public Sprite[] playerIndicatorBorder = new Sprite[4];
    public Material[] playerMaterial = new Material[4];
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public Color GetColorById(int playerId)
    {
        return playerColor[playerSkinId[ playerId]];
    }
}
