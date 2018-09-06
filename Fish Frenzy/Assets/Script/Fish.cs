using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
    public enum fState
    {
        swim=0,
        baited,
        toPlayer,
        hold
    }
    public fState state;
    public GameObject holder;
    public int mashCountDown;
    public Vector3 direction;
   

    public float jumpForce;
    public float jumpSpeed;
    public float fishMass;
    //snap
    public Vector3 holdPosition;
    public Vector3 holdRotation;

    private Rigidbody myRigid;
	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
	
    }
    public void playerCollideInteraction(GameObject player)
    {
        
    }
    public void MashForCatch()
    {
        mashCountDown -= 1;
        if (mashCountDown <= 0)
        {
            changeState(2);
        }
       
    }
    public void changeState(int i)
    {
       switch (i)
        {
            case 0: state = fState.swim;     break;

            case 1: state = fState.baited;   break;
               
            case 2:
                state = fState.toPlayer;
                direction = holder.transform.position - transform.position;
                FishJump(fishMass, jumpForce, direction,jumpSpeed);
                break;

            case 3: state = fState.hold;     break;
        }

    }

    void OnCollisionEnter(Collision other)
    {
      
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

    public void removeRigidBody()
    {
        Destroy(myRigid);
    }

}
