using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishInteraction : PlayerAbility {

    public int beforeHoldFrameDuration;

    // Use this for initialization
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
        
    }

    public void FishCollideInteraction(GameObject g)
    {
        Fish f = g.GetComponent<Fish>();
        switch (f.state)
        {
            case Fish.fState.swim:
                break;

            case Fish.fState.baited:
                break;
            case Fish.fState.toPlayer:
                break;

            case Fish.fState.hold:
                break;

            case Fish.fState.threw:
                if (!isOwnerFish(f) && !f.CheckIgnoredObject(_player.gameObject))
                {
                    _player.rigid.velocity = Vector3.zero;
                    f.RemoveRigidBody();
                    f.AddIgnoreGameObject(_player.gameObject);
                    _player.recieveDamage(f.throwAttack, f.holder ,f.lastHoldPoition, f.t_invicibilityFrame , f.t_launchingDamage);
                    f.fishBounce();
                }
                break;
            case Fish.fState.ground:
                break;
            case Fish.fState.fall:
                break;
        }
    }

    bool isOwnerFish(Fish f)
    {
        return this.gameObject.name == f.holder.gameObject.name;
    }

    public void SetHoldFish(bool b)
    {
        GetCrossZComponent<PlayerThrow>().ChangeToUnAim();
        _player.holdingFish = b;

        _pAnimator.ChangeAnimState((int)_pAnimator.GetIdleAnimation());
    }

    public enum CollideType
    {
        Uncollide,
        Collide_Opponent,
        Collide_All
    }
    public void SetFishCollideType(CollideType fl, Fish fish, Player player)
    {
        if(fl == CollideType.Uncollide)
        {
            fish.gameObject.layer = LayerMask.NameToLayer("Fish_Un");
        }
        if (fl == CollideType.Collide_Opponent)
        {
            fish.gameObject.layer = LayerMask.NameToLayer("Fish" + _player.playerID);
        }
        if (fl == CollideType.Collide_All)
        {
            fish.gameObject.layer = LayerMask.NameToLayer("Fish_All");
        }
    }

    //public void SetFishCollidePlayer(Fish fish, Player player, bool collide)
    //{
    //    string layerN = "FishO";
    //    if (!collide)
    //    {
    //        layerN = "Fish";
    //    }
    //    fish.gameObject.layer = LayerMask.NameToLayer(layerN + _player.playerID);
    //}

    public void SetPlayerCollideEverything( bool collide)
    {
        _player.gameObject.layer = LayerMask.NameToLayer("Player" + _player.playerID);
        if (!collide)
        {
            _player.gameObject.layer = LayerMask.NameToLayer("Player0");
        }
    }


    public void SetMainFishTransformAsPart(Player.ePart transPart, Player.ePart rotatPart, bool flipY)
    {
        _player.mainFish.transform.position = _player.GetPart(transPart).transform.position;
        _player.mainFish.transform.rotation = _player.GetPart(rotatPart).transform.rotation;
        if (flipY)
        {
            _player.mainFish.transform.Rotate(0, 180, 0);
        }
    }

    IEnumerator coroutineFinishFishing;

    public void FinishFishing()
    {
        coroutineFinishFishing = ieFinishFishing(beforeHoldFrameDuration);
        StartCoroutine(coroutineFinishFishing);
    }

    IEnumerator ieFinishFishing(int frameDuration)
    {
        int frameCount = 0;
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        HoldThatFish(_player.baitedFish);
    }

    public void HoldThatFish(Fish f)
    {
        f.ChangeState(Fish.fState.hold);
        f.gameObject.transform.parent = _player.GetPart(Player.ePart.rightArm).transform;
        f.SnapTransform();
        f.RemoveRigidBody();
        f.SetToGround(false);
        f.SetHolder(_player.gameObject);
        _player.mainFish = f;
        _player.baitedFish = null;
        GetCrossZComponent<PlayerFishInteraction>().SetHoldFish(true);
        GetCrossZComponent<PlayerFishInteraction>().SetFishCollideType(PlayerFishInteraction.CollideType.Uncollide, _player.mainFish, _player);
        GetCrossZComponent<PlayerFishing>().SetFishing(false);
        _player.rigid.velocity = Vector3.zero;

        if (f.gameObject.GetComponent<FishSpecialMelee>())
        {
            f.gameObject.GetComponent<FishSpecialMelee>().SetUpFishSpecial();
        }
        if (f.gameObject.GetComponent<FishSpecialSpawn>())
        {
            f.gameObject.GetComponent<FishSpecialSpawn>().SetUpFishSpecial();
        }
        if (f.gameObject.GetComponent<FishSpecialThrow>())
        {
            f.gameObject.GetComponent<FishSpecialThrow>().SetUpFishSpecial();
        }

        if (coroutineFinishFishing != null)
        {
            StopCoroutine(coroutineFinishFishing);
        }
    }
}
