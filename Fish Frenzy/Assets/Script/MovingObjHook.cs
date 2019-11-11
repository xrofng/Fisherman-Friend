using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrofng;

public class MovingObjHook : MovingObject
{
    public PercentFloat forwardTimer = new PercentFloat(0, 0.3f);
    public PercentFloat backwardTimer = new PercentFloat(0, 0.1f);
    protected bool backwarding;
    public Transform trailObject;

    public float stretchDistance = 1;
    public float stretchSpeed;

    private Vector3 startStretchPosition;
    private Vector3 endStretchPosition;

    protected Player hookedPlayer;
    public Player HookedPlayer
    {
        get { return hookedPlayer; }
    }
    protected bool hasHooked;

    [Header("SoundEffect")]
    public SoundEffect sfx_attach;

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        startStretchPosition = this.transform.position;
        endStretchPosition = transform.position + direction * stretchDistance;
        trailObject.parent = null;
    }

    protected override void Update()
    {
        if (!backwarding)
        {
            StretchForward();
        }
        else
        {
            StretchBack();
        }

        if (hookedPlayer)
        {
            hookedPlayer.transform.position = transform.position;
        }
    }

    protected void StretchForward()
    {
        forwardTimer.AddValue(Time.deltaTime);

        transform.position = Vector3.Lerp(startStretchPosition, endStretchPosition, forwardTimer.Ratio);
        trailObject.localScale += Vector3.forward * stretchSpeed;

        if (forwardTimer.IsAtMax)
        {
            SetBackward();
        }
    }

    protected void StretchBack()
    {
        backwardTimer.AddValue(Time.deltaTime);

        transform.position = Vector3.Lerp(endStretchPosition, startStretchPosition, backwardTimer.Ratio);
        trailObject.localScale -= Vector3.forward * stretchSpeed * forwardTimer.MaxVal/backwardTimer.MaxVal;

        if (backwardTimer.IsAtMax)
        {
            MoveEnd = CheckEnd();
            StopHook();
        }
    }

    protected override bool CheckEnd()
    {
        return backwardTimer.IsAtMax;
    }
  

    void StopHook()
    {
        trailObject.parent = this.transform;
        trailObject.gameObject.SetActive(false);
        if (hookedPlayer)
        {
            hookedPlayer.RemoveAbilityInputIntercepter(this);
            hookedPlayer.Rigidbody.velocity = Vector3.zero;
        }
    }

    void StartHook()
    {
        hookedPlayer.AddAbilityInputIntercepter(this);
        hookedPlayer._cPlayerFishInteraction.SetPlayerCollideEverything(false);
        SoundManager.Instance.PlaySound(sfx_attach, transform.position);
    }

    void SetBackward()
    {
        if (!backwarding)
        {
            backwarding = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        hookedPlayer = other.gameObject.GetComponent<Player>();
        if (hookedPlayer != null && !hasHooked && !backwarding)
        {
            hasHooked = true;
            StartHook();
        }
    }

    public override void OnBeforeDestroy()
    {
        base.OnBeforeDestroy();
        StopHook();
    }
}
