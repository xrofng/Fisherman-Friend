using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxStageInteraction : DamageOnHit
{
    /// <summary>
    /// Initialization
    /// </summary>
    protected override void Awake()
    {
        Initialization();
    }
    
    protected override void Initialization()
    {
        base.Initialization();
    }

    protected override void Colliding(Collider collider)
    {
        if (!this.isActiveAndEnabled)
        {
            return;
        }

        // if the object we're colliding with is part of our ignore list, we do nothing and exit
        if (_ignoredGameObjects.Contains(collider.gameObject))
        {
            return;
        }

        //// if what we're colliding with isn't part of the target layers, we do nothing and exit
        //if (collider.gameObject.layer != TargetLayerMask)
        //{

        //    return;
        //}

        _player = collider.gameObject.GetComponent<Player>();

        // if what we're colliding with player
        if (_player != null)
        {
            if (!_player.IsInvincible)
            {
                if (FreezeFramesOnHit > 0)
                {
                    StartCoroutine(FreezePlayer(_player,FreezeFramesOnHit));
                }
                else
                {
                    OnCollideWithPlayer(_player, this.transform.position);

                }
            }
        }

        // if what we're colliding with can't be damaged
        else
        {
            OnCollideWithNonDamageable();
        }
    }


    /// <summary>
    /// Describes what happens when colliding with a player object
    /// </summary>
    /// <param name="health">Health.</param>
    protected override void OnCollideWithPlayer(Player player, Vector3 damageDealerPos)
    {
        _player.recieveDamage(this,DamageCaused, damageDealerPos, InvincibilityFrame, KnockData.Instance.getSlapKnockForce((int)DamageCaused, _player.dPercent));
    }

    IEnumerator FreezePlayer(Player player,int FreezeFramesOnHitDuration)
    {
        player.AddAbilityInputIntercepter(this);
        player.FreezingMovement = true;
        int frameCount = 0;
        while (frameCount < FreezeFramesOnHitDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        player.FreezingMovement = false;
        player.RemoveAbilityInputIntercepter(this);
        OnCollideWithPlayer(_player, this.transform.position);
    }
}
