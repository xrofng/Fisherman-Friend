using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float testForce;
    public float upLaunchingMultiplier;
    public int playerID;
    public int dPercent;
    /// Is the character daeth ? 
    public bool Death { get { return _cPlayerState.IsDeath; } set { _cPlayerState.IsDeath = value; } }
    

    public static float fixedFPS_DT;
    

    public bool Aiming
    {
        get { return _cPlayerThrow.aiming; }
    }
    public bool FreezingMovement
    {
        get { return _cPlayerMovement.freezeMovement; }
        set { _cPlayerMovement.freezeMovement = value; }
    }
    public bool holdingFish;
    public Fish mainFish;
    public Fish subFish;
    public Fish baitedFish;


    // Other Component
    [HideInInspector]
    public Rigidbody rigid;
    private BoxCollider myCollider;
    [HideInInspector]
    public PlayerInvincibility _cPlayerInvincibility;
    [HideInInspector]
    public PlayerThrow _cPlayerThrow;
    [HideInInspector]
    public PlayerState _cPlayerState;
    [HideInInspector]
    public PlayerSlap _cPlayerSlap;
    [HideInInspector]
    public PlayerFishing _cPlayerFishing;
    [HideInInspector]
    public PlayerMovement _cPlayerMovement;

    public GameObject knockBackOrigin;
    public bool IsInvincible
    {
        get
        {
            return _cPlayerInvincibility.IsInvincible;
        }
    }
    public Transform[] part;
    public enum ePart
    {
        body, leftArm, rightArm
    }
    public Transform getPart(ePart p)
    {
        int index = (int)p;
        return part[index];
    }
    public Vector3 playerForward
    {
        get
        {
            return -getPart(ePart.body).forward;
        }
    }
    public enum eState
    {
        ground,
        air,
        water,
        fishing,
        waitForFish
    }
    public eState state;
    // Use this for initialization
    void Start() {


    }

    public void Initialization()
    {
        playerID = gameObject.name[6] - 48;
        this.gameObject.layer = LayerMask.NameToLayer("Player" + playerID);
        fixedFPS_DT = 0.016f;
       

        rigid = GetComponent<Rigidbody>();
        rigid.mass = PortRoyal.Instance.characterMass;
        myCollider = GetComponent<BoxCollider>();
        _cPlayerInvincibility = GetComponent<PlayerInvincibility>();
        _cPlayerThrow = GetComponent<PlayerThrow>();
        _cPlayerState = GetComponent<PlayerState>();
        _cPlayerSlap = GetComponent<PlayerSlap>();
        _cPlayerFishing = GetComponent<PlayerFishing>();
        _cPlayerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update() {
        switch (state)
        {
            case eState.ground:
                switchFish();
               // checkInput();
                break;
            case eState.fishing:
                break;
        }
    }
    void FixedUpdate() {
    }

    
    bool isOwnerFish(Fish f)
    {
        return this.gameObject.name == f.holder.gameObject.name;
    }
    
    public void SetMainFishTransformAsPart(ePart transPart, ePart rotatPart , bool flipY)
    {
        mainFish.transform.position = getPart(transPart).transform.position;
        mainFish.transform.rotation = getPart(rotatPart).transform.rotation;
        if (flipY)
        {
            mainFish.transform.Rotate(0, 180, 0);
        }
    }

    public void SetHoldFish(bool b)
    {
        _cPlayerThrow.ChangeToUnAim();
        holdingFish = b;
        mainFish = null;
    }


    public void SetFishCollidePlayer(Fish fish, Player player, bool collide)
    {
        string layerN = "FishO";
        if (!collide)
        {
            layerN = "Fish";
        }
        fish.gameObject.layer = LayerMask.NameToLayer(layerN + playerID);
    }


    void switchFish()
    {
        string switc = "Switch" + playerID;
        if (Input.GetButtonDown(switc))
        {
            
            baitedFish = subFish;
            subFish = mainFish;
            if (subFish != null) { subFish.KeepFish(true); }
            
            mainFish = baitedFish;
            baitedFish = null;
            holdingFish = false;
            if (mainFish != null)
            {
                holdingFish = true;
                mainFish.KeepFish(false);
            }
        }
    }
    
    public bool GetOneButtonsPress(string[] button)
    {
        for (int i = 0; i < button.Length; i++)
        {
            string but = button[i] + "" + playerID;
            if (Input.GetButtonDown(but))
            {
                return true;
            }
        }
        return false;
    }
    
    void checkInput()
    {
        string[] button = { "Fishing", "Switch", "Jump","Slap","Throw" };
        int numPlayer = 4;
        for (int i = 0; i < button.Length; i++)
        {
            for (int j = 1; j < numPlayer+1; j++)
            {
                string bitton = button[i] + j;
                if (Input.GetButtonDown(bitton))
                {
                    print(bitton);
                }
            }
        }
    }
    public void changeState(eState staTE)
    {

        switch (staTE)
        {
            case eState.ground:  state = eState.ground; break;

            case eState.air:  state = eState.air; break;

            case eState.water:  state = eState.water;  break;

            case eState.fishing: state = eState.fishing; break;

            case eState.waitForFish:   state = eState.waitForFish; break;
        }

    }

    public void recieveDamage(float damage , Vector3 damageDealerPos , int recoveryFrame , Vector2 knockBackForce)
    {
        dPercent += (int)damage;
        //Instantiate(knockBackOrigin, center ,Quaternion.identity);
        AddKnockBackForce(damage, damageDealerPos , knockBackForce);
        _cPlayerFishing.SetFishing(false);
        _cPlayerInvincibility.startInvincible(recoveryFrame);
        _cPlayerState.ToggleIsDamage();
        DamagePercentClamp();
    }
   
    public void DamagePercentClamp()
    {
        dPercent = Mathf.Clamp(dPercent, 0, 999);
    }

    public void AddKnockBackForce( float damge ,Vector3 forceSourcePos, Vector2 knockBackForce)
    {
        Vector3 knockBackDirection = this.transform.position - forceSourcePos;
        Vector3 nKnockBackDirection = Vector3.Normalize(knockBackDirection);
        Vector3 upLaunching = Vector3.up * knockBackForce.y;
        rigid.AddForce(nKnockBackDirection * knockBackForce.x + upLaunching, ForceMode.Impulse);
    }

    void fishCollideInteraction(GameObject g)
    {
        Fish f = g.GetComponent<Fish>(); 
        switch (f.state)
        {
            case Fish.fState.swim:
                break;

            case Fish.fState.baited:
                break;
            case Fish.fState.toPlayer:
                HoldThatFish(f);

                break;

            case Fish.fState.hold:
                break;

            case Fish.fState.threw:
                if( !isOwnerFish(f) &&!f.damageDealed)
                {
                    rigid.velocity = Vector3.zero;
                    f.RemoveRigidBody();
                    f.damageDealed  = true;
                    recieveDamage(f.throwAttack, f.lastHoldPoition , f.t_invicibilityFrame , KnockData.Instance.getThrowKnockForce(f.chargePercent, dPercent));
                    f.fishBounce();
                }
                break;
            case Fish.fState.ground:
                break;
        }
    }

    public void HoldThatFish(Fish f)
    {
        f.changeState(Fish.fState.hold);
        f.gameObject.transform.parent = getPart(ePart.rightArm).transform;
        f.SnapTransform();
        f.RemoveRigidBody();
        f.SetToGround(false);
        SetFishCollidePlayer(f, this, true);
        mainFish = f;
        baitedFish = null;
        holdingFish = true;
        _cPlayerFishing.SetFishing(false);
        rigid.velocity = Vector3.zero;
    }

    IEnumerator respawn(float waitBeforeRespawn)
    {
       
        yield return new WaitForSeconds(waitBeforeRespawn);
        rigid.velocity = Vector3.zero;
        this.transform.position = PortRoyal.Instance.randomSpawnPosition();
        Death = false;
        holdingFish = false;
        this.dPercent = 0;

    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Fish")
        {
            fishCollideInteraction(other.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StageEdge")
        {
            Death = true;
            this.transform.position = PortRoyal.Instance.deathRealm.position;
            StartCoroutine(respawn(PortRoyal.Instance.respawnTime));
        }
    }

    public Vector3 getLowestPlayerPoint()
    {
        return new Vector3(transform.position.x, transform.position.y - myCollider.size.y / 2.0f, transform.position.z);
    }

   
}
