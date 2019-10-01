using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : PlayerAbility
{
    protected override void Initialization()
    {
        base.Initialization();
        inputName = _pInput.Block;
    }
    // Update is called once per frame
    void Update ()
    {
		if(_player.state == Player.eState.ground)
        {
            HandleInput();
        }
    }

    protected override void OnInputDown()
    {
        Block();
    }

    private void Block()
    {
        _player.Animation.SetTrigger("block");
    }
}

