using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrofng;

public class EnvironmentResponse : MonoBehaviour
{
    public string animTriggerName = "response";
    public LayerMask ResponseToLayers;
    public Vector3 ResponseAreaSize;
    public Vector3 ResponseAreaOffset;

    private Animator m_animator;
    public Animator Animator
    {
        get
        {
            if(m_animator == null)
            {
                m_animator = GetComponent<Animator>();
            }
            return m_animator;
        }
    }

    public RandomFloat ResponseDelay = new RandomFloat(5, 9);
    private PercentFloat delayTimerCount = new PercentFloat(0, 100);

    private void Start()
    {
        UpdateRandomDelay();
    }

    private void Update()
    {
        delayTimerCount.AddValue(Time.deltaTime);
        if (!delayTimerCount.IsAtMax)
        {
            return;
        }

        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + ResponseAreaOffset, ResponseAreaSize, Quaternion.identity, ResponseToLayers);
        
        //Check when there is a new collider coming into contact with the box
        foreach(Collider hit  in hitColliders)
        {
            //Output all of the collider names
            Debug.Log("Hit : " + hit.name);
            if (hit)
            {
                Animator.SetTrigger(animTriggerName);
                UpdateRandomDelay();
                return;
            }
        }
    }

    void UpdateRandomDelay()
    {
        ResponseDelay.InitRandom();
        delayTimerCount.MaxVal = ResponseDelay.Value;
        delayTimerCount.SetValue01(0);
    }

    [Header("Debug")]
    public bool DrawRay = false;
    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        if (!DrawRay)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + ResponseAreaOffset, ResponseAreaSize);
    }
}
