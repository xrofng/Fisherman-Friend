using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecial : PlayerAbility
{
    public int ignoreSpecialFrame = 4;
    
    public T FishSpecialAs<T>() where T : FishSpecial
    {
        return Player.mainFish._cSpecial as T;
    }

    public FishSpecial Special
    {
        get
        {
            return Player.mainFish._cSpecial;
        }
    }

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        inputName = _pInput.Special;
    }

    /// <summary>
    /// is player performing any fish special ability
    /// </summary>
    /// <returns></returns>
    public bool GetSpecialing()
    {
        if (Player.mainFish && Player.mainFish._cSpecial)
        {
            return Player.mainFish._cSpecial.CheckPerformingSpecial();
        }
        return false;
    }

    public bool GetSpecialing<T>() where T : FishSpecial
    {
        if (!Player.mainFish)
        {
            return false;
        }
        T specialComponent = Player.mainFish.GetComponent<T>();
        if (specialComponent != null)
        {
            return specialComponent.CheckPerformingSpecial();
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.state == Player.eState.ground)
        {
            HandleInput();
        }
    }

    bool IsValidForSpecial()
    {
        if (Player.mainFish == null)
        {
            return false;
        }

        if (Player.mainFish.state == Fish.FishConditionalState.dehydrate || !Player.mainFish.GetComponent<FishSpecial>())
        {
            return false;
        }
        return true;
    }

    protected override void OnInputDown()
    {
        base.OnInputDown();
        if (!IsValidForSpecial())
        {
            return;
        }

        Special.TryPerformSpecial(FishSpecial.FishSpecialActivatedState.DOWN);
    }

    protected override void OnInputHold()
    {
        base.OnInputHold();
        if (!IsValidForSpecial())
        {
            return;
        }

        Special.TryPerformSpecial(FishSpecial.FishSpecialActivatedState.HOLD);
    }

    protected override void OnInputUp()
    {
        base.OnInputUp();
        if (!IsValidForSpecial())
        {
            return;
        }

        Special.TryPerformSpecial(FishSpecial.FishSpecialActivatedState.UP);
    }

    [Header("SFX")]
    public SoundEffect sfx_Special;
}
