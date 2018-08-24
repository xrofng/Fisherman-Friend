using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortRoyal : MonoBehaviour {
    public static Vector3 sCharacterSpeed;
    public Vector3 speed;
    public static Vector3 sJumpForce;
    public Vector3 jumpForce;
    public static float sCharMass;
    public float characterMass;
    public static bool sDebugMode;
    public bool debugMode;

    public Fish[] fishPool;
   
    // Use this for initialization
    void Start () {
        sCharacterSpeed = speed;
        sJumpForce = jumpForce;
        sCharMass = characterMass;
        sDebugMode = debugMode;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Fish getFish(int number)
    {
        return fishPool[number];
    }
    public Fish randomFish()
    {
        int fishIndex = randomFishIndex(); 
        return getFish(fishIndex);
    }
    public int randomFishIndex()
    {
        return Random.Range(0, fishPool.Length);
        
   
    }

}
