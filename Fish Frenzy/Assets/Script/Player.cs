using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private int player;
    public Vector3 speed;
    public Vector3 jumpForce;
    public static float fixedFPS_DT;
    private Rigidbody rigid;
    public bool nearCoast;
    public Transform model;
    private Vector3 lookTo;
    public Transform fishPoint_finder;
    public Transform fishPoint;

    private PortRoyal portroyal;
    // Use this for initialization
    void Start() {
        player = gameObject.name[6] - 48;
        fixedFPS_DT = 0.016f;
        portroyal = FindObjectOfType<PortRoyal>();
        speed = PortRoyal.sCharacterSpeed;
        jumpForce = PortRoyal.sJumpForce;
        rigid = GetComponent<Rigidbody>();
        rigid.mass = PortRoyal.sCharMass;
        //model.transform.Rotate(0, 180, 0, Space.World);

    }

    // Update is called once per frame
    void Update() {
        move();
        coastCheck();
        startFishing();
        action();
    }
    void FixedUpdate() {

    }
   
    void move() {

        string hori = "Hori" + player;
        string verti = "Verti" + player;

        Vector3 mov = new Vector3(Input.GetAxisRaw(hori) * speed.x, 0.0f, Input.GetAxisRaw(verti) * speed.z);
        mov = mov * Time.deltaTime;
        this.transform.Translate(mov);

        float axisRawX = Input.GetAxisRaw(hori);
        float axisRawY = Input.GetAxisRaw(verti);
        Vector3 playerDirection = lookTo;
        if (sClass.getSign(Input.GetAxis(hori), 0.015f) != 0 ||sClass.getSign(Input.GetAxis(verti), 0.015f) != 0)
        {
            if (sClass.intervalCheck(axisRawX, -0.9f, 0.9f, true) || sClass.intervalCheck(axisRawY, -0.9f, 0.9f, true))
            {
                playerDirection = Vector3.right * -axisRawX + Vector3.forward * -axisRawY;
                lookTo = playerDirection;
            }
        }


        if (playerDirection.sqrMagnitude > 0.0f)
        {
            model.transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        }

        string jump_b = "Jump" + player;
        if (Input.GetButtonDown(jump_b))
        {
            rigid.velocity = Vector3.zero;
            rigid.AddForce(jumpForce);
        }



    }
    void action()
    {

    }
    void coastCheck()
    {
        RaycastHit hit;
        nearCoast = false;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(fishPoint_finder.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Color lineColor = Color.yellow;
            if (hit.transform.gameObject.tag == "Sea")
            {
                lineColor = Color.blue;
                nearCoast = true;
                fishPoint.position = hit.point + Vector3.down;
                fishPoint.gameObject.SetActive(true);
            }
            else
            {
                fishPoint.gameObject.SetActive(false);
            }
            Debug.DrawRay(fishPoint_finder.position, transform.TransformDirection(Vector3.down) * hit.distance, lineColor);
        }
        else
        {
            Debug.DrawRay(fishPoint_finder.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
      
        }
    }
   
    void startFishing()
    {
        string fishi = "Fishing" + player;
        if (Input.GetButtonDown(fishi))
        {
            if (nearCoast == true)
            {
                Fish got = Instantiate(portroyal.randomFish(), fishPoint.position, model.transform.rotation);
                got.holder = this.gameObject;
                // in dev start with toPlayer
                got.changeState(2) ;

            }
        }
  
    }

    void OnCollisionEnter(Collision other)
    {
       
    }
    
}
