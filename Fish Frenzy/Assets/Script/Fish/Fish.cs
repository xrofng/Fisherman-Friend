using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Creature
{
    public int fishId;
    public enum FishConditionalState
    {
        swim=0,
        baited,
        toPlayer,
        hold,
        threw,
        ground,
        kept,
        lowDurability,
        dehydrate,
        fall,
    }
    public GameObject temp;
    [Header("Info")]
    public FishConditionalState state;
    public GameObject holder;
    private Player _playerHolder;
    public Player GetPlayerHolder
    {
        get
        {
            if (holder && !_playerHolder)
            {
                _playerHolder = holder.gameObject.GetComponent<Player>();
            }
            return _playerHolder;
        }
    }

    // Ignore Object
    // TODO encap to damage on hit
    public int ignorePlayerFrame;
    protected List<GameObject> _ignoredGameObjects = new List<GameObject>();
    public MeshRenderer fishMeshRenderer;

    [Header("Fishing")]
    public float durability = 7;
    public float lowDurabilityFloor = 2;
    private float dehydration;
    public float weight;
    public int mashCountDown = 2;
    public float spawnRate = 10;
    protected Vector3 direction;
    protected float jumpForce = 10;
    protected float jumpSpeed = 40;
    protected float fishMass =1;
    public float rayDistance = 0.01f;

    [Header("Throw")]
    public float maxHolding = 5;
    public float throwAttack;
    public int t_invicibilityFrame = 50;
    public bool t_launchingDamage;
    public Vector3 lastHoldPoition;
    public int chargePercent;

    [Header("Snap")]
    //snap
    public Vector3 holdPosition;
    public Vector3 holdRotation;
    public Vector3 aimPositioningOffset;
    
    public Collider MyCollider { get { return _collider; } }
    private PickupFish _pickupFish;

    [Header("SFX")]
    public SoundEffect sfx_WaterJump;
    public SoundEffect sfx_Slap;
    public SoundEffect sfx_Throw;
    public SoundEffect sfx_Special;

    public FishHudInfo fishHudInfo; 

    [HideInInspector]
    public FishSpecial _cSpecial;

    [Header("Other Class Ref")]
    protected GameLoop gameLoop;
    protected PortRoyal portRoyal;

    // Use this for initialization
    void Start ()
    {
        Initialization();
    }

    void Initialization()
    {
        _pickupFish = GetComponent<PickupFish>();
        _cSpecial = GetComponent<FishSpecial>();
        dehydration = durability;
        gameLoop = FFGameManager.Instance.GameLoop;
        portRoyal = FFGameManager.Instance.PortRoyal;
        SetMesh(fishMeshRenderer);
        Animation.Animator.SetInteger("FishId", fishId);
    }

    // Update is called once per frame
    void Update ()
    {
        Dehydrate();
        GoInDeepWater();
        CheckJustGround();
        CheckWater();
        UpdateAnimation();
    }

    public void playerCollideInteraction(GameObject player)
    {
        
    }

    void LeftPlayer()
    {
        if(state != FishConditionalState.ground)
        {
            transform.parent = null;
            if (GetPlayerHolder)
            {
                GetPlayerHolder._cPlayerFishInteraction.SetHoldFish(false);
            }
        }
    }

    void Dehydrate()
    {
        if (state == FishConditionalState.hold || state== FishConditionalState.ground || state == FishConditionalState.lowDurability)
        {
            durability -= Time.deltaTime;
            if (durability <= 0 && !_cSpecial.IsPerformingSpecial)
            {
                LeftPlayer();
                state = FishConditionalState.dehydrate;
                _pickupFish.HidePrompt();
                JumpToWater();

                if (_cSpecial)
                {
                    _cSpecial.OnDehydrate();
                }
            }
            else if (durability < lowDurabilityFloor)
            {
                state = FishConditionalState.lowDurability;
            }

        }
    }

    public float GetDurabilityRatio
    {
        get { return durability / dehydration; }
    }

    public bool MashForCatch()
    {
        if (mashCountDown > 0)
        {
            mashCountDown -= 1;
            if (mashCountDown <= 0)
            {
                ChangeState(FishConditionalState.toPlayer);
                return true;
            }
        }
        return false;
    }

    void UpdateAnimation()
    {
        Animation.ChangeAnimState((int)state);
    }


    void GoInDeepWater()
    {
        if (transform.position.y <= portRoyal.underWater.position.y)
        {
            Destroy(this.gameObject);
        }        
    }

    public void KeepFish(bool keep)
    {
        this.gameObject.SetActive(!keep);
        if (keep)
        {
            state = FishConditionalState.kept;
        }else
        {
            state = FishConditionalState.hold;
        }
    }

    public void ChangeState(FishConditionalState pState)
    {
        OnStateChange(pState);
        state = pState;
    }

    void OnStateChange(FishConditionalState stateChange)
    {
        if (stateChange == FishConditionalState.toPlayer)
        {
            direction = holder.transform.position - transform.position;
            FishJump(fishMass, jumpForce, direction, jumpSpeed);
        }
        else if(stateChange == FishConditionalState.ground)
        {
            gameObject.layer = LayerMask.NameToLayer("Fish_All");
            SetToGround(true);
            RemoveRigidBody();
        }
    }

    public void fishBounce()
    {
        StartCoroutine("ieFishBounce");
    }


    private IEnumerator  ieFishBounce()
    {
        yield return new WaitForSeconds(0.0f);
        Rigidbody.AddForce(Vector3.up * 3,ForceMode.Impulse);
    }

    public void SnapToHold()
    {
        transform.localPosition = holdPosition;
        transform.localEulerAngles = holdRotation;
    }

    public void SnapAimingTransform()
    {
        transform.position += aimPositioningOffset;
    }

    public void SetHolder(GameObject g)
    {
        holder = g;
    }

    public void FishJump(float m, float f, Vector3 d,float speed)
    {
        gameObject.AddComponent<Rigidbody>();
        _rigid = GetComponent<Rigidbody>();
        _rigid.mass = m;
        d.y = f;
        _rigid.AddForce(d*speed);
    }

    void JumpToWater()
    {
        if(state == FishConditionalState.hold)
        {
            GetPlayerHolder._cPlayerFishInteraction.SetMainFishTransformAsPart(Player.ePart.body, Player.ePart.body, false);
        }
        
        Vector3 nearest = FindNearestWater();
        nearest = new Vector3(nearest.x, this.transform.position.y, nearest.z);
        transform.LookAt(nearest);
        gameObject.AddComponent<Rigidbody>();
        _rigid = GetComponent<Rigidbody>();
        _rigid.velocity = -transform.forward * -(portRoyal.FishJumpToWaterMultiplier.x) + (transform.up * portRoyal.FishJumpToWaterMultiplier.y);
        _collider.enabled = false;
    }

    public void FishThrow(float duration , float minForwardMultiplier, float maxForwardMultiplier, float upMultiplier)
    {
        duration = Mathf.Clamp(duration, 0.5f, maxHolding);
        transform.parent = null;
        gameObject.AddComponent<Rigidbody>();
        _rigid = GetComponent<Rigidbody>();
        float scaleToDuration = duration / maxHolding;
        chargePercent = (int)scaleToDuration *100;
        float chargePer = Mathf.Lerp(0.1f, 1, scaleToDuration);
        float forwardMultiplier = minForwardMultiplier + (maxForwardMultiplier - minForwardMultiplier)*chargePer;
        _rigid.velocity = -transform.forward * -(  forwardMultiplier) + (transform.up* upMultiplier);
        throwAttack = _cSpecial.damage.damage * scaleToDuration;
    }

    public void RemoveRigidBody()
    {
        Destroy(_rigid);
    }
   
 
    Vector3 FindNearestWater()
    {
        RaycastHit hit;
        int rayFrequnecy = 8;
        int maxRadiusT = 8;
        int radiusLambda = 2;
        Vector3 positionCheck = this.transform.position;

        for(int i = 1; i < maxRadiusT; i++)
        {
            positionCheck = this.transform.position;
            float radiusPerLine = ( 360.0f / rayFrequnecy )* Mathf.PI / 180.0f;
            float radius =  Random.Range(0,361) * Mathf.PI / 180.0f;
            for (int j = 0; j < rayFrequnecy; j++)
            {
                positionCheck = new Vector3(transform.position.x + Mathf.Cos(radius) * (i*2), transform.position.y, transform.position.z + Mathf.Sin(radius) * (i * radiusLambda));
                radius += radiusPerLine;

                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(positionCheck, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
                {
                    Color lineColor = Color.yellow;
                    if (hit.transform.gameObject.tag == "Sea")
                    {
                        lineColor = Color.cyan;
                        return hit.point;
                    }
                }
            }
            rayFrequnecy *= 2;
        }
        Debug.LogWarning("Don't found sea in this max radius =" + maxRadiusT + " Distance = " + radiusLambda);
        return Vector3.zero;
    }

    public Vector3 GetLowestFishPoint()
    {
        return new Vector3(transform.position.x, transform.position.y - (transform.localScale.y *  GetCollider<BoxCollider>().size.y) / 2.0f, transform.position.z);
    }

    void CheckJustGround()
    {
        if (state == FishConditionalState.threw || state == FishConditionalState.fall)
        {
            RaycastHit hit;
            if (Physics.Raycast(GetLowestFishPoint(), transform.TransformDirection(Vector3.down), out hit, rayDistance ))
            {
                if (_rigid)
                {
                    if (hit.transform.gameObject.tag == "Ground" && _rigid.velocity.y < 0)
                    {
                        ChangeState(FishConditionalState.ground);
                    }
                }
                else
                {
                    if (hit.transform.gameObject.tag == "Ground")
                    {
                        ChangeState(FishConditionalState.ground);
                    }
                }
               
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (state == FishConditionalState.fall)
        {
            if (other.gameObject.tag == "Ground")
            {
                ChangeState(FishConditionalState.ground);
            }
           
        }
    }

    void CheckWater()
    {
        if (state == FishConditionalState.threw || state == FishConditionalState.dehydrate || state == FishConditionalState.fall)
        {
            RaycastHit hit;
            if (Physics.Raycast(GetLowestFishPoint(), transform.TransformDirection(Vector3.down), out hit, rayDistance))
            {
                if (hit.transform.gameObject.tag == "Sea" && _rigid.velocity.y < 0)
                {
                    state = FishConditionalState.swim;
                    GetCollider<BoxCollider>().enabled = false;
                    SoundManager.Instance.PlaySound(sfx_WaterJump,transform.position);
                }
            }
        }
    }

    public void SetToGround(bool b)
    {
        _pickupFish.SetAllowToPick(b);
    }

    /// <summary>
    /// Adds the gameobject set in parameters to the ignore list
    /// </summary>
    /// <param name="newIgnoredGameObject">New ignored game object.</param>
    public virtual void AddIgnoreGameObject(GameObject newIgnoredGameObject)
    {
        StartCoroutine(ieAddIgnoreGameObject(newIgnoredGameObject));
    }

    IEnumerator ieAddIgnoreGameObject(GameObject newIgnoredGameObject)
    {
        _ignoredGameObjects.Add(newIgnoredGameObject);
        int frameCount = 0;
        while (frameCount < ignorePlayerFrame)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        _ignoredGameObjects.Remove(newIgnoredGameObject);
    }

    public bool CheckIgnoredObject(GameObject go)
    {
        return _ignoredGameObjects.Contains(go);
    }
}
