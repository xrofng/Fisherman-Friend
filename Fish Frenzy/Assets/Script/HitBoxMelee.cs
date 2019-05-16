using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxMelee : DamageOnHit
{
    [Header("Utility")]
    public bool mustHaveOwner;
    /// the owner of the HitBoxMelee zone
    public bool damageFromOwner = true;
    public bool isLauncher;
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

    [Header("SFX")]
    public SoundEffect _SFX;

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
        if (!GetComponent<AudioSource>())
        {
            this.gameObject.AddComponent<AudioSource>();
        }
    }

    protected override void Colliding(Collider collider)
    {
        if (mustHaveOwner && !Owner)
        {
            return;
        }
        

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
                OnCollideWithPlayer(_player, this.Owner.gameObject);
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
    protected override void OnCollideWithPlayer(Player player , GameObject damageDealer)
    {
        // Check player will be ignored from recently collide
        if (_ignoredGameObjects.Contains(player.gameObject))
        {
            return;
        }
        AddIgnoreGameObject(player.gameObject);
        Debug.Log(damageDealer);
        CauseDamage(damageDealer);
    }

    void CauseDamage(GameObject damageDealer)
    {
        if (_player.IsInvincible)
        {
            return;
        }

        // don't care about invincibility
        if (FreezeFramesOnHit > 0)
        {
            StartCoroutine(ieFreezePlayer(_player, FreezeFramesOnHit, damageDealer));
        }
        else
        {
            OnEnemyHit(damageDealer);
        }
    }

    void OnEnemyHit(GameObject damageDealer)
    {
        SoundManager.Instance.PlaySound(_SFX,this.transform.position);
        Vector3 forcesource = this.transform.position;
        if (damageFromOwner)
        {
            forcesource = damageDealer.transform.position;
        }
        Debug.Log(damageDealer);
        _player.recieveDamage(DamageCaused, damageDealer , forcesource, InvincibilityFrame,isLauncher);
    }

    IEnumerator ieFreezePlayer(Player player, int FreezeFramesOnHitDuration, GameObject damageDealer)
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
        OnEnemyHit(damageDealer);
    }
}