using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialSpawn : FishSpecial
{

    [Header("Throw Seetting")]
    public MovingObject movingobject;
    public bool endByFrame;
    public int throwDurationFrame = 10;
    public int invicibilityFrame = 50;
    public int freezeFrame = 0;
    public bool launchingDamage = true;

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
    }

    public void SetUpFishSpecial()
    {
        SetUpGameVariable();
    }

    /// <summary>
    /// 
    /// </summary>
    void SetUpGameVariable()
    {
        movingobject.HitBox.FreezeFramesOnHit = freezeFrame;
        movingobject.HitBox.InvincibilityFrame = invicibilityFrame;
        movingobject.HitBox.DamageCaused = attack;
        if (_fish.sfx_Special.clip)
        {
            movingobject.HitBox._SFX = _fish.sfx_Special;
        }
        else
        {
            movingobject.HitBox._SFX = _playerFishSpecial.sfx_Special;
        }

    }

    protected override void Update()
    {

    }

}
