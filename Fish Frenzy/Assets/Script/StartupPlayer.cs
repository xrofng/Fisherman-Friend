﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupPlayer : PersistentSingleton<StartupPlayer> {

    public Player[] player = new Player[4];
    public Color[] playerColor = new Color[4];
    public Sprite[] playerIndicator = new Sprite[4];
    public Material[] playerMaterial = new Material[4];
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
