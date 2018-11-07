﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxMelee : DamageOnHit
{
    /// the owner of the HitBoxMelee zone
    public GameObject Owner;
    protected Player ownerPlayer;
    public Player OwnerPlayer
    {
        get
        {
            if(ownerPlayer == null)
            {
                ownerPlayer = Owner.gameObject.GetComponent<Player>();
            }
            return ownerPlayer;
        }
    }

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

        // if the object we're colliding is owner,  we do nothing and exit
        if (collider.gameObject == OwnerPlayer)
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
                
                OnCollideWithPlayer(_player , this.Owner.transform.position);
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
    protected override void OnCollideWithPlayer(Player player , Vector3 damageDealerPos)
    {
        // Check player will be ignored from recently collide
        if (_ignoredGameObjects.Contains(player.gameObject))
        {
            return;
        }
        AddIgnoreGameObject(player.gameObject);
        CauseDamage(damageDealerPos);
    }

    void CauseDamage(Vector3 damageDealerPos)
    {
        if (_player.IsInvincible)
        {
            return;
        }

        // don't care about invincibility
        if (FreezeFramesOnHit > 0)
        {
            StartCoroutine(ieFreezePlayer(_player, FreezeFramesOnHit, damageDealerPos));
        }
        else
        {
            OnEnemyHit(damageDealerPos);
        }
    }

    void OnEnemyHit(Vector3 damageDealerPos)
    {
        ownerPlayer._cPlayerSlap.PlaySlapSFX();
        _player.recieveDamage(DamageCaused, damageDealerPos, InvincibilityFrame);
    }

    IEnumerator ieFreezePlayer(Player player, int FreezeFramesOnHitDuration, Vector3 damageDealerPos)
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
        OnEnemyHit(damageDealerPos);
    }
}