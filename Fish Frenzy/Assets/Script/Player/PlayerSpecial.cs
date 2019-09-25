using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecial : PlayerAbility
{
    public int ignoreSpecialFrame = 4;
    
    public T FishSpecialAs<T>() where T : FishSpecial
    {
        return _player.mainFish._cSpecial as T;
    }

    public FishSpecial Special
    {
        get
        {
            return _player.mainFish._cSpecial;
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
        if (_player.mainFish && _player.mainFish._cSpecial)
        {
            return _player.mainFish._cSpecial.CheckPerformingSpecial();
        }
        return false;
    }

    public bool GetSpecialing<T>() where T : FishSpecial
    {
        if (!_player.mainFish)
        {
            return false;
        }
        T specialComponent = _player.mainFish.GetComponent<T>();
        if (specialComponent != null)
        {
            return specialComponent.CheckPerformingSpecial();
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.state == Player.eState.ground)
        {
            HandleInput();
        }
    }

    bool IsValidForSpecial()
    {
        if (_player.mainFish == null)
        {
            return false;
        }

        if (_player.mainFish.state == Fish.FishConditionalState.dehydrate || !_player.mainFish.GetComponent<FishSpecial>())
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

    [Header("Debug")]
    public bool showHitBox;
}
