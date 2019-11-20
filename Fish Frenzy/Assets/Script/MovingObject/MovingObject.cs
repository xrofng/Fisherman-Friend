using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;
    public Vector3 Direction { get; set; }
    private bool moveEnd;
    public bool MoveEnd
    {
        get { return moveEnd; }
        set { moveEnd = value; }
    }
    private HitBoxMelee hitBox;
    public HitBoxMelee HitBox
    {
        get
        {
            if (!hitBox)
            {
                hitBox = GetComponent<HitBoxMelee>();
            }
            return hitBox;
        }
    }
    // Use this for initialization
    protected virtual void Start ()
    {
        Initialization();

    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
		
	}

    protected virtual void Initialization()
    {
        
    }

    protected virtual bool CheckEnd()
    {
        return false;
    }

    public virtual void OnBeforeDestroy()
    {
    }
}
