using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialSpawn : FishSpecial
{
    [Header("Throw Seetting")]
    public MovingObject movingObjects;
    protected MovingObject currentMovingObj;

    [Header("Channeling")]
    public int throwingFrameDuration = 10;
    public int channelingFrameDuration = 10;

    public override void OnPlayerHold()
    {
        base.OnPlayerHold();
        movingObjects.HitBox.SetDamage(damage);
        if (fish.sfx_Special.clip)
        {
            movingObjects.HitBox.HitSFX = fish.sfx_Special;
        }
        else
        {
            movingObjects.HitBox.HitSFX = PlayerFishSpecial.sfx_Special;
        }
    }

    protected void DestroyMovingObject(MovingObject movingObject)
    {
        movingObject.OnBeforeDestroy();
        Destroy(movingObject.gameObject);
    }

    protected bool SpawnedExist()
    {
        return currentMovingObj;
    }

}
