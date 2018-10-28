using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_JellyFish : StageInteraction {

    public Vector3 bounceForce = new Vector3(0,5,0);
    public int ShockingFrame = 10;
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
    void Update () {
		
	}



    public override void OnPlayerCollide(Player _player)
    {
        // Check player will be ignored from recently collide
        if (IgnorePlayerWithID.Contains(_player.playerID))
        {
            return;
        }
        AddIgnorePlayerID(_player.playerID);
        
        if (_player._cPlayerState.IsJumping)
        {
            _player._cPlayerMovement.StartJumping(bounceForce);
        }
        else 
        {
            hitBox.CauseDamage();
        }
    }
    
}
