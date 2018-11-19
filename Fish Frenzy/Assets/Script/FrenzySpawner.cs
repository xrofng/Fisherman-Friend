using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzySpawner : MonoBehaviour {
    private bool Frenzying;
    public Vector2 spawnTime;
    public Vector2 fallSpeed;
    public List<Transform> SpawnPoints = new List<Transform>();
    public List<int> spawnedPoint = new List<int>();
    
    private float timeCount = 0;
    public float timeToNextWave = 10;
    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        if (Frenzying)
        {
            if (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
            }
            if (timeCount <= 0)
            {
                int spawnPointIndex = Random.Range(0, SpawnPoints.Count - 1);
                while (spawnedPoint.Contains(spawnPointIndex))
                {
                    spawnPointIndex = Random.Range(0, SpawnPoints.Count - 1);
                }

                Vector3 spawnPos = SpawnPoints[spawnPointIndex].position;
                spawnPos = sClass.setVector3(spawnPos, sClass.vectorComponent.y, transform.position.y);

              
              

                timeCount = timeToNextWave;

            }
        }
	}

    void SpawnWave()
    {

    }

    void SpawnFish(Vector3 spawnPos)
    {
        Fish spawnFish = Instantiate(PortRoyal.Instance.randomFish(), spawnPos, Random.rotation) as Fish;
        spawnFish.gameObject.transform.localEulerAngles = sClass.setVector3(spawnFish.gameObject.transform.localEulerAngles, sClass.vectorComponent.x, 0);
        spawnFish.gameObject.transform.localEulerAngles = sClass.setVector3(spawnFish.gameObject.transform.localEulerAngles, sClass.vectorComponent.z, 0);
        spawnFish.changeState(Fish.fState.fall);
        spawnFish.gameObject.AddComponent<Rigidbody>();
        spawnFish._rigidbody.freezeRotation = true;
        spawnFish.gameObject.layer = LayerMask.NameToLayer("Fish");
        spawnFish.GetCollider<BoxCollider>().isTrigger = true;
        spawnFish._rigidbody.AddForce(0, -Random.Range(fallSpeed.x, fallSpeed.y), 0, ForceMode.Impulse);
    }

    public void StartFrenzy(bool b)
    {
        timeCount = timeToNextWave;
        Frenzying = b;
    }

    // Draw Path
    [Header("Debug")]
    public Color rayColor;
    public float wireSphereRadius = 1.0f;
    public bool showRay = true;
    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        SpawnPoints.Clear();

        foreach (Transform pointobj in transform.GetComponentsInChildren<Transform>())
        {
            if (pointobj != this.transform)
            {
                SpawnPoints.Add(pointobj);
                Gizmos.DrawWireSphere(pointobj.position, wireSphereRadius);
            }
           
        }
    }
}
