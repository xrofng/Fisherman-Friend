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

    public SpriteRenderer playerIndicator; 
    public static float fixedFPS_DT;
    

    public bool Aiming
    {
        get { return _cPlayerThrow.aiming; }
    }

    /// <summary>
    /// Ignore input for all ability if this list is not empty.
    /// </summary>
    private List<object> abilityInputIntercepter = new List<object>();

    public bool IgnoreInputForAbilities
    {
        get { return abilityInputIntercepter.Count > 0; }
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
    [HideInInspector]
    public PlayerSwitchFish _cPlayerSwitch;
    [HideInInspector]
    public PlayerFishInteraction _cPlayerFishInteraction;

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

        playerIndicator.sprite = PortRoyal.Instance.playerIndicator[playerID-1];

        rigid = GetComponent<Rigidbody>();
        rigid.mass = PortRoyal.Instance.characterMass;
        myCollider = GetComponent<BoxCollider>();
        _cPlayerInvincibility = GetComponent<PlayerInvincibility>();
        _cPlayerThrow = GetComponent<PlayerThrow>();
        _cPlayerState = GetComponent<PlayerState>();
        _cPlayerSlap = GetComponent<PlayerSlap>();
        _cPlayerFishing = GetComponent<PlayerFishing>();
        _cPlayerMovement = GetComponent<PlayerMovement>();
        _cPlayerFishInteraction = GetComponent<PlayerFishInteraction>();
        _cPlayerSwitch = GetComponent<PlayerSwitchFish>();
            
    }

    // Update is called once per frame
    void Update() {
        switch (state)
        {
            case eState.ground:
               // checkInput();
                break;
            case eState.fishing:
                break;
        }
    }
    void FixedUpdate() {

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
    public void ChangeState(eState staTE)
    {
        state = staTE;
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

    public void recieveDamage(object intercepter,float damage, Vector3 damageDealerPos, int recoveryFrame, Vector2 knockBackForce)
    {
        StartCoroutine(IgnoreAbilityInput(intercepter, recoveryFrame));
        recieveDamage(damage, damageDealerPos, recoveryFrame, knockBackForce);
    }

    IEnumerator IgnoreAbilityInput(object intercepter , int FreezeFramesOnHitDuration  )
    {
        AddAbilityInputIntercepter(intercepter);
        int frameCount = 0;
        while (frameCount < FreezeFramesOnHitDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        RemoveAbilityInputIntercepter(intercepter);
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
            _cPlayerFishInteraction.FishCollideInteraction(other.gameObject);
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

    public virtual void AddAbilityInputIntercepter(object intercepter)
    {
        abilityInputIntercepter.Add(intercepter);
    }

    public virtual void RemoveAbilityInputIntercepter(object intercepter)
    {
        abilityInputIntercepter.Remove(intercepter);
    }

}
