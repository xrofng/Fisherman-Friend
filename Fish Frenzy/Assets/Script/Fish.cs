using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
    public enum fState
    {
        swim=0,
        baited,
        toPlayer,
        hold,
        threw,
        ground,
        kept,
        dehydrate
    }
    public GameObject temp;
    [Header("Info")]
    public fState state;
    public GameObject holder;
    private Player _playerHolder;
    protected Player GetPlayerHolder
    {
        get
        {
            if (!_playerHolder)
            {
                _playerHolder = holder.GetComponent<Player>();
            }
            return _playerHolder;
        }
    }
    public bool damageDealed;

    [Header("Fishing")]
    public float durability = 7;
    private float dehydration;
    public float weight;
    public int mashCountDown = 2;
    protected Vector3 direction;
    protected float jumpForce = 10;
    protected float jumpSpeed = 40;
    protected float fishMass =1;
    [Header("Throw")]
    public float maxHolding = 5;
    public float throwAttack;
    public int t_invicibilityFrame = 50;
    public Vector3 lastHoldPoition;
    public int chargePercent;

    [Header("Slap")]
    public float attack;
    public float attackSpeed;
    public int hitBoxStayFrame = 4;
    public int s_invicibilityFrame = 50;
    public Vector3 hitboxSize;
    public Vector3 hitboxCenter;



    [Header("Snap")]
    //snap
    public Vector3 holdPosition;
    public Vector3 holdRotation;
    public Vector3 aimPositioningOffset;

    private Rigidbody myRigid;
    private BoxCollider myCollider;
    public BoxCollider MyCollider { get { return myCollider; } }
    private PickupFish _pickupFish;
    // Use this for initialization
    void Start () {
        myCollider = GetComponent<BoxCollider>();
        _pickupFish = GetComponent<PickupFish>();
        dehydration = durability;
    }
	
	// Update is called once per frame
	void Update () {
        Dehydrate();
        GoInDeepWater();
        CheckGround();
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
                changeState(fState.toPlayer);
                return true;
            }
        }
        return false;
    }

    void GoInDeepWater()
    {
        if (transform.position.y <= PortRoyal.Instance.underWater.position.y)
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

    public void changeState(fState pState)
    {
        OnStateChange(pState);
        state = pState;
    }

    void OnStateChange(fState pState)
    {
        if (pState == fState.toPlayer)
        {
            direction = holder.transform.position - transform.position;
            FishJump(fishMass, jumpForce, direction, jumpSpeed);
        }
    }

    public void fishBounce()
    {
        StartCoroutine("ieFishBounce");
    }


    private IEnumerator  ieFishBounce()
    {
        yield return new WaitForSeconds(0.0f);
        myRigid = gameObject.AddComponent<Rigidbody>();
        myRigid.AddForce(Vector3.up * 3,ForceMode.Impulse);
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

    public void setHolder(GameObject g)
    {
        holder = g;
    }


    public void FishJump(float m, float f, Vector3 d,float speed)
    {
        gameObject.AddComponent<Rigidbody>();
        myRigid = GetComponent<Rigidbody>();
        myRigid.mass = m;
        d.y = f;
        myRigid.AddForce(d*speed);
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
        myRigid = GetComponent<Rigidbody>();
        myRigid.velocity = -transform.forward * -(PortRoyal.Instance.FishJumpToWaterMultiplier.x) + (transform.up * PortRoyal.Instance.FishJumpToWaterMultiplier.y);
        myCollider.enabled = false;
    }

    public void FishThrow(float duration , float forwardMultiplier , float upMultiplier)
    {
        duration = Mathf.Clamp(duration, 0.5f, maxHolding);
        transform.parent = null;
        gameObject.AddComponent<Rigidbody>();
        myRigid = GetComponent<Rigidbody>();
        float scaleToDuration = duration / maxHolding;
        chargePercent =  (int)(scaleToDuration * 100.0f);
        myRigid.velocity = -transform.forward * -(forwardMultiplier * scaleToDuration) + (transform.up* upMultiplier);
        throwAttack = attack * scaleToDuration;
    }

    public void RemoveRigidBody()
    {
        Destroy(myRigid);
    }
    public BoxCollider getCollider()
    {
        return myCollider;
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
        return new Vector3(transform.position.x, transform.position.y - myCollider.center.y/2.0f - myCollider.size.y / 2.0f, transform.position.z);
    }

    void CheckGround()
    {
        if( state == fState.threw)
        {
            RaycastHit hit;
            if (Physics.Raycast(getLowestFishPoint(), transform.TransformDirection(Vector3.down), out hit, 0.5f))
            {
                Color lineColor = Color.yellow;
                if (hit.transform.gameObject.tag == "Ground")
                {
                    gameObject.layer = LayerMask.NameToLayer("Fish");
                    state = fState.ground;
                    SetToGround(true);
                    RemoveRigidBody();
                }
            }
        }
        
    }

    public void SetToGround(bool b)
    {
        _pickupFish.SetAllowToPick(b);
    }

    void OnCollisionEnter(Collision other)
    {
       

    }


}
