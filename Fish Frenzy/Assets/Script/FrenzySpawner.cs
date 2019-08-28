using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzySpawner : MonoBehaviour
{
    public bool frenzyStarted;
    public bool jumping;
    public Vector2 spawnTime;
    public Vector2 fallSpeed;
    public Vector2 amountRange;

    [Header("SpawnPos")]
    public float spawnPosY;
    public Vector3 spawnOffset;

    public Transform frenzyPointStore;
    private Transform[] spawnPoints;
    private List<int> spawnedPoint = new List<int>();

    [Header("Other Class Ref")]
    protected PortRoyal portRoyal;
    // Use this for initialization
    void Start ()
    {
        portRoyal = FFGameManager.Instance.PortRoyal;
        spawnPoints = frenzyPointStore.GetComponentsInChildren<Transform>();
    }
	
    public void FrenzySpawnFish()
    {
        int amountFish = (int)Random.Range(amountRange.x, amountRange.y);
        spawnedPoint.Clear();
        for (int i = 0; i < amountFish; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length - 1);
            while (spawnedPoint.Contains(spawnPointIndex))
            {
                spawnPointIndex = Random.Range(0, spawnPoints.Length - 1);
            }
            spawnedPoint.Add(spawnPointIndex);

            Vector3 spawnPos = spawnPoints[spawnPointIndex].position;
            spawnPos.y = spawnPosY;
            spawnPos += spawnOffset;

            SpawnFish(spawnPos);
        }
    }

    void SpawnFish(Vector3 spawnPos)
    {
        Fish spawnFish = Instantiate(portRoyal.RandomFish(), spawnPos, Random.rotation) as Fish;
        spawnFish.gameObject.transform.localEulerAngles = sClass.SetVector3(spawnFish.gameObject.transform.localEulerAngles, VectorComponent.x, 0);
        spawnFish.gameObject.transform.localEulerAngles = sClass.SetVector3(spawnFish.gameObject.transform.localEulerAngles, VectorComponent.z, 0);
        spawnFish.ChangeState(Fish.FishConditionalState.fall);
        //spawnFish.GetCollider<BoxCollider>().isTrigger = true;

        //spawnFish.gameObject.layer = LayerMask.NameToLayer("Fish");
        float f = Random.Range(fallSpeed.x, fallSpeed.y);
        spawnFish.FishJump(1, 10, Vector3.down, -f);
        //spawnFish._rigidbody.freezeRotation = true;
    }

    public void StartFrenzy(bool b)
    {
        frenzyStarted = b;
    }

    // Draw Point
    [Header("Debug")]
    public Color rayColor;
    public float wireSphereRadius = 1.0f;
    public bool showRay = true;
    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;

        foreach (Transform pointobj in frenzyPointStore.GetComponentsInChildren<Transform>())
        {
            Gizmos.DrawWireSphere(pointobj.position, wireSphereRadius);
        }
    }
}
