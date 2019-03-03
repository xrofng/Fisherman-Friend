using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortRoyal : MonoBehaviour
{
    public float characterMass;

    public bool debugMode;
    
    public float respawnTime;
    public float respawnInvincTime;
    public float respawnPositionOffset = 8;

    public Vector2 FishJumpToWaterMultiplier;

    public Transform deathRealm;

    public Fish[] fishPool;
    protected float totalSpawnRate;

    public Player[] Player
    {
        get { return PlayerData.Instance.player; }
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
        totalSpawnRate = 0;
        for(int i = 0; i < fishPool.Length; i++)
        {
            totalSpawnRate += fishPool[i].spawnRate;
        }
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
    public Vector3 randomSpawnPosition(Vector3 positionOffset)
    {
        return  randomSpawnPosition() + positionOffset;
    }
    public Vector3 getSpawnPositionAtIndex(int index)
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
        float ran = Random.Range(0, totalSpawnRate) ;
        float spawnPercentOffset = 0;
        for (int i = 0; i < fishPool.Length; i++)
        {
            if(ran< fishPool[i].spawnRate + spawnPercentOffset)
            {
                return i;
            }
            spawnPercentOffset += fishPool[i].spawnRate;
        }
        Debug.LogWarning("error random fish");
        return 999;
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
