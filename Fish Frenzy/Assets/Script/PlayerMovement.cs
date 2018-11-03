using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerAbility {
    public Vector3 speed;
    public Vector3 jumpForce;
    public float jumpFaster;
    public float fallFaster;

    public bool freezeMovement;

    public Vector3 lookTo;
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
        string hori = "Hori" + _player.playerID;
        string verti = "Verti" + _player.playerID;
        Vector3 mov = new Vector3(Input.GetAxisRaw(hori) * speed.x, 0.0f, Input.GetAxisRaw(verti) * speed.z);
        mov = mov * Time.deltaTime;
        if (!freezeMovement && !GetCrossZComponent<PlayerState>().IsAttacking)
        {
            this.transform.Translate(mov);
        }

        float axisRawX = Input.GetAxisRaw(hori);
        float axisRawY = Input.GetAxisRaw(verti);
        Vector3 playerDirection = lookTo;
        if (sClass.getSign(Input.GetAxis(hori), 0.015f) != 0 || sClass.getSign(Input.GetAxis(verti), 0.015f) != 0)
        {
            if (sClass.intervalCheck(axisRawX, -0.9f, 0.9f, true) || sClass.intervalCheck(axisRawY, -0.9f, 0.9f, true))
            {
                playerDirection = Vector3.right * -axisRawX + Vector3.forward * -axisRawY;
                lookTo = playerDirection;
            }
        }

        if (playerDirection.sqrMagnitude > 0.0f && !_player.Aiming)
        {
            _player.getPart(Player.ePart.body).transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        }
    }

    void Jump()
    {
        string jump_b = "Jump" + _player.playerID;
        if (Input.GetButtonDown(jump_b))
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
