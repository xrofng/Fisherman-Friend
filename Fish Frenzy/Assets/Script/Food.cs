using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {



    public int restorePercent = 10;

    private Rigidbody myRigid;

    private BoxCollider myCollider;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        myCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        CheckGround();
    }

    public Vector3 getLowestFoodPoint()
    {
        return new Vector3(transform.position.x, transform.position.y  - myCollider.bounds.size.y / 2.0f, transform.position.z);
    }

    void CheckGround()
    {

        RaycastHit hit;
        if (Physics.Raycast(getLowestFoodPoint(), transform.TransformDirection(Vector3.down), out hit, 0.5f))
        {
            Color lineColor = Color.yellow;
            if (hit.transform.gameObject.tag == "Ground" || ( hit.transform.gameObject.tag == "Sea" && hit.transform.gameObject.layer == LayerMask.NameToLayer( "Default")) )
            {
                RemoveRigidBody();
                myCollider.isTrigger = true;
            }
        }
    }

    public void RemoveRigidBody()
    {
        Destroy(myRigid);
    }

}
