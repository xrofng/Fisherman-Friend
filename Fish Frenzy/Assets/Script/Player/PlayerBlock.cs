﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : PlayerAbility
{
    public int BlockIgnoreAbilityFrame = 40;
    public int CounterIgnoreAbilityFrame = 40;

    public Block thisBlock;

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
        ActionForFrame(BlockIgnoreAbilityFrame,
                 () => { Player.AddAbilityInputIntercepter(this); },
                 () => { Player.RemoveAbilityInputIntercepter(this); });
    }

    public bool CheckBlock(GameObject damageDealer, Player damageReciever)
    {
        if(IsBlock(damageDealer, damageReciever))
        {
            PerformCounter();
            return true;
        }
        return false;
    }

    private void PerformCounter()
    {
        Player.Animation.SetTrigger("counter");
        ActionForFrame(CounterIgnoreAbilityFrame,
                 () => { Player.AddAbilityInputIntercepter(this); },
                 () => { Player.RemoveAbilityInputIntercepter(this); });
    }

    private bool IsBlock(GameObject damageDealer, Player damageReciever)
    {
        RaycastHit[] hits;
        Vector3 dir = (damageReciever.transform.position - damageDealer.transform.position).normalized;
        float dis = Vector3.Distance(damageReciever.transform.position, damageDealer.transform.position);
        hits = Physics.RaycastAll(damageDealer.transform.position, dir, dis);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject != damageReciever.gameObject &&
                hit.collider.gameObject == thisBlock.gameObject)
            {
                return true;
            }
        }
        return false;
    }
}

