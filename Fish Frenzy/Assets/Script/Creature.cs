using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    private Rigidbody myRigid;
    private CapsuleCollider myCollider;
    private Animation myAnimation;

    public Rigidbody _rigidbody
    {
        get
        {
            if (!myRigid)
            {
                myRigid = GetComponent<Rigidbody>();

            }
            return myRigid;
        }
    }
    public CapsuleCollider _collider
    {
        get
        {
            if (!myCollider)
            {
                myCollider = GetComponent<CapsuleCollider>();

            }
            return myCollider;
        }
    }
    public Animation _animation
    {
        get
        {
            if (!myAnimation)
            {
                myAnimation = GetComponent<Animation>();

            }
            return myAnimation;
        }
    }
}
