using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private int player;
    public Vector3 speed;
    public Vector3 jumpForce;
    public static float fixedFPS_DT;
    private Rigidbody rigid;
	// Use this for initialization
	void Start () {
        player = gameObject.name[6] - 48;
        fixedFPS_DT = 0.016f;
        speed = PortRoyal.sCharacterSpeed;
        jumpForce = PortRoyal.sJumpForce;
        rigid = GetComponent<Rigidbody>();
        rigid.mass = PortRoyal.sCharMass;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	void FixedUpdate () {
		move();
       
	}
	void move(){
        string hori = "Hori" + player;
        string verti = "Verti" + player;
        Vector3 mov = new Vector3(Input.GetAxisRaw(hori) * speed.x , 0.0f, Input.GetAxisRaw(verti) * speed.z);
        
        mov = mov * Time.deltaTime;
        //   mov = mov * fixedFPS_DT;
       
        this.transform.Translate(mov);
        string jump_b = "Jump" + player;
        if (Input.GetButtonDown(jump_b) )
        {
            rigid.velocity = Vector3.zero;
            rigid.AddForce(jumpForce);
        }
       
	}
    void jump()
    {

    }
}
