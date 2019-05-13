using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    /// <summary>
    /// Rigid of creature
    /// </summary>
    protected Rigidbody _rigid;
    public Rigidbody Rigidbody
    {
        get
        {
            if (!_rigid)
            {
                if (GetComponent<Rigidbody>() == null)
                {
                    gameObject.AddComponent<Rigidbody>();
                }
                _rigid = GetComponent<Rigidbody>();
            }
            return _rigid;
        }
    }

    /// <summary>
    /// Collider of creature
    /// </summary>
    protected Collider _collider;
    public Collider Collider
    {
        get
        {
            if (!_collider)
            {
                _collider = GetComponent<Collider>();

            }
            return _collider;
        }
    }

    public T GetCollider<T>() where T : Collider
    {
        return Collider as T;
    }

    /// <summary>
    /// Animation of creature
    /// </summary>
    protected CharacterAnimation _animation;
    public CharacterAnimation Animation
    {
        get
        {
            if (!_animation)
            {
                _animation = GetComponent<CharacterAnimation>();
            }
            return _animation;
        }
    }

    public T GetAnimator<T>() where T : CharacterAnimation
    {
        return Animation as T;
    }

    /// <summary>
    /// Renderer of creature
    /// </summary>
    protected MeshRenderer _meshRenderer;
    public MeshRenderer MeshRenderer
    {
        get
        {
            if (!_meshRenderer)
            {
                _meshRenderer = GetComponent<MeshRenderer>();

            }
            return _meshRenderer;
        }
    }
    public void SetMesh(MeshRenderer mr)
    {
        _meshRenderer = mr;
    }
}
