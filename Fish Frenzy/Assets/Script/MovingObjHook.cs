using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjHook : MovingObject {

    public float fowardTime = 5.0f;
    protected bool backwarding;
    public float reachDistance = 2.0f;
    public Transform trailObject;

    public float stretchSpeed;
    protected int stretchincrement;
    protected Vector3 startStretchPosition;

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
        stretchincrement = 1;
        trailObject.parent = null;
    }

    protected override void Update()
    {
        Vector3 moveDirection = direction;
        fowardTime -= Time.deltaTime;
        if (fowardTime <= 0)
        {
            SetBackward();
            moveDirection = Vector3.Normalize(HitBox.Owner.gameObject.transform.position - transform.position);
            MoveEnd = CheckEnd();
            stretchincrement = -1;
        }
        transform.Translate(moveDirection * speed,Space.World);
        trailObject.localScale += Vector3.forward * stretchSpeed* stretchincrement;

        if (hookedPlayer)
        {
            hookedPlayer.transform.position = transform.position;
        }
    }

    protected override bool CheckEnd()
    {
        float distance = Vector3.Distance(HitBox.Owner.gameObject.transform.position, transform.position);
        if (distance <= reachDistance)
        {
            StopHook();
            return true;
        }
        return false;
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
        fowardTime = 0;
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
}
