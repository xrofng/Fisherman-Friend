using OneButton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxMelee : DamageOnHit
{
    [Header("Utility")]
    //public DamagingData DamagingData;
    public bool mustHaveOwner;

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
    public SoundEffect m_hitSfx;
    public SoundEffect HitSFX
    {
        get
        {
            if(m_hitSfx == null)
            {
                m_hitSfx = new SoundEffect();
            }
            return m_hitSfx;
        }
        set
        {
            m_hitSfx = value;
        }
    }

    public GameObject HitEffect;
    public Vector3 HitEffectSpawnOffest;
    public float HitEffectDuration;

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

    public void SetDamage(DamagingData data)
    {
        Damage = data;
        mustHaveOwner = data.damageFromOwner;
        //FreezeFramesOnHit = data.freezeFrame;
        //InvincibilityFrame = data.invicibilityFrame;
        //DamageCaused = data.damage;
        //isLauncher = data.isLauncher;

        //damageFromOwner = data.damageFromOwner;
        //mustHaveOwner = damageFromOwner;

        //unlimitedDamaging = data.unlimitedDamaging;
        //numberOfDamaging = data.numberOfDamaging;
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
            CheckCollidingOther(collider);
            OnCollideWithNonDamageable();
        }
    }

  
    /// <summary>
    /// Describes what happens when colliding with a player object
    /// </summary>
    /// <param name="health">Health.</param>
    protected override void OnCollideWithPlayer(Player damagedReceiver , GameObject damageDealer)
    {
        // Check player will be ignored from recently collide
        if (_ignoredGameObjects.Contains(damagedReceiver.gameObject))
        {
            return;
        }
        AddIgnoreGameObject(damagedReceiver.gameObject);
        SpawnHitEffect(damagedReceiver.transform.position);
        CauseDamage(damageDealer);
    }

    private void SpawnHitEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(HitEffect, pos+ HitEffectSpawnOffest, Quaternion.identity);
        //Destroy(HitEffect, HitEffectDuration);
    }

    void CauseDamage(GameObject damageDealer)
    {
        if (_player.IsInvincible)
        {
            return;
        }

        if (_player._cPlayerBlock.CheckBlock(damageDealer,_player) && !Damage.IsUnblockable)
        {
            return;
        }

        // don't care about invincibility
        if (Damage.FreezeFramesOnHit > 0)
        {
            StartCoroutine(ieFreezePlayer(_player, Damage.FreezeFramesOnHit, damageDealer));
        }
        else
        {
            OnEnemyHit(damageDealer);
        }
    }

    void OnEnemyHit(GameObject damageDealer)
    {
        SoundManager.Instance.PlaySound(HitSFX, this.transform.position);
        Vector3 forcesource = this.transform.position;
        if (Damage.damageFromOwner)
        {
            forcesource = damageDealer.transform.position;
        }
        if (_player)
        {
            _player._cPlayerFishInteraction.damagedByFish = OwnerPlayer.mainFish;
            _player.RecieveDamage(Damage.damage, damageDealer, forcesource, Damage.InvincibilityFrame, Damage.isLauncher);
        }

        EvaluateLimitedUses();
    }

    public void EvaluateLimitedUses()
    {
        Damage.numberOfDamaging -= 1;
        if (!Damage.unlimitedDamaging && Damage.numberOfDamaging <= 0)
        {
            if (OnReachLimitedDamaging)
            {
                OnReachLimitedDamaging.PerformResponse();
            }
        }
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