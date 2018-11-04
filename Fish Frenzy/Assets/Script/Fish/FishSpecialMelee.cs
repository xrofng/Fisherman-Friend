using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialMelee : FishSpecial {

    protected HitBoxMelee playerHitBox;
    public int invicibilityFrame = 20;
    public int freezeFrame = 10;
    public int damage = 50;

    protected PlayerFishSpecial _playerFishSpecial
    {
        get { return _fish.GetPlayerHolder._cPlayerFishSpecial; }
    }
    
    
    protected Vector3 snapPosition;
    protected Vector3 snapRotation;
    protected Vector3 snapScale;

    [Header("Prefab Ref")]
    public Transform hitBoxRef;
    public Transform trailAnimRef;

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
    }

    public void SetUpSpecialMelee()
    {
        SetUpSpecialHitBox();
        SetUpSpecialTrailAnim();
    }

    public void SetUpSpecialHitBox()
    {
        SetSnapFromRef(hitBoxRef.transform);

        if (_playerFishSpecial.specialHitBox)
        {
            Destroy(_playerFishSpecial.specialHitBox.gameObject);
        }
        _playerFishSpecial.specialHitBox = Instantiate(hitBoxRef).GetComponent<HitBoxMelee>();
        _playerFishSpecial.specialHitBox.transform.SetParent(_playerFishSpecial.hitBoxParent);
        _playerFishSpecial.specialHitBox.Owner = _fish.holder;

        Snap(_playerFishSpecial.specialHitBox.transform);
        
        SetUpGameVariable();
    }

    

    public void SetUpSpecialTrailAnim()
    {
        SetSnapFromRef(trailAnimRef.transform);

        if (_playerFishSpecial.specialTrail)
        {
            Destroy(_playerFishSpecial.specialTrail.gameObject);
        }
        _playerFishSpecial.specialTrail = Instantiate(trailAnimRef).GetComponent<Animation>();
        _playerFishSpecial.specialTrail.transform.SetParent(_playerFishSpecial.hitBoxParent);

        Snap(_playerFishSpecial.specialTrail.transform);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tRef"></param>
    void SetSnapFromRef(Transform tRef)
    {
        snapPosition = tRef.localPosition;
        snapRotation = tRef.localEulerAngles;
        snapScale    =  tRef.localScale;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    void Snap(Transform t)
    {
        t.localPosition = snapPosition;
        t.localEulerAngles = snapRotation;
        t.localScale = snapScale;
    }

    /// <summary>
    /// 
    /// </summary>
    void SetUpGameVariable()
    {
        _playerFishSpecial.specialHitBox.gameObject.layer = LayerMask.NameToLayer("Fish" + _fish.GetPlayerHolder.playerID);
        playerHitBox = _playerFishSpecial.specialHitBox;
        playerHitBox.FreezeFramesOnHit = freezeFrame;
        playerHitBox.InvincibilityFrame = invicibilityFrame;
        playerHitBox.DamageCaused = damage;
    }

    protected override void Update()
    {

    }
   
}
