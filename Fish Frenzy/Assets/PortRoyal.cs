﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortRoyal : MonoBehaviour {
    public static Vector3 sCharacterSpeed;
    public Vector3 speed;
    public static Vector3 sJumpForce;
    public Vector3 jumpForce;
    public static float sCharMass;
    public float characterMass;

    // Use this for initialization
    void Start () {
        sCharacterSpeed = speed;
        sJumpForce = jumpForce;
        sCharMass = characterMass;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
