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

    private Rigidbody myRigid;
    private BoxCollider myCollider;
    // Use this for initialization
    void Start () {
        myCollider = GetComponent<BoxCollider>();
        dehydration = durability;
    }
	
	// Update is called once per frame
	void Update () {

         Dehydrate();
        GoInDeepWater();
    }
    public void playerCollideInteraction(GameObject player)
    {
        
    }

    void Dehydrate()
    {
        if (state == fState.hold)
        {
            durability -= Time.deltaTime;
            if (durability <= 0)
            {
                state = fState.dehydrate;
                JumpToWater();
                GetPlayerHolder.SetHoldFish(false);
            }
        }
    }

    public float GetDurabilityRation
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


    public void snapTransform()
    {
        transform.localPosition = holdPosition;
        transform.localEulerAngles = holdRotation;
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
        GetPlayerHolder.SetMainFishTransformAsPart(Player.ePart.body, Player.ePart.body , false);
        transform.parent = null;
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
        transform.parent = null;
        gameObject.AddComponent<Rigidbody>();
        myRigid = GetComponent<Rigidbody>();
        float scaleToDuration = duration / PortRoyal.Instance.maxHoldToThrow;
        chargePercent =  (int)(scaleToDuration * 100.0f);
        myRigid.velocity = -transform.forward * -(forwardMultiplier * scaleToDuration) + (transform.up* upMultiplier);
        throwAttack = attack * scaleToDuration;
    }

    public void removeRigidBody()
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
        Vector3 positionCheck = this.transform.position;

        for(int i = 1; i < maxRadiusT; i++)
        {
            positionCheck = this.transform.position;
            float radiusPerLine = ( 360.0f / rayFrequnecy )* Mathf.PI / 180.0f;
            float radius =  Random.Range(0,361) * Mathf.PI / 180.0f;
            for (int j = 0; j < rayFrequnecy; j++)
            {
                positionCheck = new Vector3(transform.position.x + Mathf.Cos(radius) * (i*2), transform.position.y, transform.position.z + Mathf.Sin(radius) * (i * 2));
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
        Debug.LogWarning("Don't found sea in this max radius");
        return Vector3.zero;
    }


    void OnCollisionEnter(Collision other)
    {
       

    }
}
