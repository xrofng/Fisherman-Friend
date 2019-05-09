using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerAbility
{
    public Vector3 speed;
    public Vector3 jumpForce;
    public float jumpFaster;
    public float fallFaster;

    public bool freezeMovement;

    public Vector3 lookTo;
    Vector3 playerDirection;

    // Use this for initialization
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere( transform.position-lookTo * 3, 0.5f);
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
        float axisX = _pInput.GetAxis(_pInput.Hori, _player.playerID-1);
        float axisY = _pInput.GetAxis(_pInput.Verti, _player.playerID-1);

        Vector3 mov = new Vector3(axisRawX * speed.x, 0.0f, axisRawY*speed.z );

        //Vector3 mov = new Vector3(Input.GetAxisRaw(hori) * speed.x, 0.0f, Input.GetAxisRaw(verti) * speed.z);
        mov = mov * Time.deltaTime;
        if (!freezeMovement && !GetCrossZComponent<PlayerState>().IsAttacking && !GetCrossZComponent<PlayerState>().IsDamaged)
        {
            this.transform.Translate(mov);
        }

        
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

    public void ChangeDirection(float dirX,float dirZ)
    {
        playerDirection = lookTo;
        playerDirection = Vector3.right * -dirX + Vector3.forward * -dirZ;
        lookTo = playerDirection;
    }

    void Jump()
    {
        if (_pInput.GetButtonDown(_pInput.Jump, _player.playerID - 1) && !GetCrossZComponent<PlayerThrow>().aiming)
        {
            if ( GetCrossZComponent<PlayerState>().IsSwiming)
            {
                StartJumping(jumpForce * 0.7f);
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
        _pRigid.velocity = Vector3.zero;
        _pRigid.AddForce(force, ForceMode.Impulse);
        GetCrossZComponent<PlayerState>().IsJumping = true;
        _pRigid.drag = jumpFaster;
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
