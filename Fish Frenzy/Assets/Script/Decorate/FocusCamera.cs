using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FocusCamera : MonoBehaviour
{
    public Vector3 focusPlayerOffset;

    private Queue<Vector3> focusQueue = new Queue<Vector3>();

    public float smoothTime;
    public Vector3 velocity;

    private bool focusingTarget = false;
    private float focusingTime = 0.0f;
    public float playerFocusingTime = 0.1f;
    private float focusingTimeCountDown = 0.0f;

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

    void Start()
    {
         focusingTime = playerFocusingTime * (PlayerData.Instance.maxNumPlayer+1 - PlayerData.Instance.numPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        Update_Game();
    }

    void Update_Game()
    {
        
        if (focusQueue.Count != 0)
        {
            Vector3 newPosition = focusQueue.Peek();
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }
    }
}
