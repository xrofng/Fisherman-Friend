using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortRoyal : PersistentSingleton<PortRoyal>
{
    public int numPlayer = 4;
    public int maxNumPlayer = 4;

    public float characterMass;

    public bool debugMode;
    
    public float respawnTime;

    public Vector2 FishJumpToWaterMultiplier;

    public Transform deathRealm;

    public Fish[] fishPool;
    [HideInInspector]
    public StartupPlayer startupPlayer;
    public Player[] Player
    {
        get { return startupPlayer.player; }
    }
    public Transform[] spawnPoint;
    public Transform underWater;
    public Camera mainCamera;
    public MultiPlayerCamera multiPCamera;

    [Header("Debug")]
    public bool FixedFish = false;
    public Fish TestingFish;
    // Use this for initialization
    void Start ()
    {
        startupPlayer = GetComponent<StartupPlayer>();
    }
	
	// Update is called once per frame
	void Update () {
        changePlayer();
        if (Input.GetKeyDown(KeyCode.H))
        {
            Fish f = Instantiate(TestingFish, Player[0].transform.position+Vector3.up * 5, Random.rotation) as Fish;
            f.gameObject.transform.localEulerAngles = sClass.setVector3(f.gameObject.transform.localEulerAngles, sClass.vectorComponent.x, 0);
            f.gameObject.transform.localEulerAngles = sClass.setVector3(f.gameObject.transform.localEulerAngles, sClass.vectorComponent.z, 0);
            f.ChangeState(Fish.fState.fall);
            f.gameObject.AddComponent<Rigidbody>();
            f._rigidbody.freezeRotation = true;
            f.gameObject.layer = LayerMask.NameToLayer("Fish");
            f.GetCollider<BoxCollider>().isTrigger = true;
        }
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
        if (FixedFish)
        {
            return TestingFish;
        }
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
                GameObject g = Player[i].gameObject;
                oldOne.name = g.name;
                g.name = "Player1";
                for(int j = 0; j < 4; j++)
                {
                    Player[j].playerID = Player[j].gameObject.name[6] - 48;
                }
            }
            k++;
        }
    }

}
