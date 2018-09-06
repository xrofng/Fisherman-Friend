using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPoint : MonoBehaviour {
    public bool inWater;
    List<Collider> collidedObjects = new List<Collider>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void FixedUpdate()
    {
        inWater = contactOnlyWater();
    }

    bool contactOnlyWater()
    {
        string lis = "";
        foreach(Collider c in collidedObjects)
        {
            lis += c.gameObject.name;
            if(c.gameObject.tag != "Sea")
            {
                collidedObjects.Clear();
                return false;
            }
        }
        collidedObjects.Clear();
        return true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (!collidedObjects.Contains(other.collider) ){
            collidedObjects.Add(other.gameObject.GetComponent<Collider>());
        }
    }

    void OnCollisionStay(Collision other)
    {
        OnCollisionEnter(other);
    }
    void OnCollisionExit(Collision other)
    {
        collidedObjects.Remove(other.gameObject.GetComponent<Collider>());
        if (other.gameObject.tag == "Sea")
        {
            inWater = false;
        }
    }
}
