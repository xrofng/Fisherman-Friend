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
            case Fish.FishConditionalState.swim:
                break;

            case Fish.FishConditionalState.baited:
                break;
            case Fish.FishConditionalState.toPlayer:
                break;

            case Fish.FishConditionalState.hold:
                break;

            case Fish.FishConditionalState.threw:
                if (!isOwnerFish(f) && !f.CheckIgnoredObject(_player.gameObject))
                {
                    _player.rigid.velocity = Vector3.zero;
                    f.RemoveRigidBody();
                    f.AddIgnoreGameObject(_player.gameObject);
                    _player.recieveDamage(f.throwAttack, f.holder ,f.lastHoldPoition, f.t_invicibilityFrame , f.t_launchingDamage);
                    f.fishBounce();
                }
                break;
            case Fish.FishConditionalState.ground:
                break;
            case Fish.FishConditionalState.fall:
                break;
        }
    }

    bool isOwnerFish(Fish f)
    {
        return this.gameObject.name == f.holder.gameObject.name;
    }

    public void SetHoldFish(bool isHoldingFish)
    {
        GetCrossZComponent<PlayerThrow>().ChangeToUnAim();
        _player.holdingFish = isHoldingFish;
        if (!isHoldingFish)
        {
            _player.mainFish = null;
        }
        _pAnimator.ChangeAnimState((int)_player._cPlayerAnimator.GetIdleAnimation());
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

    public void FinishFishing(Fish fish)
    {
        ActivateOnFishing activateOnFishing = fish.GetComponent<ActivateOnFishing>();
        if (activateOnFishing)
        {
            activateOnFishing.OnFishingAction();
        }

        IEnumerator coroutineFinishFishing = ieFinishFishing(beforeHoldFrameDuration);
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
        HoldFish(_player.baitedFish);
    }

    public void HoldFish(Fish fish)
    {
        fish.ChangeState(Fish.FishConditionalState.hold);
        fish.gameObject.transform.parent = _player.GetPart(Player.ePart.rightArm).transform;
        fish.SnapTransform();
        fish.RemoveRigidBody();
        fish.SetToGround(false);
        fish.SetHolder(_player.gameObject);
        _player.mainFish = fish;
        _player.baitedFish = null;
        GetCrossZComponent<PlayerFishInteraction>().SetHoldFish(true);
        GetCrossZComponent<PlayerFishInteraction>().SetFishCollideType(PlayerFishInteraction.CollideType.Uncollide, _player.mainFish, _player);
        GetCrossZComponent<PlayerFishing>().SetFishing(false);
        _player.rigid.velocity = Vector3.zero;

        if (fish.gameObject.GetComponent<FishSpecialMelee>())
        {
            fish.gameObject.GetComponent<FishSpecialMelee>().SetUpFishSpecial();
        }
        if (fish.gameObject.GetComponent<FishSpecialSpawn>())
        {
            fish.gameObject.GetComponent<FishSpecialSpawn>().SetUpFishSpecial();
        }
        if (fish.gameObject.GetComponent<FishSpecialThrow>())
        {
            fish.gameObject.GetComponent<FishSpecialThrow>().SetUpFishSpecial();
        }

        StopCoroutine(ieFinishFishing(beforeHoldFrameDuration));
    }
}
