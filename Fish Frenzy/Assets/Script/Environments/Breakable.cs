using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public Vector2 force = new Vector2(1, 5);
    public float destroyDelay = 3;

    public void Break()
    {
        int f = 2;
        int c = 0;
        foreach (MeshRenderer child in GetComponentsInChildren<MeshRenderer>())
        {
            if (child == this.gameObject.transform)
            {
                continue;
            }
            if (c % f == 0)
            {
                Rigidbody _rigidbody = child.gameObject.AddComponent<Rigidbody>();
                if (_rigidbody)
                {
                    _rigidbody.velocity = Vector3.zero;
                    float ranz = Random.Range(0.0f, 1.0f);
                    float _force = Random.Range(force.x, force.y);
                    _rigidbody.AddForce(Random.insideUnitSphere * _force);
                }
                
                child.gameObject.AddComponent<SphereCollider>();
                Destroy(child.gameObject, destroyDelay);
            }
            else
            {
                Destroy(child.gameObject);
            }
            c += 1;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
