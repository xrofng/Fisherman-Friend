using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInteraction : Creature
{
    public int damage=15;
    public int invicibilityFrame = 20;

    public int ignorePlayerFrame = 20;

    protected HitBoxStageInteraction hitBox;

    /// <summary>
    /// On Start(), we call the ability's intialization
    /// </summary>
    protected virtual void Start()
    {
        Initialization();
    }
    protected virtual void Update()
    {

    }

    /// <summary>
    /// Gets and stores components for further use
    /// </summary>
    protected virtual void Initialization()
    {
        hitBox = GetComponent<HitBoxStageInteraction>();
        hitBox.Damage.InvincibilityFrame = invicibilityFrame;
        hitBox.Damage.IgnorePlayerFrame = ignorePlayerFrame;
        hitBox.Damage.damage = damage;
        gameObject.layer = LayerMask.NameToLayer("StageInteraction");
    }

    /// <summary>
    /// Override this to describe what happens to player when collide from HitBoxStageInteraction
    /// </summary>
    public virtual void OnPlayerCollide(Player _player)
    {
        Debug.LogWarning("Default Empty OnPlayerCollide with player " + _player.name);
    }

    /// <summary>
    /// Override this to dealing
    /// </summary>
    public virtual void DealDamage(Player _player)
    {
        Debug.LogWarning("Default Deal Damage OnPlayerCollide with player " + _player.name);
    }
}
