using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialMelee : FishSpecial {

    protected HitBoxMelee playerHitBox;
    public HitBoxMelee thisSpecialHitBox;
    public int invicibilityFrame = 50;
    public int freezeFrame = 10;
    public bool launchingDamage = true;

    protected bool mSpecialing;
    public bool MeleeSpecialing
    {
        get { return mSpecialing; }
        set { mSpecialing = value; }
    }

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
        if (_fish.sfx_Special.clip)
        {
            playerHitBox._SFX = _fish.sfx_Special;
        }
        else
        {
            playerHitBox._SFX = _playerFishSpecial.sfx_Special;
        }
    }

    public override void OnSpecialActivated()
    {
        base.OnSpecialActivated();
        ActionForFrame(SpeiclaClipFrameCount + IgnoreInputFrameDuration,
                 () => { MeleeSpecialing = true; },
                 () => { MeleeSpecialing = false; });

        _player._cPlayerAnimator.ChangeAnimState((int)specialClip, SpeiclaClipFrameCount, true);
    }

    public override bool GetSpecialing()
    {
        return MeleeSpecialing;
    }

    protected override void Update()
    {

    }
   
    
}
