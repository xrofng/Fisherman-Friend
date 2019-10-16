using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerAbility
{
    public Vector3 jumpForce;
    public float jumpFaster;
    public float fallFaster;
    private Vector3 _playerForwardOnJump = Vector3.zero;

    public float JumpOfWaterMultiplier = 0.7f;

    // Use this for initialization
    protected override void Start()
    {
        Initialization();
        UpdatePlayerForward();
    }

    protected override void Initialization()
    {
        base.Initialization();
        UpdatePlayerForward();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLoop.state == GameLoop.GameState.beforeStart)
        {
            return;
        }
        if (Player.state == Player.eState.ground)
        {
            if (Player.IgnoreInputForAbilities || IgnoreInput)
            {
                return;
            }
            else
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        if (_pInput.GetButtonDown(_pInput.Jump, Player.playerID - 1) && !GetCrossZComponent<PlayerThrow>().aiming)
        {
            if (GetCrossZComponent<PlayerState>().IsSwiming)
            {
                StartJumping(jumpForce * JumpOfWaterMultiplier);
            }
            if (GetCrossZComponent<PlayerState>().IsGrounded)
            {
                StartJumping(jumpForce);
            }
        }

        if (_pRigid.velocity.y < 0)
        {
            _pRigid.velocity += Vector3.up * Physics.gravity.y * fallFaster * Time.deltaTime;
            _pRigid.drag = 0;
        }
    }

    public void StartJumping(Vector3 force)
    {
        UpdatePlayerForward();
        _pRigid.velocity = Vector3.zero;
        _pRigid.AddForce(force, ForceMode.Impulse);
        GetCrossZComponent<PlayerState>().IsJumping = true;
        _pRigid.drag = jumpFaster;
    }

    private void UpdatePlayerForward()
    {
        _playerForwardOnJump = PlayerModel.ModelDirection(-Vector3.forward);
    }

    public Vector3 GetForwardOnJump()
    {
        return _playerForwardOnJump;
    }
}
