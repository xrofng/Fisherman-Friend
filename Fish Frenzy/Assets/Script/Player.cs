using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int player;
    public int dPercent;
    public bool death;
    public Vector3 speed;
    public Vector3 jumpForce;
    public float jumpFaster;
    public float fallFaster;
    private float holdToThrow;
    public float aimRadius;
    private bool aiming;
    private bool freezeMovement;

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
    public Collider tree;
    public enum ePart
    {
        body, leftArm,rightArm

    }
    Transform getPart(ePart p)
    {
        int index = (int)p;
        return part[index];
    }
    public Vector3 playerForward
    {
        get
        {
            return -getPart(ePart.body).forward;
        }
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
        this.gameObject.layer = LayerMask.NameToLayer("Player"+player);
        fixedFPS_DT = 0.016f;
        portroyal = FindObjectOfType<PortRoyal>();
        speed = PortRoyal.sCharacterSpeed;
        jumpForce = PortRoyal.sJumpForce;
        fallFaster = PortRoyal.sFallFaster;
        jumpFaster = PortRoyal.sJumpFaster;
        aimRadius = PortRoyal.sAimRadius;
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
                throwFish();
                slapFish();
                checkInput();
                break;
            case eState.fishing:
                startFishing();
                break;
        }
    }
    void FixedUpdate() {
    }
   
    void move()
    {
        string hori = "Hori" + player;
        string verti = "Verti" + player;
        Vector3 mov = new Vector3(Input.GetAxisRaw(hori) * speed.x, 0.0f, Input.GetAxisRaw(verti) * speed.z);
        mov = mov * Time.deltaTime;
        if (!freezeMovement)
        {
            this.transform.Translate(mov);
        }

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
        
        if (playerDirection.sqrMagnitude > 0.0f && !aiming)
        {
            getPart(ePart.body).transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        }

        string jump_b = "Jump" + player;
        if (Input.GetButtonDown(jump_b) && rigid.velocity.y<=0)
        {
            rigid.velocity = Vector3.zero;
            rigid.AddForce(jumpForce, ForceMode.Impulse);
            rigid.drag = jumpFaster;
        }
        if (rigid.velocity.y < 0)
        {
            rigid.velocity += Vector3.up * Physics.gravity.y * fallFaster * Time.deltaTime;
            rigid.drag = 0;
        }
    }
    bool isOwnerFish(Fish f)
    {
        return this.gameObject.name == f.holder.gameObject.name;
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
                GUIManager.Instance.UpdateFishButtonIndicator(player, fishPoint.position,true);
            }
            else
            {
                GUIManager.Instance.UpdateFishButtonIndicator(player, fishPoint.position, false);
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
                        baitedFish.gameObject.layer = LayerMask.NameToLayer("FishO" + player); 
                        baitedFish.setHolder(this.gameObject);
                        GUIManager.Instance.UpdateMashFishingButtonIndicator(player, fishPoint.position, true);
                        changeState(eState.fishing);
                        baitedFish.changeState(1);
                    }
                    break;
                case eState.fishing:
                    if(baitedFish.MashForCatch())
                    {
                        changeState(eState.waitForFish);
                    }
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
            baitedFish = subFish;
            subFish = mainFish;
            subFish.gameObject.SetActive(false);
            mainFish = baitedFish;
            baitedFish = null;
            holdingFish = false;
            if (mainFish != null)
            {
                holdingFish = true;
                mainFish.gameObject.SetActive(true);
            }
        }
    }
    void throwFish()
    {
        string thro = "Throw" + player;
        if (Input.GetButtonDown(thro))
        {
            holdToThrow = 0;
            mainFish.transform.position = getPart(ePart.rightArm).transform.position;
            mainFish.transform.rotation = getPart(ePart.body).transform.rotation;
            freezeMovement = true;
        }
        else if (Input.GetButton(thro))
        {
            holdToThrow += Time.deltaTime;
            //aim assist
           for(int i = 0; i < 4; i++)
            {
                float[] angle = { 1000, 1000, 1000, 1000 };
                Vector3[] direction = new Vector3[4];
                aiming = false;
                if (i+1 != player)
                {
                    Player target = portroyal.player[i];
                     direction[i] = target.transform.position - this.transform.position;
                    angle[i] = Vector3.Angle(direction[i], playerForward);
                    bool found=false;
                    if(angle[i] < aimRadius*0.5f )
                    {
                        found = true;
                        print(target.gameObject.name);
                    }
                    if (found)
                    {
                        aiming = true;
                        int index = sClass.findMinOfArray(angle);
                        getPart(ePart.body).transform.rotation = Quaternion.LookRotation(direction[index], Vector3.up);
                        print(found);
                    }
                }
            }
        }
        else if (Input.GetButtonUp(thro))
        {
            mainFish.transform.position = getPart(ePart.body).transform.position;
            mainFish.transform.rotation = getPart(ePart.body).transform.rotation;
            holdToThrow = Mathf.Clamp(holdToThrow, 0.5f, PortRoyal.sMaxHoldToThrow);
            mainFish.gameObject.layer = LayerMask.NameToLayer("Fish" + player);
            mainFish.FishThrow( holdToThrow);
            mainFish.changeState(4);
            mainFish = null;
            holdingFish = false;
            freezeMovement = false;
            aiming = false;
        }
    }

    void slapFish()
    {

    }
    void checkInput()
    {
        string[] button = { "Fishing", "Switch", "Jump","Slap","Throw" };
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

            case eState.waitForFish: GUIManager.Instance.UpdateMashFishingButtonIndicator(player, fishPoint.position, false);  state = eState.waitForFish; break;
        }

    }
    void recieveDamage(float damage , Fish fish)
    {
        dPercent += (int)damage;
        fish.damageDealed = true;
        rigid.AddExplosionForce(dPercent*50, fish.transform.position, 1.0f, 5.0f,ForceMode.Force);
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
                f.removeRigidBody();
                
                mainFish = f;
                baitedFish = null;
                holdingFish = true;
                state = eState.ground;
                rigid.velocity = Vector3.zero;
                break;

            case 3:
                break;

            case 4:
                if( !isOwnerFish(f) &&!f.damageDealed)
                {
                    rigid.velocity = Vector3.zero;
                    f.removeRigidBody();
                    recieveDamage(f.throwAttack, f );
                    f.fishBounce();
                }
                break;
            case 5:
                break;
        }
    }
    IEnumerator respawn(float waitBeforeRespawn)
    {
       
        yield return new WaitForSeconds(waitBeforeRespawn);
        rigid.velocity = Vector3.zero;
        this.transform.position = portroyal.randomSpawnPosition();
        this.death = false;

    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Fish")
        {
            fishCollideInteraction(other.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StageEdge")
        {
            this.death = true;
            this.transform.position = portroyal.deathRealm.position;
            StartCoroutine(respawn(PortRoyal.sRespawnTime));
        }
    }
     
}
