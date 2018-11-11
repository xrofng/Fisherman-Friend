using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerCamera : PersistentSingleton<MultiPlayerCamera> {
    public List<Player> targets;
    public Transform stage;
    public Vector3 offset;
    // Use this for initialization
    private float smoothTime;
    private Vector3 velocity;
    public float minZoom;
    public float maxZoom;
    public float zoomLimit;
    public float GizmoRadius;
    private Camera cam;
    Bounds bound;
    void Start () {
        cam = GetComponent<Camera>();
    }

    public void Initialization()
    {
        for (int i = 0; i < 4; i++)
        {
            targets.Add(PortRoyal.Instance.Player[i]);
        }
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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetCenterPoint(), GizmoRadius);
    }
    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimit);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
    Bounds GetEncapsulatingBounds()
    {
        bound = new Bounds();
        for (int i = 0; i < targets.Count; i++)
        {
            if (!targets[i].Death)
            {
                bound.Encapsulate(targets[i].gameObject.transform.position);
            }
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
            return targets[0].gameObject.transform.position;
        }
  
        return GetEncapsulatingBounds().center;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
