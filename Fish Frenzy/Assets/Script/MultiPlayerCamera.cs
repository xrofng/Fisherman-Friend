using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerCamera : MonoBehaviour {
    public List<Transform> targets;
    public Vector3 offset;
    // Use this for initialization
    private float smoothTime;
    private Vector3 velocity;
    public float minZoom;
    public float maxZoom;
    public float zoomLimit;
    private Camera cam;
    Bounds bound;
    void Start () {
        cam = GetComponent<Camera>();
        
    }
	void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }
        Move();
        Zoom();
        
    }
    void Move()
    {
        Vector3 newPosition = GetCenterPoint() + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }
    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimit);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
    Bounds GetEncapsulatingBounds()
    {
        bound = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bound.Encapsulate(targets[i].position);
        }
        return bound;
    }
    float GetGreatestDistance()
    {
       
        return GetEncapsulatingBounds().size.x;
    }
    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }
  
        return GetEncapsulatingBounds().center;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
