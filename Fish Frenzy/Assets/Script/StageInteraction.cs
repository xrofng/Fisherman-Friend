using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInteraction : MonoBehaviour
{
    public int damage=15;
    public int invicibilityFrame = 20;

    public int IgnorePlayerFrame = 20;

    protected HitBoxStageInteraction hitBox;

    protected List<int> IgnorePlayerWithID = new List<int>();

    /// <summary>
    /// On Start(), we call the ability's intialization
    /// </summary>
    protected virtual void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Gets and stores components for further use
    /// </summary>
    protected virtual void Initialization()
    {
        hitBox = GetComponent<HitBoxStageInteraction>();
        hitBox.InvincibilityFrame = invicibilityFrame;
        hitBox.DamageCaused = damage;
        gameObject.layer = LayerMask.NameToLayer("StageInteraction");
    }

    public void AddIgnorePlayerID (int id) {
        StartCoroutine(ieAddIgnorePlayerID(id));
	}

    IEnumerator ieAddIgnorePlayerID(int id)
    {
        IgnorePlayerWithID.Add(id);
        int frameCount = 0;
        while (frameCount < IgnorePlayerFrame)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        IgnorePlayerWithID.Remove(id);
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
