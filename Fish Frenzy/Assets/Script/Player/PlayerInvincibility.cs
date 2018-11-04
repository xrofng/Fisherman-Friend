﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincibility : PlayerAbility {
   
    public bool IsInvincible
    {
        get
        {
            return isInvincible;
        }
        set
        {
            isInvincible = value;
        }
    }
    public  int invisibilityFrame = 50;

    [ReadOnly]
    bool isInvincible;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startInvincible(int inviFrame)
    {
        invisibilityFrame = inviFrame;
        StartCoroutine(invokeInvinceible(invisibilityFrame));
    }

    IEnumerator invokeInvinceible(int frameDuration)
    {
        int frameCount = 0;
        IsInvincible = true;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        IsInvincible = false;
    }
}
