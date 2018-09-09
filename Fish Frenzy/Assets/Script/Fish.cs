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
        threw
    }
    public fState state;
    public GameObject holder;
    public int mashCountDown;
    public float attack;
    public float deHydration;
    public float attackSpeed;
    public float weight;
    public float throwAttack;

    public Vector3 direction;
    public float jumpForce;
    public float jumpSpeed;
    public float fishMass;
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
    public void MashForCatch()
    {
        if (mashCountDown > 0)
        {
            mashCountDown -= 1;
            if (mashCountDown <= 0)
            {
                changeState(2);
            }
        }
   

    }
    public void changeState(int i)
    {
       
        switch (i)
        {
            case 0: state = fState.swim; break;

            case 1: state = fState.baited; break;

            case 2:
                state = fState.toPlayer;
                direction = holder.transform.position - transform.position;
                FishJump(fishMass, jumpForce, direction, jumpSpeed);
                break;

            case 3: state = fState.hold; break;
            case 4: state = fState.threw; break;
        }

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
        myRigid.useGravity = false;
        myRigid.velocity = transform.forward * -50;
        // in dev damage = duration
        throwAttack = duration;

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
