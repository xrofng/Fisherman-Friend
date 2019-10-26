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
        if (Player.state == Player.eState.ground)
        {
            CoastCheck();
            //Fishing();
            HandleInput();
           
        }
        if (Player.state == Player.eState.fishing)
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
        switch (Player.state)
        {
            case Player.eState.ground:
                if (nearCoast == true && !Player.holdingFish)
                {
                    guiManager.UpdateFishButtonIndicator(Player.playerID, fishPoint.position, false);
                    Player.ChangeState(Player.eState.rodSwinging);
                    Player.Animation.TriggerAnimation("f_startfishing");
                    StartCoroutine(StartFishing(delayFrame));
                }
                break;
            case Player.eState.fishing:
                if (Player.baitedFish.MashForCatch())
                {
                    GetCrossZComponent<PlayerFishInteraction>().FinishFishing(Player.baitedFish);

                    Player.baitedFish.fishMeshRenderer.enabled = true;
                    Player.ChangeState(Player.eState.waitForFish);
                    Player.Animation.TriggerAnimation("f_endfishing");

                    guiManager.UpdateMashFishingButtonIndicator(Player.playerID, fishPoint.position, false);
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

        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount += 1;
        }
        PlaySFX(sfx_WaterTouch);

        Player.baitedFish = Instantiate(portRoyal.RandomFish(), fishPoint.position, Player.GetPart(Player.ePart.body).transform.rotation);
        Player.Animation.ChangeAnimState((int)PlayerAnimation.Anim.Fishing);


        Fish baitedFish = Player.baitedFish;
        baitedFish.fishMeshRenderer.enabled = false;
        GetCrossZComponent<PlayerFishInteraction>().SetFishCollideType(PlayerFishInteraction.CollideType.Uncollide ,baitedFish, Player);
        baitedFish.SetHolder(this.gameObject);
        SetFishing(true);
        baitedFish.ChangeState(Fish.FishConditionalState.baited);
    }

    void CoastCheck()
    {
        RaycastHit hit;
        nearCoast = false;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(fishPoint_finder.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Color lineColor = Color.yellow;
            if (hit.transform.gameObject.tag == "Sea" && !Player.holdingFish &&
               GetCrossZComponent<PlayerState>().IsGrounded && 
               !GetCrossZComponent<PlayerState>().IsDeath)
            {
                lineColor = Color.blue;
                nearCoast = true;
                fishPoint.position = hit.point + Vector3.down;
                guiManager.UpdateFishButtonIndicator(Player.playerID, fishPoint.position, true);
            }
            else
            {
                guiManager.UpdateFishButtonIndicator(Player.playerID, fishPoint.position, false);
            }
            Debug.DrawRay(fishPoint_finder.position, transform.TransformDirection(Vector3.down) * hit.distance, lineColor);
        }
    }

    public void SetFishing(bool b)
    {
        guiManager.UpdateMashFishingButtonIndicator(Player.playerID, fishPoint.position, b);
        if (b)
        {
            Player.ChangeState(Player.eState.fishing);
            return;
        }
        Player.ChangeState(Player.eState.ground);
    }

    
}
