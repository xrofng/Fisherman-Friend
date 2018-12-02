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
    public bool IsGrounded { get; set; }
    /// Is the character daeth ? 
    public bool IsDeath { get; set; }
    /// Is the character get hurt recently
    public bool IsDamaged { get; set; }
    public int damagedFrameDuration = 10;
    /// Is the character swim ? 
    public bool IsSwiming { get; set; }
    protected bool prevIsSwiming;

    /// Is the character attacking ? 
    public bool IsAttacking { get { return GetCrossZComponent<PlayerSlap>().Attacking ||
                                            GetCrossZComponent<PlayerFishSpecial>().MeleeSpecialing;  }  }
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

    [Header("SFX")]
    public AudioClip sfx_WaterJump;

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
        IsGrounded = false;
        IsSwiming = false;
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

        UpdatePrevState();

        Reset();
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_player.getLowestPlayerPoint(), transform.TransformDirection(Vector3.down), out hit, 0.5f))
        {
            Color lineColor = Color.yellow;
            if(hit.transform != null)
            {
                hitBelow = true;
            }

            // Both Tag share same Action
            if(hit.transform.gameObject.tag == "Sea" ||
                hit.transform.gameObject.tag == "Ground")
            {
                CheckFinishJumping();
            }

            // Each Tag has different Action
            if (hit.transform.gameObject.tag == "Sea")
            {
                if (!prevIsSwiming)
                {
                    PlaySFX(sfx_WaterJump);
                }
                IsSwiming = true;

            }
            if (hit.transform.gameObject.tag == "Ground")
            {
                IsGrounded = true;
            }
            
        }
        IsCollidingBelow = hitBelow;
    }

    public void CheckFinishJumping(){
        
        if (IsJumping && _pRigid.velocity.y <= 0)
        {
            IsJumping = false;
        }
    }

    void UpdatePrevState()
    {
        prevIsSwiming = IsSwiming; ;
    }

    public void ToggleIsDamage()
    {
        StartCoroutine(ToggleIsDamageForFrame(damagedFrameDuration));
    }

    IEnumerator ToggleIsDamageForFrame(int frameDuration)
    {
        IsDamaged = true;
        int frameCount = 0;
        if (_player.holdingFish)
        {
            _pAnimator.ChangeAnimState((int)PlayerAnimation.State.Damaged_HoldFish);
        }
        else
        {
            _pAnimator.ChangeAnimState((int)PlayerAnimation.State.Damaged);
        }
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount += 1;
        }
        IsDamaged = false;
        if (_player.holdingFish)
        {
            _pAnimator.ChangeAnimState((int)PlayerAnimation.State.HoldFish);
        }
        else
        {
            _pAnimator.ChangeAnimState((int)PlayerAnimation.State.Idle);
        }
    }


}
