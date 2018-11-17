using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzySpawner : MonoBehaviour {
    private bool Frenzying;
    public Vector2 spawnTime;
    public Vector2 fallSpeed;
    private float timeCount = 0;
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
                float posX = Random.Range(transform.position.x - transform.lossyScale.x / 2.0f , transform.position.x + transform.lossyScale.x / 2.0f);
                float posY = transform.position.y;
                float posZ = Random.Range(transform.position.z - transform.lossyScale.z / 2.0f, transform.position.z + transform.lossyScale.z / 2.0f);
                Vector3 spawnPos = new Vector3(posX, posY, posZ);
                print(CheckGround(spawnPos));
                if (!CheckGround(spawnPos)) {
                    return;
                }
                Fish spawnFish = Instantiate(PortRoyal.Instance.randomFish(), spawnPos, Random.rotation) as Fish;
                spawnFish.gameObject.transform.localEulerAngles = sClass.setVector3(spawnFish.gameObject.transform.localEulerAngles, sClass.vectorComponent.x, 0);
                spawnFish.gameObject.transform.localEulerAngles = sClass.setVector3(spawnFish.gameObject.transform.localEulerAngles, sClass.vectorComponent.z, 0);
                spawnFish.changeState(Fish.fState.fall);
                spawnFish.gameObject.AddComponent<Rigidbody>();
                spawnFish._rigidbody.freezeRotation = true;
                spawnFish.gameObject.layer = LayerMask.NameToLayer("Fish");
                spawnFish._rigidbody.AddForce(0, -Random.Range(fallSpeed.x, fallSpeed.y), 0,ForceMode.Impulse);
                RandomNextSpawnTime();
            }
        }
	}

    public void StartFrenzy(bool b)
    {
        RandomNextSpawnTime();
        Frenzying = b;
    }

    void RandomNextSpawnTime()
    {
        timeCount = Random.Range(spawnTime.x, spawnTime.y);
    }

    bool CheckGround(Vector3 spawnPoint)
    {
        RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(spawnPoint, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {

            if (hit.transform.gameObject.tag == "Ground")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

}
