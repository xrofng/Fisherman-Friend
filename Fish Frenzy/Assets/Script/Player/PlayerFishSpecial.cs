using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishSpecial : PlayerAbility
{
    public int ignoreSpecialFrame = 4;
    
    public T FishSpecial<T>() where T : FishSpecial
    {
        return _player.mainFish._cSpecial as T;
    }

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

    [Header("Melee")]
    public Transform hitBoxParent;
    public HitBoxMelee specialHitBox;
    public Animation specialTrail;
    protected bool mSpecialing;
    public bool MeleeSpecialing
    {
        get { return mSpecialing; }
        set { mSpecialing = value; }
    }
    void SpecialMelee(string special)
    {
        if (!_player.mainFish.GetComponent<FishSpecialMelee>() || MeleeSpecialing)
        {
            return;
        }

        if (_pInput.GetButtonDown(_pInput.Fishing, _player.playerID - 1))
        {
            // Ignore Input
            ActionForFrame(FishSpecial<FishSpecialMelee>().SpeiclaClipFrameCount + ignoreSpecialFrame,
                  () => { MeleeSpecialing = true;  },
                  () => { MeleeSpecialing = false;  });
            // enable trail
            ActionForFrame(FishSpecial<FishSpecialMelee>().SpeiclaClipFrameCount,
                  () => { specialTrail.gameObject.SetActive(true); specialTrail.Play(); },
                  () => { specialTrail.gameObject.SetActive(false); specialTrail.Stop(); });

            _pAnimator.ChangeAnimState((int)PlayerAnimation.State.V_Slap, FishSpecial<FishSpecialMelee>().SpeiclaClipFrameCount, true, (int)PlayerAnimation.State.HoldFish);             
        }   
    }

    [Header("Throw")]
    protected bool tSpecialing;
    private bool finishThrow;
    private MovingObject currentMovingObj;
    void SpecialThrow(string special)
    {
        if (!_player.mainFish.GetComponent<FishSpecialThrow>() || ThrowSpecialing)
        {
            return;
        }       

        if (_pInput.GetButtonDown(_pInput.Fishing, _player.playerID - 1))
        {
            GetCrossZComponent<PlayerThrow>().OnButtonDown();
        }
        else if (_pInput.GetButton(_pInput.Fishing, _player.playerID - 1))
        {
            GetCrossZComponent<PlayerThrow>().OnButtonHold();
        }
        else if (_pInput.GetButtonUp(_pInput.Fishing, _player.playerID - 1))
        {
            //PlayThrowSFX();
            StartCoroutine(SpecialThrowAttack(FishSpecial<FishSpecialThrow>().throwDurationFrame, FishSpecial<FishSpecialThrow>().endByFrame));
            GetCrossZComponent<PlayerThrow>().ChangeToUnAim();
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
                currentMovingObj = Instantiate(FishSpecial<FishSpecialThrow>().movingobject, this.transform.position,Quaternion.identity);// add pos set lyr
                currentMovingObj.gameObject.layer = LayerMask.NameToLayer("Fish" + _player.playerID);
                currentMovingObj.HitBox.Owner = this.gameObject;
                currentMovingObj.HitBox._SFXclip = sfx_Special;
                currentMovingObj.direction = currentMovingObj.HitBox.OwnerPlayer.GetPart(Player.ePart.body).transform.TransformDirection(-Vector3.forward);
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

    [Header("SFX")]
    public AudioClip sfx_Special;
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

    [Header("Debug")]
    public bool showHitBox;
}
