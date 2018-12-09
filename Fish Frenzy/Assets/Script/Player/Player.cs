using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature {
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

    /// associated input manager

    public JoystickManager _linkedInputManager;
    public JoystickManager LinkedInputManager
    {
        get
        {
            if (_linkedInputManager == null)
            {
                _linkedInputManager = FindObjectOfType<JoystickManager>();
            }
            return _linkedInputManager;
        }
    }

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
    [HideInInspector]
    public PlayerAnimation animator;
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
    [HideInInspector]
    public PlayerSpecial _cPlayerFishSpecial;


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
        body, leftArm, rightArm, hitBox
    }
    public Transform GetPart(ePart p)
    {
        int index = (int)p;
        return part[index];
    }
    public Vector3 playerForward
    {
        get
        {
            return -GetPart(ePart.body).forward;
        }
    }
    public enum eState
    {
        ground,
        air,
        water,
        fishing,
        waitForFish,
        rodSwinging
    }
    public eState state;

    [Header("SFX")]
    public AudioClip sfx_Death;

    [Header("Other Class Ref")]
    protected GameLoop gameLoop;
    protected PortRoyal portRoyal;
    protected KnockData knockData;
    // Use this for initialization
    void Start() {


    }

    public void Initialization()
    {
        gameLoop = FFGameManager.Instance.GameLoop;
        portRoyal = FFGameManager.Instance.PortRoyal;
        knockData = FFGameManager.Instance.KnockData;

        playerID = gameObject.name[6] - 48;
        this.gameObject.layer = LayerMask.NameToLayer("Player" + playerID);
        fixedFPS_DT = 0.016f;
        playerIndicator.sprite = StartupPlayer.Instance.playerIndicator[playerID-1];
        rigid = GetComponent<Rigidbody>();
        rigid.mass = portRoyal.characterMass;
        _collider = GetComponent<BoxCollider>();
        animator = GetComponent<PlayerAnimation>();
        _cPlayerInvincibility = GetComponent<PlayerInvincibility>();
        _cPlayerThrow = GetComponent<PlayerThrow>();
        _cPlayerState = GetComponent<PlayerState>();
        _cPlayerSlap = GetComponent<PlayerSlap>();
        _cPlayerFishing = GetComponent<PlayerFishing>();
        _cPlayerMovement = GetComponent<PlayerMovement>();
        _cPlayerFishInteraction = GetComponent<PlayerFishInteraction>();
        _cPlayerSwitch = GetComponent<PlayerSwitchFish>();
        _cPlayerFishSpecial = GetComponent<PlayerSpecial>();
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

    public void ChangeState(eState staTE)
    {
        state = staTE;
    }

    public void recieveDamage(float damage , GameObject damageDealer, Vector3 damageDealerPos,  int recoveryFrame , bool launchingDamage)
    {
        dPercent += (int)damage;
        //Instantiate(knockBackOrigin, center ,Quaternion.identity);
        Vector2 knockBackForce = knockData.getSlapKnockForce((int)damage, dPercent);

        //print(launchingDamage);

        if (launchingDamage)
        {
            AddKnockBackForce(damage, damageDealerPos, knockBackForce);
        }
        _cPlayerFishing.SetFishing(false);
        _cPlayerInvincibility.startInvincible(recoveryFrame);
        _cPlayerState.ToggleIsDamage();
        MatchResult.Instance.StoreAttacker(playerID, damageDealer);

        DamagePercentClamp();
    }
    public void recieveDamage(float damage, GameObject damageDealer, Vector3 damageDealerPos, int recoveryFrame, bool launchingDamage, float upMultiplier)
    {
        dPercent += (int)damage;
        Vector2 knockBackForce = knockData.getSlapKnockForce((int)damage, dPercent);
        knockBackForce += Vector2.up * knockData.getVerticalKnockForce(dPercent) * upMultiplier;

        if (launchingDamage)
        {
            AddKnockBackForce(damage, damageDealerPos, knockBackForce);
        }
        _cPlayerFishing.SetFishing(false);
        _cPlayerInvincibility.startInvincible(recoveryFrame);
        _cPlayerState.ToggleIsDamage();
        MatchResult.Instance.StoreAttacker(playerID, damageDealer);

        DamagePercentClamp();
    }

    public void recieveDamage(object intercepter,float damage, GameObject damageDealer, Vector3 damageDealerPos, int recoveryFrame, bool launchingDamage)
    {
        StartCoroutine(IgnoreAbilityInput(intercepter, recoveryFrame));
        recieveDamage(damage, damageDealer, damageDealerPos,recoveryFrame,launchingDamage);
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

    IEnumerator Respawn(float waitBeforeRespawn , float waitBeforeCancelInvinc)
    {
        yield return new WaitForSeconds(waitBeforeRespawn);
        rigid.velocity = Vector3.zero;
        this.transform.position = portRoyal.randomSpawnPosition();
        Death = false;
        _cPlayerFishInteraction.SetHoldFish(false);
        this.dPercent = 0;
        MatchResult.Instance.ClearRecentDamager(playerID);
        yield return new WaitForSeconds(waitBeforeCancelInvinc);
        _cPlayerFishInteraction.SetPlayerCollideEverything(true);

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
        if (other.gameObject.tag == "StageEdge" && !Death)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        Death = true;
        _cPlayerFishInteraction.SetPlayerCollideEverything(false);
        PlaySFX(sfx_Death);
        GameObject latest= MatchResult.Instance.GetLatestDamager(playerID,false);
        if (latest)
        {
            MatchResult.Instance.StoreKnocker(playerID, latest);
        }
        else
        {
            MatchResult.Instance.StoreKnocker(playerID, this.gameObject);
        }

        this.transform.position = portRoyal.deathRealm.position;
        Animator.ChangeAnimState((int)PlayerAnimation.State.Idle);
        StartCoroutine(Respawn(portRoyal.respawnTime , portRoyal.respawnTime));
    }

    public Vector3 getLowestPlayerPoint()
    {
        return new Vector3(transform.position.x, transform.position.y - GetCollider<BoxCollider>().size.y / 2.0f, transform.position.z);
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
