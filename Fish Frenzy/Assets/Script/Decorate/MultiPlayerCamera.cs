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
        if (SceneManager.GetActiveScene().name != "Gameplay")
        {
            return;
        }
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
        float clampedZ = Mathf.Clamp(transform.position.z, minZPos, maxZPos);
        transform.position = sClass.setVector3(transform.position, sClass.vectorComponent.z, clampedZ);
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

    [Header("Debug")]
    public Color RayColor;
    public Vector3 gizmoSize;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = RayColor;
        Gizmos.DrawCube(new Vector3(this.transform.position.x, this.transform.position.y, minZPos), gizmoSize);
        Gizmos.DrawCube(new Vector3(this.transform.position.x, this.transform.position.y, maxZPos), gizmoSize);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetCenterPoint(), GizmoRadius);
    }
}
