using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : PlayerAbility
{
    public int IgnoreAbilityFrame = 40;

    protected override void Initialization()
    {
        base.Initialization();
        inputName = _pInput.Block;
    }
    // Update is called once per frame
    void Update ()
    {
		if(Player.state == Player.eState.ground)
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
        Player.Animation.SetTrigger("block");
        ActionForFrame(IgnoreAbilityFrame,
                 () => { Player.AddAbilityInputIntercepter(this); },
                 () => { Player.RemoveAbilityInputIntercepter(this); });
    }
}

