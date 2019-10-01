using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialSpawn : FishSpecial
{
    [Header("Throw Seetting")]
    public MovingObject movingObjects;
    protected MovingObject currentMovingObj;

    public bool endByFrame;
    public int invicibilityFrame = 50;
    public int freezeFrame = 0;
    public bool launchingDamage = true;

    [Header("Channeling")]
    public int throwingFrameDuration = 10;
    public int channelingFrameDuration = 10;
    public bool ignoreInput;

    public override void OnPlayerHold()
    {
        base.OnPlayerHold();
        movingObjects.HitBox.FreezeFramesOnHit = freezeFrame;
        movingObjects.HitBox.InvincibilityFrame = invicibilityFrame;
        movingObjects.HitBox.DamageCaused = attack;
        if (fish.sfx_Special.clip)
        {
            movingObjects.HitBox._SFX = fish.sfx_Special;
        }
        else
        {
            movingObjects.HitBox._SFX = PlayerFishSpecial.sfx_Special;
        }
    }

    protected void DestroyMovingObject(MovingObject movingObject)
    {
        movingObject.OnBeforeDestroy();
        Destroy(movingObject.gameObject);
    }

}
