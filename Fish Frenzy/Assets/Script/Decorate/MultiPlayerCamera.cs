using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerCamera : PersistentSingleton<MultiPlayerCamera>
{
    public bool MultiCamEnable;
    public List<Transform> targets;
    public Transform stage;
    public Vector3 offset;
    // Use this for initialization
    private float smoothTime;
    private Vector3 velocity;
    public float minZoom;
    public float maxZoom;
    public float zoomLimit;
    public float GizmoRadius;
    public float speedToCenter;
    public float speedToPlayer;
    private Camera cam;
    Bounds bound;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void AddTarget(Transform target)
    {
        targets.Add(target);
    }
    void LateUpdate()
    {
        if (!MultiCamEnable)
        {
            return;
        }
        if (targets.Count == 0)
        {
            return;
        }
        Move();
        Zoom();

    }
    void Move()
    {
        transform.position = Vector3.SmoothDamp(transform.position, GetNewPosition(), ref velocity, smoothTime);
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
            bound.Encapsulate(targets[i].gameObject.transform.position);
        }
        return bound;
    }
    float GetGreatestDistance()
    {
        return GetEncapsulatingBounds().size.x;
    }
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].gameObject.transform.position;
        }

        return GetEncapsulatingBounds().center;
    }

    public Vector3 GetNewPosition()
    {
        return GetCenterPoint() + offset;
    }
}
