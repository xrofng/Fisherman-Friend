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
    public Vector3 speed;
    public Vector3 jumpForce;
    public float jumpFaster;
    public float fallFaster;

    public bool freezeMovement;

    public static float fixedFPS_DT;
    private Rigidbody rigid;
    public bool nearCoast;
    private bool Aiming
    {
        get { return _cPlayerThrow.aiming; }
    }
    public bool holdingFish;
    public Fish mainFish;
    public Fish subFish;
    public Fish baitedFish;

    private Vector3 lookTo;
    public Transform fishPoint_finder;
    public Transform fishPoint;

    // Other Component
    private BoxCollider myCollider;
    [HideInInspector]
    public PlayerInvincibility _cPlayerInvincibility;
    [HideInInspector]
    public PlayerThrow _cPlayerThrow;
    [HideInInspector]
    public PlayerState _cPlayerState;
    [HideInInspector]
    public PlayerSlap _cPlayerSlap;

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
        speed = PortRoyal.Instance.speed;
        jumpForce = PortRoyal.Instance.jumpForce;
        fallFaster = PortRoyal.Instance.fallFaster;
        jumpFaster = PortRoyal.Instance.jumpFaster;

        rigid = GetComponent<Rigidbody>();
        rigid.mass = PortRoyal.Instance.characterMass;
        myCollider = GetComponent<BoxCollider>();
        _cPlayerInvincibility = GetComponent<PlayerInvincibility>();
        _cPlayerThrow = GetComponent<PlayerThrow>();
        _cPlayerState = GetComponent<PlayerState>();
        _cPlayerSlap = GetComponent<PlayerSlap>();
    }

    // Update is called once per frame
    void Update() {
        switch (state)
        {
            case eState.ground:
                move();
                coastCheck();
                switchFish();
                startFishing();
                //checkInput();
                break;
            case eState.fishing:
                startFishing();
                break;
        }
        if (playerID == 2 && rigid.velocity != Vector3.zero)
        {
            // print(rigid.velocity);
        }
    }
    void FixedUpdate() {
    }

    void move()
    {
        string hori = "Hori" + playerID;
        string verti = "Verti" + playerID;
        Vector3 mov = new Vector3(Input.GetAxisRaw(hori) * speed.x, 0.0f, Input.GetAxisRaw(verti) * speed.z);
        mov = mov * Time.deltaTime;
        if (!freezeMovement && !_cPlayerState.IsAttacking)
        {
            this.transform.Translate(mov);
        }

        float axisRawX = Input.GetAxisRaw(hori);
        float axisRawY = Input.GetAxisRaw(verti);
        Vector3 playerDirection = lookTo;
        if (sClass.getSign(Input.GetAxis(hori), 0.015f) != 0 ||sClass.getSign(Input.GetAxis(verti), 0.015f) != 0)
        {
            if (sClass.intervalCheck(axisRawX, -0.9f, 0.9f, true) || sClass.intervalCheck(axisRawY, -0.9f, 0.9f, true))
            {
                playerDirection = Vector3.right * -axisRawX + Vector3.forward * -axisRawY;
                lookTo = playerDirection;
            }
        }
        
        if (playerDirection.sqrMagnitude > 0.0f && !Aiming)
        {
            getPart(ePart.body).transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        }

        string jump_b = "Jump" + playerID;
        if (Input.GetButtonDown(jump_b) && rigid.velocity.y<=0)
        {
            rigid.velocity = Vector3.zero;
            rigid.AddForce(jumpForce, ForceMode.Impulse);
            rigid.drag = jumpFaster;
        }
        if (rigid.velocity.y < 0)
        {
            rigid.velocity += Vector3.up * Physics.gravity.y * fallFaster * Time.deltaTime;
            rigid.drag = 0;
        }
    }
    bool isOwnerFish(Fish f)
    {
        return this.gameObject.name == f.holder.gameObject.name;
    }
    
    public void SetTransformAsPart(ePart transPart, ePart rotatPart)
    {
        mainFish.transform.position = getPart(transPart).transform.position;
        mainFish.transform.rotation = getPart(rotatPart).transform.rotation;
    }

    public void SetHoldingFish(bool b)
    {
        holdingFish = b;
    }

    public void SetFishCollidePlayer(Fish fish , Player player, bool collide)
    {
        string layerN = "FishO";
        if (!collide)
        {
            layerN = "Fish";
        }
        fish.gameObject.layer = LayerMask.NameToLayer(layerN + playerID);
    }
    void coastCheck()
    {
        RaycastHit hit;
        nearCoast = false;
       
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(fishPoint_finder.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Color lineColor = Color.yellow;
            if (hit.transform.gameObject.tag == "Sea" && !holdingFish && _cPlayerState.IsGrounded && !_cPlayerState.IsDeath )
            {
                lineColor = Color.blue;
                nearCoast = true;
                fishPoint.position = hit.point + Vector3.down;
                GUIManager.Instance.UpdateFishButtonIndicator(playerID, fishPoint.position,true);
            }
            else
            {
                GUIManager.Instance.UpdateFishButtonIndicator(playerID, fishPoint.position, false);
            }
            Debug.DrawRay(fishPoint_finder.position, transform.TransformDirection(Vector3.down) * hit.distance, lineColor);
        }
        else
        {
            Debug.DrawRay(fishPoint_finder.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
        }
        
    }
   
    void startFishing()
    {
        string fishi = "Fishing" + playerID;
        if (Input.GetButtonDown(fishi))
        {
            switch (state)
            {
                case eState.ground:
                    if (nearCoast == true && !holdingFish)
                    {
                        baitedFish = Instantiate(PortRoyal.Instance.randomFish(), fishPoint.position, getPart(ePart.body).transform.rotation);
                        SetFishCollidePlayer(baitedFish, this, true);
                        baitedFish.setHolder(this.gameObject);
                        GUIManager.Instance.UpdateMashFishingButtonIndicator(playerID, fishPoint.position, true);
                        changeState(eState.fishing);
                        baitedFish.changeState(Fish.fState.baited);
                    }
                    break;
                case eState.fishing:
                    if(baitedFish.MashForCatch())
                    {
                        changeState(eState.waitForFish);
                    }
                    break;
                case eState.waitForFish:
                   
                    break;
            }
        }
    }

    void switchFish()
    {
        string switc = "Switch" + playerID;
        if (Input.GetButtonDown(switc))
        {
            
            baitedFish = subFish;
            subFish = mainFish;
            if (subFish != null) { subFish.gameObject.SetActive(false); }
            
            mainFish = baitedFish;
            baitedFish = null;
            holdingFish = false;
            if (mainFish != null)
            {
                holdingFish = true;
                mainFish.gameObject.SetActive(true);
            }
        }
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

            case eState.waitForFish: GUIManager.Instance.UpdateMashFishingButtonIndicator(playerID, fishPoint.position, false);  state = eState.waitForFish; break;
        }

    }

    public void recieveDamage(float damage , Vector3 damageDealerPos , int recoveryFrame , Vector2 knockBackForce)
    {
      
        dPercent += (int)damage;
        //Instantiate(knockBackOrigin, center ,Quaternion.identity);
        AddKnockBackForce(damage, damageDealerPos , knockBackForce);
        //rigid.AddExplosionForce(dPercent, center, 1.0f, 5.0f, ForceMode.Impulse);
        _cPlayerInvincibility.startInvincible(recoveryFrame);
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
        switch ((int)f.state)
        {
            case 0:
                break;

            case 1:
                break;
            case 2:
                f.changeState(Fish.fState.hold);
                f.gameObject.transform.parent = getPart(ePart.rightArm).transform;
                f.snapTransform();
                f.removeRigidBody();
                
                mainFish = f;
                baitedFish = null;
                holdingFish = true;
                state = eState.ground;
                rigid.velocity = Vector3.zero;
                break;

            case 3:
                break;

            case 4:
                if( !isOwnerFish(f) &&!f.damageDealed)
                {
                    rigid.velocity = Vector3.zero;
                    f.removeRigidBody();
                    f.damageDealed  = true;
                    recieveDamage(f.throwAttack, f.lastHoldPoition , f.t_invicibilityFrame , KnockData.Instance.getThrowKnockForce(f.chargePercent, dPercent));
                    f.fishBounce();
                }
                break;
            case 5:
                break;
        }
    }
    IEnumerator respawn(float waitBeforeRespawn)
    {
       
        yield return new WaitForSeconds(waitBeforeRespawn);
        rigid.velocity = Vector3.zero;
        this.transform.position = PortRoyal.Instance.randomSpawnPosition();
        Death = false;
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
