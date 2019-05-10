using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishing : PlayerAbility
{
    public bool nearCoast;

    public Transform fishPoint;
    public Transform fishPoint_finder;
    public int delayFrame;

    [Header("SFX")]
    public SoundEffect sfx_RodSwing;
    public SoundEffect sfx_WaterTouch;
    // Use this for initialization
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        // setup button
        inputName = _pInput.Fishing;
    }

    // Update is called once per frame
    void Update ()
    {
        if (_player.state == Player.eState.ground)
        {
            CoastCheck();
            //Fishing();
            HandleInput();
           
        }
        if (_player.state == Player.eState.fishing)
        {
            //Fishing();
            HandleInput();
        }
    }

    protected override void OnInputDown()
    {
        Fishing();
    }

    void Fishing()
    {
        switch (_player.state)
        {
            case Player.eState.ground:
                if (nearCoast == true && !_player.holdingFish)
                {
                    guiManager.UpdateFishButtonIndicator(_player.playerID, fishPoint.position, false);
                    _player.ChangeState(Player.eState.rodSwinging);
                    StartCoroutine(StartFishing(delayFrame));
                }
                break;
            case Player.eState.fishing:
                if (_player.baitedFish.MashForCatch())
                {
                    _player.baitedFish.fishMeshRenderer.enabled = true;
                    _player.ChangeState(Player.eState.waitForFish);
                    _player.Animation.ChangeAnimState((int)PlayerAnimation.Anim.FishingEnd, true, (int)PlayerAnimation.Anim.HoldFish);

                    GetCrossZComponent<PlayerFishInteraction>().FinishFishing();
                    guiManager.UpdateMashFishingButtonIndicator(_player.playerID, fishPoint.position, false);
                }
                break;
            case Player.eState.waitForFish:
                break;
        }
    }

    IEnumerator StartFishing(int frameDuration)
    {
        PlaySFX(sfx_RodSwing);
        int frameCount = 0;

        _player.Animation.ChangeAnimState((int)PlayerAnimation.Anim.FishingStart);

        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount += 1;
        }
        PlaySFX(sfx_WaterTouch);

        _player.baitedFish = Instantiate(portRoyal.randomFish(), fishPoint.position, _player.GetPart(Player.ePart.body).transform.rotation);
        _player.Animation.ChangeAnimState((int)PlayerAnimation.Anim.Fishing);


        Fish baitedFish = _player.baitedFish;
        baitedFish.fishMeshRenderer.enabled = false;
        GetCrossZComponent<PlayerFishInteraction>().SetFishCollideType(PlayerFishInteraction.CollideType.Uncollide ,baitedFish, _player);
        baitedFish.SetHolder(this.gameObject);
        SetFishing(true);
        baitedFish.ChangeState(Fish.fState.baited);
    }

    void CoastCheck()
    {
        RaycastHit hit;
        nearCoast = false;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(fishPoint_finder.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Color lineColor = Color.yellow;
            if (hit.transform.gameObject.tag == "Sea" && !_player.holdingFish &&
               GetCrossZComponent<PlayerState>().IsGrounded && 
               !GetCrossZComponent<PlayerState>().IsDeath)
            {
                lineColor = Color.blue;
                nearCoast = true;
                fishPoint.position = hit.point + Vector3.down;
                guiManager.UpdateFishButtonIndicator(_player.playerID, fishPoint.position, true);
            }
            else
            {
                guiManager.UpdateFishButtonIndicator(_player.playerID, fishPoint.position, false);
            }
            Debug.DrawRay(fishPoint_finder.position, transform.TransformDirection(Vector3.down) * hit.distance, lineColor);
        }
    }

    public void SetFishing(bool b)
    {
        guiManager.UpdateMashFishingButtonIndicator(_player.playerID, fishPoint.position, b);
        if (b)
        {
            _player.ChangeState(Player.eState.fishing);
            return;
        }
        _player.ChangeState(Player.eState.ground);
    }

    
}
