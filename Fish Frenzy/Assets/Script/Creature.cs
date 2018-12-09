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
    /// Audio of creature
    /// </summary>
    protected AudioSource _audioSource;
    public AudioSource SFX
    {
        get
        {
            if (!_audioSource)
            {
                _audioSource = GetComponent<AudioSource>();

            }
            return _audioSource;
        }
    }

    protected virtual void PlaySFX(AudioClip SFXclip)
    {
        if (SFX.isPlaying) { return; }
        SFX.clip = SFXclip;
        SFX.Play();
    }

    /// <summary>
    /// Animation of creature
    /// </summary>
    protected CharacterAnimation _animation;
    public CharacterAnimation Animator
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
        return Animator as T;
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
