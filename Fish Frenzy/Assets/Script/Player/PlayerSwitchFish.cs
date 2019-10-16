using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchFish : PlayerAbility
{
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
        if (Player.state == Player.eState.ground)
        {
            if (Player.IgnoreInputForAbilities || IgnoreInput)
            {
                return;
            }
            if (GetCrossZComponent<PlayerSpecial>().GetSpecialing())
            {
                return;
            }
            SwitchFish();
        }

    }

    void SwitchFish()
    {
        if (_pInput.GetButtonDown(_pInput.Switch, Player.playerID - 1))
        {
            Player.baitedFish = Player.subFish;
            Player.subFish = Player.mainFish;
            if (Player.subFish != null)
            {
                Player.subFish.KeepFish(true);
            }

            Player.mainFish = Player.baitedFish;
            Player.baitedFish = null;

            if (Player.mainFish != null)
            {
                Player.mainFish.KeepFish(false);
            }
            GetCrossZComponent<PlayerFishInteraction>().SetHoldFish(Player.mainFish != null);
        }
    }
}
