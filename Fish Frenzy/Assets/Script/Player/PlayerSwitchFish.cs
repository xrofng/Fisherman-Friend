﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchFish : PlayerAbility
{
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();

    }

    // Update is called once per frame
    void Update()
    {
        if (_player.state == Player.eState.ground)
        {
            if (_player.IgnoreInputForAbilities || IgnoreInput)
            {
                return;
            }
            if (GetCrossZComponent<PlayerSpecial>().Specialing)
            {
                return;
            }
            SwitchFish();
        }

    }

    void SwitchFish()
    {
        if (_pInput.GetButtonDown(_pInput.Switch, _player.playerID - 1))
        {
            _player.baitedFish = _player.subFish;
            _player.subFish = _player.mainFish;
            if (_player.subFish != null)
            {
                _player.subFish.KeepFish(true);
            }

            _player.mainFish = _player.baitedFish;
            _player.baitedFish = null;

            if (_player.mainFish != null)
            {
                _player.mainFish.KeepFish(false);
            }
            GetCrossZComponent<PlayerFishInteraction>().SetHoldFish(_player.mainFish != null);
        }
    }
}
