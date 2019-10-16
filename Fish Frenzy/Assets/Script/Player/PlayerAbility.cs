﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]

public class PlayerAbility : MonoBehaviour, MMEventListener<PlayerInputEvent>
    ,MMEventListener<PlayerInputDownEvent>, MMEventListener<PlayerInputHoldEvent>, MMEventListener<PlayerInputUpEvent>
{
    protected string inputName;

    private Player _player;
    public Player Player
    {
        get
        {
            if (!_player) { _player = GetComponent<Player>(); }
            return _player;
        }
    }

    private PlayerModel _playerModel;
    public PlayerModel PlayerModel
    {
        get
        {
            if (!_playerModel) { _playerModel = GetComponent<PlayerModel>(); }
            return _playerModel;
        }
    }

    // private ignore Input for specific ability
    protected bool ignoreInput;
    public bool IgnoreInput { get { return ignoreInput; } }

    public JoystickManager _pInput
    {
        get { return Player.LinkedInputManager; }
    }

    public Rigidbody _pRigid
    {
        get { return Player.rigid; }

    }
    public PlayerAnimation  _pAnimator
    {
        get { return Player._cPlayerAnimator; }
    }

    protected virtual void PlaySFX(SoundEffect SFXclip)
    {
        SoundManager.Instance.PlaySound(SFXclip, transform.position);
    }

    /// <summary>
    /// 
    /// </summary>
    [Header("Other Class Ref")]
    protected GameLoop gameLoop;
    protected PortRoyal portRoyal;
    protected GUIManager guiManager;
    /// <summary>
    /// On Start(), we call the ability's intialization
    /// </summary>
    protected virtual void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Gets and stores components for further use
    /// </summary>
    protected virtual void Initialization()
    {
        gameLoop = FFGameManager.Instance.GameLoop;
        portRoyal = FFGameManager.Instance.PortRoyal;
        guiManager = FFGameManager.Instance.GUIManager;
        this.MMEventStartListening<PlayerInputEvent>();
        this.MMEventStartListening<PlayerInputDownEvent>();
        this.MMEventStartListening<PlayerInputHoldEvent>();
        this.MMEventStartListening<PlayerInputUpEvent>();
    }

    protected virtual void HandleInput()
    {
        if (Player.IgnoreInputForAbilities || IgnoreInput)
        {
            return;
        }
        if (_pInput.GetButtonDown(inputName, Player.playerID - 1))
        {
            MMEventManager.TriggerEvent(new PlayerInputDownEvent(inputName, Player.playerID - 1));
        }
        if (_pInput.GetButton(inputName, Player.playerID - 1))
        {
            MMEventManager.TriggerEvent(new PlayerInputHoldEvent(inputName, Player.playerID - 1));
        }
        if (_pInput.GetButtonUp(inputName, Player.playerID - 1))
        {
            MMEventManager.TriggerEvent(new PlayerInputUpEvent(inputName, Player.playerID - 1));
        }
    }

    public void IgnoreInputFor(int ignoreFrame)
    {
        StartCoroutine(InvokeIgnoreInput(ignoreFrame));
    }

    IEnumerator InvokeIgnoreInput(int frameDuration)
    {
        int frameCount = 0;
        ignoreInput = true;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        ignoreInput = false;
    }

    public void ActionForFrame(int frameDuration, System.Action begin, System.Action end)
    {
        StartCoroutine(ieActionForFrame(frameDuration, begin, end));
    }

    IEnumerator ieActionForFrame(int frameDuration, System.Action begin, System.Action end)
    {
        begin();
        int frameCount = 0;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        end();
    }

    public T GetCrossZComponent<T>() where T : PlayerAbility
    {
        if (typeof(T) == typeof(PlayerMovement)) { return Player._cPlayerMovement as T; }
        if (typeof(T) == typeof(PlayerThrow)) { return Player._cPlayerThrow as T; }
        if (typeof(T) == typeof(PlayerFishing)) { return Player._cPlayerFishing as T; }
        if (typeof(T) == typeof(PlayerSlap)) { return Player._cPlayerSlap as T; }
        if (typeof(T) == typeof(PlayerSwitchFish)) { return Player._cPlayerSwitch as T; }
        if (typeof(T) == typeof(PlayerInvincibility)) { return Player._cPlayerInvincibility as T; }
        if (typeof(T) == typeof(PlayerState)) { return Player._cPlayerState as T; }
        if (typeof(T) == typeof(PlayerFishInteraction)) { return Player._cPlayerFishInteraction as T; }
        if (typeof(T) == typeof(PlayerSpecial)) { return Player._cPlayerSpecial as T; }
        if (typeof(T) == typeof(PlayerDamageHitbox)) { return Player._cPlayerDamageHitBox as T; }

        return this as T;
    }

    protected virtual void OnInputDown() { }
    protected virtual void OnInputHold() { }
    protected virtual void OnInputUp() { }

    public void OnMMEvent(PlayerInputEvent eventType)
    {
        if (eventType.buttonName == inputName && eventType.playerId == Player.playerID - 1)
        {
            MMEventManager.TriggerEvent(new PlayerInputDownEvent(inputName, Player.playerID - 1));
            StartCoroutine(PressInputFor(eventType.holdingFrame));
        }
    }

    IEnumerator PressInputFor(int frameDuration)
    {
        int frameCount = 0;
        while(frameCount < frameDuration)
        {
            MMEventManager.TriggerEvent(new PlayerInputHoldEvent(inputName, Player.playerID - 1));
            frameCount += 1;
            yield return new WaitForEndOfFrame();
        }
        MMEventManager.TriggerEvent(new PlayerInputUpEvent(inputName, Player.playerID - 1));
    }

    public void OnMMEvent(PlayerInputUpEvent eventType)
    {
        if (eventType.buttonName == inputName && eventType.playerId == Player.playerID - 1)
        {
            OnInputUp();
        }
    }

    public void OnMMEvent(PlayerInputDownEvent eventType)
    {
        if (eventType.buttonName == inputName && eventType.playerId == Player.playerID - 1)
        {
            OnInputDown();
        }
    }

    public void OnMMEvent(PlayerInputHoldEvent eventType)
    {
        if (eventType.buttonName == inputName && eventType.playerId == Player.playerID - 1)
        {
            OnInputHold();
        }
    }
}
