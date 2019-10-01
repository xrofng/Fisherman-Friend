using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFish : Pickup {

    public bool allowToPick = false;
    private Fish _fish;
    private Fish FishRef
    {
        get
        {
            if (!_fish) { _fish = this.gameObject.GetComponent<Fish>(); }
            return _fish;
        }
    }

    protected override void Pick(Collider othercollider)
    {
        if (allowToPick)
        {
            Player _player;
            if (othercollider.GetComponent<Player>())
            {
                _player = othercollider.GetComponent<Player>();

                if (PressPickup() && !_player.holdingFish)
                {
                    _player._cPlayerSlap.IgnoreInputFor(8);
                    _player._cPlayerThrow.IgnoreInputFor(8);
                    _player._cPlayerFishInteraction.HoldFish(FishRef);
                }
            }
        }
    }

    public void SetAllowToPick(bool b)
    {
        allowToPick = b;
        FishRef.GetCollider<BoxCollider>().isTrigger = b;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
