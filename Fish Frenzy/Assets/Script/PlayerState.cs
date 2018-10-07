using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : PlayerAbility
{
    /// is the character colliding right ?
    public bool IsCollidingRight { get; set; }
    /// is the character colliding left ?
    public bool IsCollidingLeft { get; set; }
    /// is the character colliding with something above it ?
    public bool IsCollidingAbove { get; set; }
    /// is the character colliding with something above it ?
    public bool IsCollidingBelow { get; set; }
    /// is the character colliding with anything ?
    public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; } }

    /// Is the character grounded ? 
    public bool IsGrounded { get { return IsCollidingBelow; } }
    /// Is the character daeth ? 
    public bool IsDeath { get; set; }
    /// Is the character attacking ? 
    public bool IsAttacking { get { return _player._cPlayerSlap.Attacking;  }  }
    /// is the character falling right now ?
    public bool IsFalling { get; set; }
    /// is the character falling right now ?
    public bool IsJumping { get; set; }
    /// was the character grounded last frame ?
    public bool WasGroundedLastFrame { get; set; }
    /// was the character grounded last frame ?
    public bool WasTouchingTheCeilingLastFrame { get; set; }
    /// did the character just become grounded ?
    public bool JustGotGrounded { get; set; }

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        Reset();

    }
  
    public virtual void Reset()
    {
        IsCollidingLeft = false;
        IsCollidingRight = false;
        IsCollidingAbove = false;
     
        IsFalling = true;
        IsJumping = false;

    }
    // Update is called once per frame
    void Update () {
        CheckGround();
	}

    /// <summary>
    /// Every frame, we cast a number of rays below our character to check for platform collisions
    /// </summary>
    void CheckGround()
    {
        RaycastHit hit;
        bool hitBelow = false;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_player.getLowestPlayerPoint(), transform.TransformDirection(Vector3.down), out hit, 1.0f))
        {
            Color lineColor = Color.yellow;
            if (hit.transform.gameObject.tag == "Ground")
            {
                hitBelow = true;
            }
        }
        IsCollidingBelow = hitBelow;
    }
}
