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
    public LayerMask firstSpawnFishLayer;

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

    [Header("Camera")]
    public Camera mainCamera;
    public MultiPlayerCamera multiPCamera;

    [Header("Coast")]
    public GameObject coastHolder;
    public List<Coast> coastPoints = new List<Coast>();

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

        foreach (Coast coast in coastHolder.GetComponentsInChildren<Coast>())
        {
            coastPoints.Add(coast);
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
    
    public Fish GetFish(int number)
    {
        return fishPool[number];
    }
    public Fish RandomFish()
    {
        if (FixedFish)
        {
            return TestingFish;
        }
        int fishIndex = GetRandomFishIndex(); 
        return GetFish(fishIndex);
    }

    public int GetRandomFishIndex()
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
        Fish newFish = Instantiate(spawnFish, Player[0].transform.position + Vector3.up * 5, Random.rotation) as Fish;
        newFish.gameObject.transform.localEulerAngles = sClass.SetVector3(newFish.gameObject.transform.localEulerAngles, VectorComponent.x, 0);
        newFish.gameObject.transform.localEulerAngles = sClass.SetVector3(newFish.gameObject.transform.localEulerAngles, VectorComponent.z, 0);
        newFish.ChangeState(Fish.fState.fall);
        newFish.gameObject.AddComponent<Rigidbody>();
        newFish.Rigidbody.freezeRotation = true;
        newFish.gameObject.layer = firstSpawnFishLayer;
        newFish.GetCollider<BoxCollider>().isTrigger = true;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        foreach(Transform point in coastHolder.GetComponentInChildren<Transform>())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(point.position, 1);
            Gizmos.color += new Color(0, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(point.position+point.forward*5, 1);
        }

    }
}
