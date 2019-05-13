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

    protected override void Pick(Collider othercollider)
    {
        if (allowToPick)
        {
            Player _player;
            if (othercollider.GetComponent<Player>())
            {
                _player = othercollider.GetComponent<Player>();
                string[] pickupButton = { _player.LinkedInputManager.Throw, _player.LinkedInputManager.Special };
                if (_player.LinkedInputManager.GetOneButtonsDown(pickupButton, _player.playerID-1) && !_player.holdingFish)
                {
                    //_player._cPlayerSlap.IgnoreInputFor(8);
                    //_player._cPlayerThrow.IgnoreInputFor(8);
                    _player.damagePercent -= FoodRef.restorePercent;
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
