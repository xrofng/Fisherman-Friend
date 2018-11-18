using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StageInteraction))]
public class HitBoxStageInteraction : DamageOnHit
{
    public bool ignoreInvicibility;
    protected StageInteraction stageInteraction;
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
        stageInteraction = GetComponent<StageInteraction>();
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
            OnCollideWithPlayer(_player, this.gameObject);
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
    protected override void OnCollideWithPlayer(Player player, GameObject damageDealer)
    {
        // Check player will be ignored from recently collide
        if (_ignoredGameObjects.Contains(player.gameObject))
        {
            print("Ignore");
            return;
        }
        AddIgnoreGameObject(player.gameObject);

        stageInteraction.OnPlayerCollide(player);
    }

    public void CauseDamage()
    {
        // skip if player invincible and this stage interaction care about invincibility
        if (!ignoreInvicibility && _player.IsInvincible)
        {
            return;
        }

        // don't care about invincibility
        if (FreezeFramesOnHit > 0)
        {
            StartCoroutine(ieFreezePlayer(_player, FreezeFramesOnHit));
        }
        else
        {
            _player.recieveDamage(this, DamageCaused, this.gameObject,gameObject.transform.position , InvincibilityFrame);
        }
    }

    IEnumerator ieFreezePlayer(Player player,int FreezeFramesOnHitDuration)
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
        player.recieveDamage(this, DamageCaused, this.gameObject , this.gameObject.transform.position, InvincibilityFrame);
    }
}
