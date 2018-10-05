using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
    public enum fState
    {
        swim=0,
        baited,
        toPlayer,
        hold,
        threw,
        ground
    }
    [Header("Info")]
    public fState state;
    public GameObject holder;
    public bool damageDealed;

    [Header("Fishing")]
    public float deHydration;
    public float weight;
    public int mashCountDown;
    protected Vector3 direction;
    public float jumpForce;
    public float jumpSpeed;
    public float fishMass;
    [Header("Throw")]
    public float throwAttack;
    public int t_invicibilityFrame = 50;
    

    [Header("Slap")]
    public float attack;
    public float attackSpeed;
    public int hitBoxStayFrame = 4;
    public int s_invicibilityFrame = 50;
    public Vector3 hitboxSize;
    public Vector3 hitboxCenter;



    [Header("Snap")]
    //snap
    public Vector3 holdPosition;
    public Vector3 holdRotation;

    private Rigidbody myRigid;
    private BoxCollider myCollider;
    // Use this for initialization
    void Start () {
        myCollider = GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () {
	
    }
    public void playerCollideInteraction(GameObject player)
    {
        
    }
    public bool MashForCatch()
    {
        if (mashCountDown > 0)
        {
            mashCountDown -= 1;
            if (mashCountDown <= 0)
            {
                changeState(fState.toPlayer);
                return true;
            }
        }
        return false;
    }

    public void changeState(fState pState)
    {
        OnStateChange(pState);
        state = pState;
    }

    void OnStateChange(fState pState)
    {
        if (pState == fState.toPlayer)
        {
            direction = holder.transform.position - transform.position;
            FishJump(fishMass, jumpForce, direction, jumpSpeed);
        }
    }

    public void fishBounce()
    {
        StartCoroutine("ieFishBounce");
    }
    private IEnumerator  ieFishBounce()
    {
        yield return new WaitForSeconds(0.0f);
        myRigid = gameObject.AddComponent<Rigidbody>();
        myRigid.AddForce(Vector3.up * 3,ForceMode.Impulse);
    }
    public void snapTransform()
    {
        transform.localPosition = holdPosition;
        transform.localEulerAngles = holdRotation;
    }
    public void setHolder(GameObject g)
    {
        holder = g;
    }
    public void FishJump(float m, float f, Vector3 d,float speed)
    {
        
        gameObject.AddComponent<Rigidbody>();
        myRigid = GetComponent<Rigidbody>();
        myRigid.mass = m;
        d.y = f;
        myRigid.AddForce(d*speed);
      
    }
    public void FishThrow(float duration)
    {
        transform.parent = null;
        gameObject.AddComponent<Rigidbody>();
        myRigid = GetComponent<Rigidbody>();
        float scaleToDuration = duration / PortRoyal.Instance.maxHoldToThrow;
        myRigid.velocity = transform.forward * -(50 * scaleToDuration);
       
        throwAttack = attack * scaleToDuration;
    }

    public void removeRigidBody()
    {
        Destroy(myRigid);
    }
    public BoxCollider getCollider()
    {
        return myCollider;
    }
 
    void OnCollisionEnter(Collision collision)
    {
   
        
    }
}
