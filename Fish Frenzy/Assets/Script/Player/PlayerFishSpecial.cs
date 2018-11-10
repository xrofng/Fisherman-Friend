﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishSpecial : PlayerAbility
{
    public enum FishSpecialType
    {
        Melee,
        Throw,
        Spawn
    }

    [Header("Melee")]
    public Transform hitBoxParent;
    public HitBoxMelee specialHitBox;
    public Animation specialTrail;
    protected bool mSpecialing;

    [Header("Throw")]
    protected bool tSpecialing;
    private bool finishThrow;
    
    private MovingObject currentMovingObj;


    [Header("SFX")]
    public AudioClip sfx_Special;

    [Header("Debug")]
    public bool showHitBox;

    [Header("Fish Special Ref")]
    FishSpecialThrow _fishThrow;
    FishSpecialMelee _fishMelee;
    
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.state == Player.eState.ground)
        {
            if (_player.IgnoreInputForAbilities || IgnoreInput)
            {
                return;
            }
            SpecialFish();
        }
    }

    // Update is called once per frame
    void SpecialFish()
    {
        string special = "Fishing" + _player.playerID;
        if (_player.mainFish == null || !_player.mainFish.GetComponent<FishSpecial>())
        {
            return;
        }
        SpecialMelee(special);
        SpecialThrow(special);
    }

    void SpecialMelee(string special)
    {
        if (!_player.mainFish.GetComponent<FishSpecialMelee>() || MeleeSpecialing)
        {
            return;
        }

        if (Input.GetButtonDown(special))
        {
                _fishMelee = _player.mainFish._cSpecial as FishSpecialMelee;
                StartCoroutine(SpecialMeleeAttack(_fishMelee.hitBoxStayFrame));                
        }   
    }

    void SpecialThrow(string special)
    {
        if (!_player.mainFish.GetComponent<FishSpecialThrow>() || ThrowSpecialing)
        {
            return;
        }       

        if (Input.GetButtonDown(special))
        {
            _fishThrow = _player.mainFish._cSpecial as FishSpecialThrow;
            GetCrossZComponent<PlayerThrow>().OnButtonDown();
        }
        else if (Input.GetButton(special))
        {
            GetCrossZComponent<PlayerThrow>().OnButtonHold();
        }
        else if (Input.GetButtonUp(special))
        {
            //PlayThrowSFX();
            print(_fishThrow);
            StartCoroutine(SpecialThrowAttack(_fishThrow.throwDurationFrame, _fishThrow.endByFrame));
            GetCrossZComponent<PlayerThrow>().ChangeToUnAim();
        }        
    }

    public void PlaySlapSFX()
    {
        if (_player.mainFish.sfx_Special)
        {
            PlaySFX(_player.mainFish.sfx_Special);
        }
        else
        {
            PlaySFX(sfx_Special);
        }
    }

    IEnumerator SpecialMeleeAttack(int frameDuration)
    {
        MeleeSpecialing = true;
        int frameCount = 0;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        MeleeSpecialing = false;
    }

    public bool MeleeSpecialing
    {
        get
        {
            return mSpecialing;
        }
        set
        {
            mSpecialing = value;
            specialHitBox.GetCollider<MeshCollider>().enabled = value;
            specialTrail.Play();
            specialTrail.gameObject.SetActive(value);
            if (showHitBox)
            {
                specialHitBox.GetMeshRenderer().enabled = value;
            }
        }
    }

    IEnumerator SpecialThrowAttack(int frameDuration , bool endByFrame)
    {
        ThrowSpecialing = true;
        if (endByFrame)
        {
            int frameCount = 0;
            while (frameCount < frameDuration)
            {
                yield return new WaitForEndOfFrame();
                frameCount++;
            }
            ThrowSpecialing = false;
        }else
        {
            while (!currentMovingObj.MoveEnd)
            {
                yield return new WaitForEndOfFrame();
            }
            ThrowSpecialing = false;            
        }       
    }

    public bool ThrowSpecialing
    {
        get
        {
            return tSpecialing;
        }
        set
        {
            if (value)
            {
                currentMovingObj = Instantiate(_fishThrow.movingobject, this.transform.position,Quaternion.identity);// add pos set lyr
                currentMovingObj.gameObject.layer = LayerMask.NameToLayer("Fish" + _player.playerID);
                currentMovingObj.HitBox.Owner = this.gameObject;
                currentMovingObj.HitBox._SFXclip = sfx_Special;
                currentMovingObj.direction = currentMovingObj.HitBox.OwnerPlayer.getPart(Player.ePart.body).transform.TransformDirection(-Vector3.forward);
                //spanw
                // tick move it
            }
            else
            {
                Destroy(currentMovingObj.gameObject);
                //destroy specialMovingHitBox
            }
            tSpecialing = value;
        }
    }
}
