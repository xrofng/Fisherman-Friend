using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerAbility {
    public Vector3 speed;
    public Vector3 jumpForce;
    public float jumpFaster;
    public float fallFaster;

    public bool freezeMovement;


    private Vector3 lookTo;
    // Use this for initialization
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
       

    }
    // Update is called once per frame
    void Update()
    {
        if (_player.state == Player.eState.ground)
        {
            Move();
            Jump();
        }
    }

    void Move()
    {
        string hori = "Hori" + _player.playerID;
        string verti = "Verti" + _player.playerID;
        Vector3 mov = new Vector3(Input.GetAxisRaw(hori) * speed.x, 0.0f, Input.GetAxisRaw(verti) * speed.z);
        mov = mov * Time.deltaTime;
        if (!freezeMovement && !_player._cPlayerState.IsAttacking)
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
        if (Input.GetButtonDown(jump_b) && (_pState.IsGrounded || _pState.IsSwiming))
        {
            _pRigid.velocity = Vector3.zero;
            _pRigid.AddForce(jumpForce, ForceMode.Impulse);
            _pRigid.drag = jumpFaster;
        }
        if (_pRigid.velocity.y < 0)
        {
            _pRigid.velocity += Vector3.up * Physics.gravity.y * fallFaster * Time.deltaTime;
            _pRigid.drag = 0;
        }
    }
}
