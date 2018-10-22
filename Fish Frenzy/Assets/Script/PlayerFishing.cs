using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishing : PlayerAbility
{


    public bool nearCoast;

    public Transform fishPoint;
    public Transform fishPoint_finder;
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
    void Update () {
        if (_player.state == Player.eState.ground)
        {
            coastCheck();
            Fishing();
        }
        if (_player.state == Player.eState.fishing)
        {
            Fishing();
        }
    }


    void Fishing()
    {
        string fishi = "Fishing" + _player.playerID;

        if (Input.GetButtonDown(fishi))
        {
            switch (_player.state)
            {
                case Player.eState.ground:
                    if (nearCoast == true && !_player.holdingFish)
                    {
                        _player.baitedFish = Instantiate(PortRoyal.Instance.randomFish(), fishPoint.position, _player.getPart(Player.ePart.body).transform.rotation);
                        Fish baitedFish = _player.baitedFish;
                        baitedFish.GetComponent<MeshRenderer>().enabled = false;
                        _player.SetFishCollidePlayer(baitedFish, _player, true);
                        baitedFish.setHolder(this.gameObject);
                        SetFishing(true);
                        baitedFish.changeState(Fish.fState.baited);
                    }
                    break;
                case Player.eState.fishing:
                    if (_player.baitedFish.MashForCatch())
                    {
                        _player.baitedFish.GetComponent<MeshRenderer>().enabled = true;
                        _player.changeState(Player.eState.waitForFish);
                        GUIManager.Instance.UpdateMashFishingButtonIndicator(_player.playerID, fishPoint.position, false);
                    }
                    break;
                case Player.eState.waitForFish:

                    break;
            }
        }
    }

    void coastCheck()
    {
        RaycastHit hit;
        nearCoast = false;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(fishPoint_finder.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Color lineColor = Color.yellow;
            if (hit.transform.gameObject.tag == "Sea" && !_player.holdingFish && 
                _player._cPlayerState.IsGrounded && 
                !_player._cPlayerState.IsDeath)
            {
                lineColor = Color.blue;
                nearCoast = true;
                fishPoint.position = hit.point + Vector3.down;
                GUIManager.Instance.UpdateFishButtonIndicator(_player.playerID, fishPoint.position, true);
            }
            else
            {
                GUIManager.Instance.UpdateFishButtonIndicator(_player.playerID, fishPoint.position, false);
            }
            Debug.DrawRay(fishPoint_finder.position, transform.TransformDirection(Vector3.down) * hit.distance, lineColor);
        }
    }

    public void SetFishing(bool b)
    {
        GUIManager.Instance.UpdateMashFishingButtonIndicator(_player.playerID, fishPoint.position, b);
        if (b)
        {
            _player.changeState(Player.eState.fishing);
            return;
        }
        _player.changeState(Player.eState.ground);
    }
}
