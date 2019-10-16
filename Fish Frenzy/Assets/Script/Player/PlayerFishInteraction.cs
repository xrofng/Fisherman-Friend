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
                if (!IsOwnerFish(f) && !f.CheckIgnoredObject(Player.gameObject))
                {
                    Player.rigid.velocity = Vector3.zero;
                    f.RemoveRigidBody();
                    f.AddIgnoreGameObject(Player.gameObject);
                    Player.recieveDamage(f.throwAttack, f.holder ,f.lastHoldPoition, f.t_invicibilityFrame , f.t_launchingDamage);
                    f.fishBounce();
                }
                break;
            case Fish.FishConditionalState.ground:
                break;
            case Fish.FishConditionalState.fall:
                break;
        }
    }

    bool IsOwnerFish(Fish f)
    {
        return this.gameObject.name == f.holder.gameObject.name;
    }

    public void SetHoldFish(bool isHoldingFish)
    {
        GetCrossZComponent<PlayerThrow>().ChangeToUnAim();
        Player.holdingFish = isHoldingFish;
        if (!isHoldingFish)
        {
            Player.mainFish = null;
        }
        _pAnimator.ChangeAnimState((int)Player._cPlayerAnimator.GetIdleAnimation());
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
            fish.gameObject.layer = LayerMask.NameToLayer("Fish" + Player.playerID);
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
        Player.gameObject.layer = LayerMask.NameToLayer("Player" + Player.playerID);
        if (!collide)
        {
            Player.gameObject.layer = LayerMask.NameToLayer("Player0");
        }
    }


    public void SetMainFishTransformAsPart(Player.ePart transPart, Player.ePart rotatPart, bool flipY)
    {
        Player.mainFish.transform.position = Player.GetPart(transPart).transform.position;
        Player.mainFish.transform.rotation = Player.GetPart(rotatPart).transform.rotation;
        if (flipY)
        {
            Player.mainFish.transform.Rotate(0, 180, 0);
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
        HoldFish(Player.baitedFish);
    }

    public void HoldFish(Fish fish)
    {
        fish.ChangeState(Fish.FishConditionalState.hold);
        fish.gameObject.transform.parent = Player.GetPart(Player.ePart.rightArm).transform;
        fish.SnapTransform();
        fish.RemoveRigidBody();
        fish.SetToGround(false);
        fish.SetHolder(Player.gameObject);
        Player.mainFish = fish;
        Player.baitedFish = null;
        GetCrossZComponent<PlayerFishInteraction>().SetHoldFish(true);
        GetCrossZComponent<PlayerFishInteraction>().SetFishCollideType(PlayerFishInteraction.CollideType.Uncollide, Player.mainFish, Player);
        GetCrossZComponent<PlayerFishing>().SetFishing(false);
        Player.rigid.velocity = Vector3.zero;

        fish._cSpecial.OnPlayerHold();

        StopCoroutine(ieFinishFishing(beforeHoldFrameDuration));
    }
}
