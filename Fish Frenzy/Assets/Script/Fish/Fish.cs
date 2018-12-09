using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Creature {
    public enum fState
    {
        swim=0,
        baited,
        toPlayer,
        hold,
        threw,
        ground,
        kept,
        dehydrate,
        fall
    }
    public GameObject temp;
    [Header("Info")]
    public fState state;
    public GameObject holder;
    private Player _playerHolder;
    public Player GetPlayerHolder
    {
        get
        {
            if (!_playerHolder)
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


    [Header("Fishing")]
    public float durability = 7;
    private float dehydration;
    public float weight;
    public int mashCountDown = 2;
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
    public AudioClip sfx_WaterJump;   [Range(0.0f, 1.0f)]    public float volume_wj; 
    public AudioClip sfx_Slap;[Range(0.0f, 1.0f)]    public float volume_slap;
    public AudioClip sfx_Throw;[Range(0.0f, 1.0f)]    public float volume_throw;
    public AudioClip sfx_Special;[Range(0.0f, 1.0f)]    public float volume_special;

    [Header("Picture")]
    public Sprite fishIcon;
    public Sprite fishName;
    public Sprite fishStored;

    [HideInInspector]
    public FishSpecial _cSpecial;

    [Header("Other Class Ref")]
    protected GameLoop gameLoop;
    protected PortRoyal portRoyal;

    // Use this for initialization
    void Start () {
        Initialization();
    }

    void Initialization()
    {
        _pickupFish = GetComponent<PickupFish>();
        _cSpecial = GetComponent<FishSpecial>();
        dehydration = durability;
        gameLoop = FFGameManager.Instance.GameLoop;
        portRoyal = FFGameManager.Instance.PortRoyal;
    }

    // Update is called once per frame
    void Update () {
        Dehydrate();
        GoInDeepWater();
        CheckJustGround();
        CheckWater();
    }

    public void playerCollideInteraction(GameObject player)
    {
        
    }

    void LeftPlayer()
    {
        if(state != fState.ground)
        {
            transform.parent = null;
            GetPlayerHolder._cPlayerFishInteraction.SetHoldFish(false);
        }
    }

    void Dehydrate()
    {
        if (state == fState.hold || state== fState.ground)
        {
            durability -= Time.deltaTime;
            if (durability <= 0)
            {
                LeftPlayer();
                state = fState.dehydrate;
                JumpToWater();

                _cSpecial.OnDehydrate();
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
                ChangeState(fState.toPlayer);
                return true;
            }
        }
        return false;
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
            state = fState.kept;
        }else
        {
            state = fState.hold;
        }
    }

    public void ChangeState(fState pState)
    {
        OnStateChange(pState);
        state = pState;
    }

    void OnStateChange(fState stateChange)
    {
        if (stateChange == fState.toPlayer)
        {
            direction = holder.transform.position - transform.position;
            FishJump(fishMass, jumpForce, direction, jumpSpeed);
        }
        else if(stateChange == fState.ground)
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

    public void SnapTransform()
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
        if(state == fState.hold)
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

    public void FishThrow(float duration , float forwardMultiplier , float upMultiplier)
    {
        duration = Mathf.Clamp(duration, 0.5f, maxHolding);
        transform.parent = null;
        gameObject.AddComponent<Rigidbody>();
        _rigid = GetComponent<Rigidbody>();
        float scaleToDuration = duration / maxHolding;
        chargePercent = (int)scaleToDuration *100;
        float chargePer = Mathf.Lerp(0.1f, 1, scaleToDuration);
        _rigid.velocity = -transform.forward * -(forwardMultiplier * chargePer) + (transform.up* upMultiplier);
        throwAttack = _cSpecial.attack * scaleToDuration;
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

    public Vector3 getLowestFishPoint()
    {
        return new Vector3(transform.position.x, transform.position.y - (transform.localScale.y *  GetCollider<BoxCollider>().size.y) / 2.0f, transform.position.z);
    }

    void CheckJustGround()
    {
        if (state == fState.threw || state == fState.fall)
        {
            RaycastHit hit;
            if (Physics.Raycast(getLowestFishPoint(), transform.TransformDirection(Vector3.down), out hit, rayDistance ))
            {
                if (_rigid)
                {
                    if (hit.transform.gameObject.tag == "Ground" && _rigid.velocity.y < 0)
                    {
                        ChangeState(fState.ground);
                    }
                }else
                {
                    if (hit.transform.gameObject.tag == "Ground")
                    {
                        ChangeState(fState.ground);
                    }
                }
               
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (state == fState.fall)
        {
            if (other.gameObject.tag == "Ground")
            {
                ChangeState(fState.ground);
            }
           
        }
    }

    void CheckWater()
    {
        if (state == fState.threw || state == fState.dehydrate || state == fState.fall)
        {
            RaycastHit hit;
            if (Physics.Raycast(getLowestFishPoint(), transform.TransformDirection(Vector3.down), out hit, rayDistance))
            {
                if (hit.transform.gameObject.tag == "Sea" && _rigid.velocity.y < 0)
                {
                    GetCollider<BoxCollider>().enabled = false;
                    PlaySFX(sfx_WaterJump);
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
