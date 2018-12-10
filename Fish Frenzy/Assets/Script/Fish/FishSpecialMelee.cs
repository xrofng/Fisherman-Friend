using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialMelee : FishSpecial {

    protected HitBoxMelee playerHitBox;
    public HitBoxMelee thisSpecialHitBox;
    public int invicibilityFrame = 50;
    public int freezeFrame = 10;
    public bool launchingDamage = true;


    protected Vector3 snapPosition;
    protected Vector3 snapRotation;
    protected Vector3 snapScale;

    [Header("Prefab Ref")]
    public Transform hitBoxRef;

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
    }


    public virtual void SetUpFishSpecial()
    {
        SetUpSpecialHitBox();
        SetUpGameVariable();
    }

    protected void SetUpSpecialHitBox()
    {
        //SetSnapFromRef(hitBoxRef.transform);

        foreach (Transform hb in _fish.GetPlayerHolder.GetPart(Player.ePart.hitBox))
        {
            if(hb.gameObject.name == thisSpecialHitBox.gameObject.name)
            {
                _playerFishSpecial.specialHitBox = hb.GetComponent<HitBoxMelee>();
            }
        }
        if(_playerFishSpecial.specialHitBox == null) { Debug.LogError("Can't find hitbox"); }

        _playerFishSpecial.specialHitBox.transform.SetParent(_playerFishSpecial.hitBoxParent);
        _playerFishSpecial.specialHitBox.Owner = _fish.holder;
        _playerFishSpecial.specialHitBox.isLauncher = launchingDamage;

        //Snap(_playerFishSpecial.specialHitBox.transform);
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


    public virtual void SpecialMeleeAttack(Player _player)
    {
        _player.animator.ChangeAnimState((int)specialClip, SpeiclaClipFrameCount, true, (int)PlayerAnimation.Anim.HoldFish);      
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
        playerHitBox.DamageCaused = attack;
        if (_fish.sfx_Special)
        {
            playerHitBox._SFXclip = _fish.sfx_Special;
        }
        else
        {
            playerHitBox._SFXclip = _playerFishSpecial.sfx_Special;
        }
    }

    protected override void Update()
    {

    }
   
}
