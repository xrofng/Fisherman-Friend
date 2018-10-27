﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    /// the possible ways to add knockback : noKnockback, which won't do nothing, set force, or add force
    public enum KnockbackStyles { NoKnockback, SetForce, AddForce }
    /// the possible knockback directions
    public enum KnockbackDirections { BasedOnOwnerPosition, BasedOnSpeed }

    [Header("Targets")]
    // the layers that will be damaged by this object
    public LayerMask TargetLayerMask;

    [Header("Damage Caused")]
    /// The amount of health to remove from the player's health
    public float DamageCaused = 10;
    /// the type of knockback to apply when causing damage
    public KnockbackStyles DamageCausedKnockbackType = KnockbackStyles.SetForce;
    /// The direction to apply the knockback 
    public KnockbackDirections DamageCausedKnockbackDirection;
    /// The force to apply to the object that gets damaged
    public Vector2 DamageCausedKnockbackForce = new Vector2(10, 2);
    /// The duration of the invincibility frames after the hit (in seconds)
    public float InvincibilityDuration = 0.5f;
    /// The duration of the invincibility frames after the hit (in frames)
    public int InvincibilityFrame = 50;

    [Header("Feedback")]
    /// the duration of freeze frames on hit (leave it at 0 to ignore)
    public float FreezeFramesOnHitDuration = 0f;
    /// the frames of player freeze frames on hit (leave it at 0 to ignore)
    public int FreezeFramesOnHit = 0;

    // storage		
    protected Vector2 _lastPosition, _velocity, _knockbackForce;
    protected float _startTime = 0f;
    protected Player _player;
    protected List<GameObject> _ignoredGameObjects;
    protected Color _gizmosColor;
    protected Vector3 _gizmoSize;

    protected SphereCollider _circleCollider;
    protected BoxCollider _boxCollider;

    /// <summary>
    /// Initialization
    /// </summary>
    protected virtual void Awake()
    {
        Initialization();
    }

    /// <summary>
    /// Initialization
    /// </summary>
    protected virtual void Initialization()
    {
        _ignoredGameObjects = new List<GameObject>();
        _boxCollider = GetComponent<BoxCollider>();
        _circleCollider = GetComponent<SphereCollider>();
        _gizmosColor = Color.red;
        _gizmosColor.a = 0.25f;
    }

    /// <summary>
    /// OnEnable we set the start time to the current timestamp
    /// </summary>
    protected virtual void OnEnable()
    {
        _startTime = Time.time;
    }

    /// <summary>
    /// During last update, we store the position and velocity of the object
    /// </summary>
    protected virtual void Update()
    {
        ComputeVelocity();
    }

    /// <summary>
    /// Adds the gameobject set in parameters to the ignore list
    /// </summary>
    /// <param name="newIgnoredGameObject">New ignored game object.</param>
    public virtual void IgnoreGameObject(GameObject newIgnoredGameObject)
    {
        _ignoredGameObjects.Add(newIgnoredGameObject);
    }

    /// <summary>
    /// Removes the object set in parameters from the ignore list
    /// </summary>
    /// <param name="ignoredGameObject">Ignored game object.</param>
    public virtual void StopIgnoringObject(GameObject ignoredGameObject)
    {
        _ignoredGameObjects.Remove(ignoredGameObject);
    }

    /// <summary>
    /// Clears the ignore list.
    /// </summary>
    public virtual void ClearIgnoreList()
    {
        _ignoredGameObjects.Clear();
    }

    /// <summary>
    /// Computes the velocity based on the object's last position
    /// </summary>
    protected virtual void ComputeVelocity()
    {
        _velocity = (_lastPosition - (Vector2)transform.position) / Time.deltaTime;
        _lastPosition = transform.position;
    }

  

    public virtual void OnTriggerEnter(Collider collider)
    {
        ;
        Colliding(collider);
    }

    /// <summary>
    /// Overidding this
    /// </summary>
    /// <param name="collider"></param>
    protected virtual void Colliding(Collider collider)
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

        if (FreezeFramesOnHitDuration > 0)
        {
            //StartCoroutine(FreezePlayer(_player,FreezeFramesOnHitDuration));
            // MMEventManager.TriggerEvent(new MMFreezeFrameEvent(FreezeFramesOnHitDuration));
        }

        // if what we're colliding with player
        if (_player != null)
        {
            if (!_player.IsInvincible)
            {
                
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
    protected virtual void OnCollideWithPlayer(Player player, Vector3 damageDealerPos)
    {

    }

    /// <summary>
    /// Describes what happens when colliding with a non damageable object
    /// </summary>
    protected virtual void OnCollideWithNonDamageable()
    {
        //SelfDamage(DamageTakenEveryTime + DamageTakenNonDamageable);
    }


    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;

        if ((_boxCollider != null) && _boxCollider.enabled)
        {
            _gizmoSize.x = _boxCollider.bounds.size.x;
            _gizmoSize.y = _boxCollider.bounds.size.y;
            _gizmoSize.z = 1f;
            Gizmos.DrawCube(_boxCollider.bounds.center, _gizmoSize);
        }
        if (_circleCollider != null && _circleCollider.enabled)
        {
            Gizmos.DrawSphere((Vector3)this.transform.position + _circleCollider.center, _circleCollider.radius);
        }
    }

    
}