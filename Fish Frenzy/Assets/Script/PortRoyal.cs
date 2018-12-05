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
    public float respawnInvincTime;

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
	void Update ()
    {
        Test();
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


    void Test()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ForceSpawnFish(TestingFish);
        }
        KeyCode spa = KeyCode.Keypad1;
        for(int i = 0; i < fishPool.Length; i++)
        {
            if (Input.GetKeyDown(spa))
            {
                ForceSpawnFish(fishPool[i]);
            }
            spa += 1;
        }
    }

    void ForceSpawnFish(Fish spawnFish)
    {
        Fish f = Instantiate(spawnFish, Player[0].transform.position + Vector3.up * 5, Random.rotation) as Fish;
        f.gameObject.transform.localEulerAngles = sClass.setVector3(f.gameObject.transform.localEulerAngles, sClass.vectorComponent.x, 0);
        f.gameObject.transform.localEulerAngles = sClass.setVector3(f.gameObject.transform.localEulerAngles, sClass.vectorComponent.z, 0);
        f.ChangeState(Fish.fState.fall);
        f.gameObject.AddComponent<Rigidbody>();
        f.Rigidbody.freezeRotation = true;
        f.gameObject.layer = LayerMask.NameToLayer("Fish_All");
        f.GetCollider<BoxCollider>().isTrigger = true;
    }
}
