using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_JellyFish : StageInteraction {

    public Vector3 bounceForce = new Vector3(0,5,0);
    public int ShockingFrame = 10;
    public AudioClip sfx_Shock;
    public AudioClip sfx_Bounce;

    // Use this for initialization
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        hitBox.FreezeFramesOnHit = ShockingFrame;
    }

    // Update is called once per frame
    protected override void Update () {
		
	}

    public override void OnPlayerCollide(Player _player)
    {
        if (_player._cPlayerState.IsJumping)
        {
            // play animation feedback
            Animator.ChangeAnimState(1, true, 0);
            PlaySFX(sfx_Bounce);
            _player._cPlayerMovement.StartJumping(bounceForce);
        }
        else 
        {
            PlaySFX(sfx_Shock);
            hitBox.CauseDamage();
        }
    }
    
}
