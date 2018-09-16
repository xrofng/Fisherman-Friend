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
    public static float sJumpFaster;
    public float jumpFaster;
    public static float sFallFaster;
    public float fallFaster;
    public static float sMaxHoldToThrow;
    public float maxHoldToThrow;


    public Fish[] fishPool;
    public Player[] player;
   
    // Use this for initialization
    void Start () {
        sCharacterSpeed = speed;
        sJumpForce = jumpForce;
        sCharMass = characterMass;
        sDebugMode = debugMode;
        sFallFaster = fallFaster;
        sJumpFaster = jumpFaster;
        sMaxHoldToThrow = maxHoldToThrow;
    }
	
	// Update is called once per frame
	void Update () {
        changePlayer();

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
    void changePlayer()
    {
        KeyCode k = KeyCode.Alpha1;
        for(int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(k)) {

                GameObject oldOne = GameObject.Find("Player1");
                GameObject g = player[i].gameObject;
                oldOne.name = g.name;
                g.name = "Player1";
                for(int j = 0; j < 4; j++)
                {
                    player[j].player = player[j].gameObject.name[6] - 48;
                }
            }
            k++;
        }
        

    }

}
