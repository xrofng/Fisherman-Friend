using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortRoyal : MonoBehaviour
{
    public float characterMass;

    public float respawnTime;
    public float respawnInvincTime;
    public float respawnPositionOffset = 8;

    public Vector2 FishJumpToWaterMultiplier;

    public Transform deathRealm;

    public FishPool FishPool;
    protected float totalSpawnRate;

    public Player[] Player
    {
        get { return PlayerData.Instance.player; }
    }

    public Transform StageCenterCeil;

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
    public bool DrawDebugRay;

    // Use this for initialization
    void Start ()
    {
        FishPool.Initialization();

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

    public int RandomSpawnPosIndex()
    {
        return Random.Range(0, spawnPoint.Length);
    }
    public Vector3 RandomSpawnPosition()
    {
        return spawnPoint[RandomSpawnPosIndex()].position;
    }
    public Vector3 RandomSpawnPosition(Vector3 positionOffset)
    {
        return  RandomSpawnPosition() + positionOffset;
    }
    public Vector3 getSpawnPositionAtIndex(int index)
    {
        return spawnPoint[index].position;
    }
    
    public Fish GetFish(int number)
    {
        return FishPool.FishSpawnings[number].Fish;
    }
    public Fish RandomFish()
    {
        if (FixedFish)
        {
            return TestingFish;
        }
        int fishIndex = FishPool.GetRandomFishIndex(); 
        return GetFish(fishIndex);
    }


    void Test()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ForceSpawnFish(TestingFish);
        }
        KeyCode spa = KeyCode.Keypad1;
        for(int i = 0; i < FishPool.FishSpawnings.Count; i++)
        {
            if (Input.GetKeyDown(spa))
            {
                ForceSpawnFish(FishPool.FishSpawnings[i].Fish);
            }
            spa += 1;
        }
    }

    void ForceSpawnFish(Fish spawnFish)
    {
        Fish newFish = Instantiate(spawnFish, Player[0].transform.position + Vector3.up * 5, Random.rotation) as Fish;
        newFish.gameObject.transform.localEulerAngles = sClass.SetVector3(newFish.gameObject.transform.localEulerAngles, VectorComponent.x, 0);
        newFish.gameObject.transform.localEulerAngles = sClass.SetVector3(newFish.gameObject.transform.localEulerAngles, VectorComponent.z, 0);
        newFish.ChangeState(Fish.FishConditionalState.fall);
        newFish.gameObject.AddComponent<Rigidbody>();
        newFish.Rigidbody.freezeRotation = true;
        newFish.gameObject.layer = LayerMask.NameToLayer("Fish_All");
        newFish.GetCollider<BoxCollider>().isTrigger = true;
    }

    void OnDrawGizmos()
    {
        if (!DrawDebugRay)
        {
            return;
        }
        // Draw a yellow sphere at the transform's position
        foreach(Transform point in coastHolder.GetComponentInChildren<Transform>())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(point.position, 1);
            Gizmos.color += new Color(0, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(point.position+point.forward*5, 1);
        }
        Gizmos.DrawWireSphere(StageCenterCeil.transform.position, 1);
    }
}
