using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiPlayerCamera : MonoBehaviour
{
    public bool MultiCamEnable;
    public List<Transform> targets;
    public Vector3 offset;
    // Use this for initialization
    public float smoothTime;
    private Vector3 velocity;
    public float minZoom;
    public float maxZoom;
    public float zoomLimit;

    [Header("Camera Bound")]
    public float minXPos;
    public float maxXPos;
    public float minYPos;
    public float maxYPos;
    public float minZPos;
    public float maxZPos;


    public float GizmoRadius;
    public float speedToCenter;
    public float speedToPlayer;
    private Camera cam;
    Bounds bound;
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    public void ClearTarget()
    {
        targets.Clear();
    }
    public void AddTarget(Transform target)
    {
        targets.Add(target);
    }
    void LateUpdate()
    {
        Update_Game();
    }

    void Update_Game()
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

        float clampedY = Mathf.Clamp(transform.position.y, minYPos, maxYPos);
        transform.position = sClass.SetVector3(transform.position, VectorComponent.y, clampedY);
        float clampedZ = Mathf.Clamp(transform.position.z, minZPos, maxZPos);
        transform.position = sClass.SetVector3(transform.position, VectorComponent.z, clampedZ);

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
        if (targets != null && targets.Count == 1)
        {
            return targets[0].gameObject.transform.position;
        }

        return GetEncapsulatingBounds().center;
    }

    public Vector3 GetNewPosition()
    {
        return GetCenterPoint() + offset;
    }

    [Header("Debug")]
    public Color RayColor;
    public Vector3 gizmoSize;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = RayColor;
        Gizmos.DrawCube(new Vector3(this.transform.position.x, this.transform.position.y, minZPos), gizmoSize -Vector3.forward * (gizmoSize.z - 1));
        Gizmos.DrawCube(new Vector3(this.transform.position.x, this.transform.position.y, maxZPos), gizmoSize -Vector3.forward * (gizmoSize.z - 1));
        Gizmos.DrawCube(new Vector3(this.transform.position.x, maxYPos, this.transform.position.z ), gizmoSize - Vector3.up*(gizmoSize.y-1));

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetCenterPoint(), GizmoRadius);
    }
}
