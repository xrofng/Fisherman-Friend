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
    public Transform snap;
    //public float veloY;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if(state == fState.toPlayer)
        {
            //veloY -= Time.deltaTime*4;
            
            //this.transform.Translate(Vector3.forward);
            //this.transform.Translate(Vector3.up*veloY);
               
            this.transform.Translate( (direction) *Time.deltaTime);
            
        }
    }
    public void playerCollideInteraction(GameObject player)
    {
        switch ((int)state)
        {
            case 0:
                break;

            case 1:
                break;
            case 2:
                state = fState.hold;
                Player p = player.GetComponent<Player>();
                this.gameObject.transform.parent = p.model.transform;
                p.holdingFish = true;
                p.state = 0;
                break;

            case 3:
                break;
        }
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
                //veloY = 1;
                break;

            case 3: state = fState.hold;     break;
        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Fisherman")
        {
            playerCollideInteraction(other.gameObject);
        }
    }
    
   
}
