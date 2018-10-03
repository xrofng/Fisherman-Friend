using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortRoyal : PersistentSingleton<PortRoyal>
{

    public Vector3 speed;

    public Vector3 jumpForce;

    public float characterMass;

    public bool debugMode;


    public float jumpFaster;

    public float fallFaster;
    
    public float maxHoldToThrow;

    public float aimRadius;

    public float respawnTime;
    
    public Transform deathRealm;

    public Fish[] fishPool;
    public Player[] player;
    public Color[] playerColor;
    public Material[] playerMaterial;
    public Transform[] spawnPoint;
    public Camera mainCamera;
    public MultiPlayerCamera multiPCamera;

    // Use this for initialization
    void Start ()
    { 

    }
	
	// Update is called once per frame
	void Update () {
        changePlayer();
    }
    public int randomSpawnPosIndex()
    {
        return Random.Range(0, spawnPoint.Length);
    }
    public Vector3 randomSpawnPosition()
    {
        return spawnPoint[randomSpawnPosIndex()].position;
    }
    public Vector3 getSpwanPositionAtIndex(int index)
    {
        return spawnPoint[index].position;
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
            if (Input.GetKeyDown(k))
            {
                GameObject oldOne = GameObject.Find("Player1");
                GameObject g = player[i].gameObject;
                oldOne.name = g.name;
                g.name = "Player1";
                for(int j = 0; j < 4; j++)
                {
                    player[j].playerID = player[j].gameObject.name[6] - 48;
                }
            }
            k++;
        }
    }

}
