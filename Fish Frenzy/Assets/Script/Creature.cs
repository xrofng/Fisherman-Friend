using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {
    public enum ColliderType
    {
        Box,
        Capsule
    }

    protected Rigidbody myRigid;
    protected Collider myCollider;
    protected Animation myAnimation;

    protected AudioSource _SFX;

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
    public Collider _collider
    {
        get
        {
            if (!myCollider)
            {
                myCollider = GetComponent<Collider>();

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

    protected virtual void PlaySFX(AudioClip SFXclip)
    {
        if (_SFX.isPlaying) { return; }
        _SFX.clip = SFXclip;
        _SFX.Play();
    }

    public T GetCollider<T>() where T : Collider
    {
        if (!myCollider)
        {
            myCollider = GetComponent<T>() as T;
        }
        return myCollider as T;
    }
}
