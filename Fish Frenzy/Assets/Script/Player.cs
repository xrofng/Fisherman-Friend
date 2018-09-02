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

    public bool holdingFish;
    public Fish mainFish;
    public Fish subFish;
    public Fish baitedFish;
    
    private Vector3 lookTo;
    public Transform fishPoint_finder;
    public Transform fishPoint;

    private PortRoyal portroyal;
    private BoxCollider myCollider;

    public Transform[] part;
    public enum ePart
    {
        body, leftArm,rightArm

    }
    Transform getPart(ePart p)
    {
        int index = (int)p;
        return part[index];
    }
    public enum eState
    {
        ground,
        air,
        water,
        fishing,
        waitForFish

    }
    public eState state;
    // Use this for initialization
    void Start() {
        player = gameObject.name[6] - 48;
        fixedFPS_DT = 0.016f;
        portroyal = FindObjectOfType<PortRoyal>();
        speed = PortRoyal.sCharacterSpeed;
        jumpForce = PortRoyal.sJumpForce;
        rigid = GetComponent<Rigidbody>();
        rigid.mass = PortRoyal.sCharMass;
        myCollider = GetComponent<BoxCollider>();
        myCollider.size = getPart(ePart.body).transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        switch (state)
        {
            case eState.ground:
                move();
                coastCheck();
                switchFish();
                startFishing();
                checkInput();
                break;
            case eState.fishing:
                startFishing();
                break;
        }
        
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
            getPart(ePart.body).transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        }

        string jump_b = "Jump" + player;
        if (Input.GetButtonDown(jump_b))
        {
            rigid.velocity = Vector3.zero;
            rigid.AddForce(jumpForce);
        }
    }

 
    void coastCheck()
    {
        RaycastHit hit;
        nearCoast = false;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(fishPoint_finder.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Color lineColor = Color.yellow;
            if (hit.transform.gameObject.tag == "Sea" && !holdingFish)
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
            switch (state)
            {
                case eState.ground:
                    if (nearCoast == true && !holdingFish)
                    {
                        baitedFish = Instantiate(portroyal.randomFish(), fishPoint.position, getPart(ePart.body).transform.rotation);
                        baitedFish.setHolder(this.gameObject);
                        state = eState.fishing;
                        baitedFish.changeState(1);
                    }
                    break;
                case eState.fishing:
                    baitedFish.MashForCatch();
                    
                    break;
                case eState.waitForFish:
                    break;
            }
        }
    }
    void switchFish()
    {
        string switc = "Switch" + player;
        if (Input.GetButtonDown(switc))
        {
         
        }
    }
    void checkInput()
    {
        string[] button = { "Fishing", "Switch", "Jump" };
        int numPlayer = 4;
        for (int i = 0; i < button.Length; i++)
        {
            for (int j = 1; j < numPlayer+1; j++)
            {
                string bitton = button[i] + j;
                if (Input.GetButtonDown(bitton))
                {
                    print(bitton);
                }
            }
        }
    }
    public void changeState(eState staTE)
    {

        switch (staTE)
        {
            case eState.ground:  state = eState.ground; break;

            case eState.air:  state = eState.air; break;

            case eState.water:  state = eState.water;  break;

            case eState.fishing: state = eState.fishing; break;

            case eState.waitForFish: state = eState.waitForFish; break;
        }

    }
    void fishCollideInteraction(GameObject g)
    {
        Fish f = g.GetComponent<Fish>(); 
        switch ((int)f.state)
        {
            case 0:
                break;

            case 1:
                break;
            case 2:
                f.changeState(3);

                f.gameObject.transform.parent = getPart(ePart.rightArm).transform;
                f.snapTransform();

                holdingFish = true;
                state = eState.ground;
                f.removeRigidBody();
                break;

            case 3:
                break;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Fish")
        {
            print("F");
            fishCollideInteraction(other.gameObject);
        }
    }
    
}
