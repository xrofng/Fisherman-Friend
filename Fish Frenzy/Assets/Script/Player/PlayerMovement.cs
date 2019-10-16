using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerAbility
{
    public float Speed = 7.5f;
    public Vector3 jumpForce;
    public float jumpFaster;
    public float fallFaster;

    public bool FreezeMovement;
    public bool FreezeRotation;
    public float JumpOfWaterMultiplier = 0.7f;

    public float JumpDirectionInfluence = 0.5f;
    public float JumpInputInfluence = 0.5f;

    public Vector3 lookTo;
    Vector3 playerDirection;

    private PlayerModel _playerModel;
    private Vector3 _playerForwardOnJump = Vector3.zero;

    // Use this for initialization
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        _playerModel = _player.gameObject.GetComponent<PlayerModel>();
        UpdatePlayerForward();
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _playerForwardOnJump * 3);
        Gizmos.color = Color.green;
        Vector3 _playerRightOnJump = Quaternion.Euler(0, 90, 0) * _playerForwardOnJump;
        Gizmos.DrawLine(transform.position, transform.position + _playerRightOnJump * 3);

    }

    // Update is called once per frame
    void Update()
    {
        if(gameLoop.state == GameLoop.GameState.beforeStart)
        {
            return;
        }
        if (_player.state == Player.eState.ground)
        {
            if (_player.IgnoreInputForAbilities || IgnoreInput)
            {
                return;
            }else
            {
                Move();
                Jump();
            }
        }
    }

    void Move()
    {
        float axisRawX = _pInput.GetAxisRaw(_pInput.Hori, _player.playerID-1);
        float axisRawY = _pInput.GetAxisRaw(_pInput.Verti, _player.playerID-1);
        float axisX =    _pInput.GetAxis(_pInput.Hori, _player.playerID-1);
        float axisY =    _pInput.GetAxis(_pInput.Verti, _player.playerID-1);

        if (JoystickManager.Instance.IncludeKeyboardKey)
        {
            axisRawX += _pInput.GetAxisRaw("k" + _pInput.Hori, _player.playerID - 1);
            axisRawY += _pInput.GetAxisRaw("k" + _pInput.Verti, _player.playerID - 1);
            axisX += _pInput.GetAxis("k" + _pInput.Hori, _player.playerID - 1);
            axisY += _pInput.GetAxis("k" + _pInput.Verti, _player.playerID - 1);
        }

        Move(new Vector3(axisRawX, 0.0f, axisRawY));
        
        playerDirection = lookTo;
        if (sClass.getSign(axisX, 0.015f) != 0 || sClass.getSign(axisY, 0.015f) != 0)
        {
            if (sClass.intervalCheck(axisRawX, -0.9f, 0.9f, true) || sClass.intervalCheck(axisRawY, -0.9f, 0.9f, true))
            {
                ChangeDirection(axisRawX, axisRawY);
            }
        }

        if (GetCrossZComponent<PlayerThrow>().useAimAssist)
        {
            if (playerDirection.sqrMagnitude > 0.0f && !_player.Aiming)
            {
                _player.GetPart(Player.ePart.body).transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            }
        }else
        {
            if (playerDirection.sqrMagnitude > 0.0f)
            {
                _player.GetPart(Player.ePart.body).transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            }
        }
        
    }

    public void Move(Vector3 mov)
    {
        if (!FreezeMovement && !GetCrossZComponent<PlayerState>().IsAttacking && !GetCrossZComponent<PlayerState>().IsDamaged)
        {
            Vector3 _forward = mov.z * Vector3.forward * Speed;
            Vector3 _right = mov.x* Vector3.right * Speed ;
            Vector3 _moveSpeed = _forward + _right;
            if (_player._cPlayerState.IsJumping)
            {
                Vector3 _inputSpeed = _moveSpeed * JumpInputInfluence;
                Vector3 _jumpSpeed = _playerForwardOnJump* Speed * JumpDirectionInfluence;
                _moveSpeed = _jumpSpeed + _inputSpeed;
            }
            _moveSpeed.y *= 0;
            _moveSpeed *= Time.deltaTime;
            this.transform.Translate(_moveSpeed.x, 0.0f, _moveSpeed.z);
        }
    }

    public void ChangeDirection(float dirX,float dirZ)
    {
        if (FreezeRotation)
        {
            return;
        }
        playerDirection = lookTo;
        playerDirection = Vector3.right * -dirX + Vector3.forward * -dirZ;
        lookTo = playerDirection;
    }

    void Jump()
    {
        if (_pInput.GetButtonDown(_pInput.Jump, _player.playerID - 1) && !GetCrossZComponent<PlayerThrow>().aiming)
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
        _playerForwardOnJump = _playerModel.ModelDirection(-Vector3.forward);
    }

    public float GetTurningDegree()
    {
        float cos = -lookTo.x;
        float sin = -lookTo.z;
        float tan = Mathf.Abs(cos / sin);

        float Deg = Mathf.Atan(tan) * Mathf.Rad2Deg;

        Deg = ComputeDegree(Deg, cos, sin);
        return Deg;
    }

    float ComputeDegree(float oDegree, float cos, float sin)
    {
        //Find Quadrant
        if (cos == 0 && sin == 0)
        {
        }
        /* check for point on x-axis */
        else if (sin == 0)
        {
            return (cos > 0) ? 0 : 180;
        }
        /* check for point on y-axis */
        else if (cos == 0)
        {
            return (sin > 0) ? 90 : 270;
        }
        /* check for quardrant */
        int Q = 1;
        Q = (cos > 0 && sin > 0) ? 1 : Q;
        Q = (cos < 0 && sin > 0) ? 2 : Q;
        Q = (cos < 0 && sin < 0) ? 3 : Q;
        Q = (cos > 0 && sin < 0) ? 4 : Q;

        float d = oDegree +( 90 * (Q - 1));
        d = (Q == 1) ? 90 - d : d;
        if (Q == 3)
        {
            float changer = (225.0f - d) * 2.0f;
            d = d + changer;
        }
        return d;
    }
}
