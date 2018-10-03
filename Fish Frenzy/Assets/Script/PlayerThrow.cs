using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThrow : PlayerAbility {

    private float holdToThrow;
    public RectTransform aimArrow;
    protected RectTransform _aimArrow;

    private float aimRadius;
    public bool aiming;

    // Use this for initialization
    protected override void Start () {
        
        Initialization();
        
    }

    protected override void Initialization()
    {
        base.Initialization();
        _aimArrow = Instantiate(aimArrow, _player.getPart(Player.ePart.body));
        _aimArrow.GetComponent<Image>().color = PortRoyal.Instance.playerColor[_player.playerID-1];
        aimRadius = PortRoyal.Instance.aimRadius;
    }

        // Update is called once per frame
    void Update () {
        if(_player.state == Player.eState.ground)
        {
            ThrowFish();
        }
    }

    void ThrowFish()
    {
        string thro = "Throw" + _player.playerID;
        if (_player.mainFish == null)
        {
            return;
        }
        if (Input.GetButtonDown(thro))
        {
            holdToThrow = 0;
            _player.SetTransformAsPart(Player.ePart.rightArm, Player.ePart.body);
            _player.freezeMovement = true;
            _aimArrow.gameObject.SetActive(true);
        }
        else if (Input.GetButton(thro))
        {
            holdToThrow += Time.deltaTime;

            //AimAssist();
        }
        else if (Input.GetButtonUp(thro))
        {
            _player.SetTransformAsPart(Player.ePart.body, Player.ePart.body);

            holdToThrow = Mathf.Clamp(holdToThrow, 0.5f, PortRoyal.Instance.maxHoldToThrow);

            _player.SetFishCollidePlayer(_player.mainFish, _player, false);

            _player.mainFish.FishThrow(holdToThrow);
            _player.mainFish.changeState(Fish.fState.threw);
            _player.mainFish = null;
            _player.SetHoldingFish(false);
            _player.freezeMovement = false;
            _aimArrow.gameObject.SetActive(false);
            aiming = false;
        }
    }

    void AimAssist()
    {
        //aim assist
        //for(int i = 0; i < 4; i++)
        // {
        //     float[] angle = { 1000, 1000, 1000, 1000 };
        //     Vector3[] direction = new Vector3[4];
        //     aiming = false;
        //     if (i+1 != player)
        //     {
        //         Player target = portroyal.player[i];
        //          direction[i] = target.transform.position - this.transform.position;
        //         angle[i] = Vector3.Angle(direction[i], playerForward);
        //         bool found=false;
        //         if(angle[i] < aimRadius*0.5f )
        //         {
        //             found = true;
        //             print(target.gameObject.name);
        //         }
        //         if (found)
        //         {
        //             aiming = true;
        //             int index = sClass.findMinOfArray(angle);
        //             getPart(ePart.body).transform.rotation = Quaternion.LookRotation(direction[index], Vector3.up);
        //             print(found);
        //         }
        //     }
        // }
    }
}
