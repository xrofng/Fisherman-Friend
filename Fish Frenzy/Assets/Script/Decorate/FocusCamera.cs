using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : PersistentSingleton<FocusCamera>
{
    public Vector3 focusPlayerOffset;

    private Queue<Vector3> focusQueue = new Queue<Vector3>();

    public float timeToNextTarget = 0.1f;

    public float smoothTime;
    public Vector3 velocity;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="applyOffset"></param>
    public void MoveCameraTo(Vector3 destination , bool applyOffset)
    {
        if (focusQueue.Count > 0)
        {
            focusQueue.Dequeue();
        }
        Vector3 newPos = destination;
        if (applyOffset)
        {
            newPos = destination + focusPlayerOffset;
        }
        focusQueue.Enqueue(newPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (focusQueue.Count != 0)
        {
            Vector3 newPosition = focusQueue.Peek() ;
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }
    }
}
