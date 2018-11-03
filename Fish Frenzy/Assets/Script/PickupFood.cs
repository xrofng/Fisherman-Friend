using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFood : Pickup {

    public bool allowToPick = false;

    private Food _food;
    private Food FoodRef
    {
        get
        {
            if (!_food) { _food = this.gameObject.GetComponent<Food>(); }
            return _food;
        }
    }

    string[] pickupButton = { "Slap", "Throw" };

    protected override void Pick(Collider othercollider)
    {
        if (allowToPick)
        {
            Player _player;
            if (othercollider.GetComponent<Player>())
            {
                _player = othercollider.GetComponent<Player>();
                if (_player.GetOneButtonsPress(pickupButton) )
                {
                    //_player._cPlayerSlap.IgnoreInputFor(8);
                    //_player._cPlayerThrow.IgnoreInputFor(8);
                    _player.dPercent -= FoodRef.restorePercent;
                    _player.DamagePercentClamp();
                    Destroy(FoodRef.gameObject);
                }
            }

        }
    }

    public void SetAllowToPick(bool b)
    {
        allowToPick = b;
    }
    
}
