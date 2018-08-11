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
    private float veloY;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if(state == fState.toPlayer)
        {
            veloY -= Time.deltaTime*4;
            if( transform.position.x != holder.transform.position.x &&
                transform.position.y != holder.transform.position.y &&
                transform.position.z != holder.transform.position.z)
            {
                this.transform.Translate(Vector3.forward);
                this.transform.Translate(Vector3.up*veloY);
            }
        }
	}
    void playerCollideInteraction(GameObject player)
    {
        switch ((int)state)
        {
            case 0:
                break;

            case 1:

                break;

            case 2:
                state = fState.hold;
                this.gameObject.transform.parent = player.transform;
                break;

            case 3:
                break;
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
                Vector3 dire = holder.transform.position - transform.position;
                veloY = 1;
                
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
