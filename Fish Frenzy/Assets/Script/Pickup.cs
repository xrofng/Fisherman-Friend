using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    
    /// The effect to instantiate when the coin is hit
    public GameObject Effect;
    /// The sound effect to play when the object gets picked
    public AudioClip PickSfx;
    /// if this is set to true, the object will be disabled when picked
    public bool DisableObjectOnPick = true;
    
    protected Collider _otherCollider;
    
    protected Player _player = null;
    protected bool _pickable = false;

    protected virtual void Start()
    {
    }

    /// <summary>
    /// Triggered when something collides with the coin
    /// </summary>
    /// <param name="collider">Other.</param>
    public virtual void OnTriggerStay(Collider collider)
    {
        _otherCollider = collider;
        PickItem();
    }

    /// <summary>
    /// Check if the item is pickable and if yes, proceeds with triggering the effects and disabling the object
    /// </summary>
    public virtual void PickItem()
    {
        if (CheckIfPickable())
        {
            Effects();
           
            Pick();
            Pick(_otherCollider);
            if (DisableObjectOnPick)
            {
                // we desactivate the gameobject
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Checks if the object is pickable.
    /// </summary>
    /// <returns><c>true</c>, if if pickable was checked, <c>false</c> otherwise.</returns>
    protected virtual bool CheckIfPickable()
    {
        // if what's colliding with the this ain't a characterBehavior, we do nothing and exit
        _player = _otherCollider.gameObject.GetComponent<Player>();
        if (_player == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Triggers the various pick effects
    /// </summary>
    protected virtual void Effects()
    {
        if (PickSfx != null)
        {
            GetComponent<AudioSource>().clip = PickSfx; GetComponent<AudioSource>().Play();
        }

        if (Effect != null)
        {
            // adds an instance of the effect at the coin's position
            Instantiate(Effect, transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// Override this to describe what happens when the object gets picked
    /// </summary>
    protected virtual void Pick()
    {
        
    }

    /// <summary>
    /// Override this to describe what happens when the object gets picked
    /// </summary>
    protected virtual void Pick(Collider othercollider)
    {

    }
}
