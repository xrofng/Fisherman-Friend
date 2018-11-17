using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishInteraction : PlayerAbility {

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
                HoldThatFish(f);

                break;

            case Fish.fState.hold:
                break;

            case Fish.fState.threw:
                if (!isOwnerFish(f) && !f.damageDealed)
                {
                    _player.rigid.velocity = Vector3.zero;
                    f.RemoveRigidBody();
                    f.damageDealed = true;
                    _player.recieveDamage(f.throwAttack, f.holder ,f.lastHoldPoition, f.t_invicibilityFrame);
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
        _player.mainFish = null;
    }

    public void SetFishCollidePlayer(Fish fish, Player player, bool collide)
    {
        string layerN = "FishO";
        if (!collide)
        {
            layerN = "Fish";
        }
        fish.gameObject.layer = LayerMask.NameToLayer(layerN + _player.playerID);
    }


    public void SetMainFishTransformAsPart(Player.ePart transPart, Player.ePart rotatPart, bool flipY)
    {
        _player.mainFish.transform.position = _player.getPart(transPart).transform.position;
        _player.mainFish.transform.rotation = _player.getPart(rotatPart).transform.rotation;
        if (flipY)
        {
            _player.mainFish.transform.Rotate(0, 180, 0);
        }
    }

    public void HoldThatFish(Fish f)
    {
        f.changeState(Fish.fState.hold);
        f.gameObject.transform.parent = _player.getPart(Player.ePart.rightArm).transform;
        f.SnapTransform();
        f.RemoveRigidBody();
        f.SetToGround(false);
        f.setHolder(_player.gameObject);
        SetFishCollidePlayer(f, _player, true);
        _player.mainFish = f;
        _player.baitedFish = null;
        _player.holdingFish = true;
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
    }
}
