using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_FishSpecial", menuName = "Action/Fisherman/FishSpecial", order = 52)]
    public class Action_FishSpecial : Action_Fisherman
    {
        FFRandom autoThrowFrame;

        private PlayerSpecial _cSpecial;
        public PlayerSpecial PlayerSpecial
        {
            get
            {
                if (!_cSpecial)
                {
                    _cSpecial = ownerPlayer._cPlayerSpecial;
                }
                return _cSpecial;
            }
        }

        

        public override void OnActionInit()
        {
            base.OnActionInit();
            
        }

        public override void OnActionStart()
        {
            base.OnActionStart();
            if (PlayerSpecial.Player.mainFish)
            {
                autoThrowFrame = new FFRandom(0, PlayerSpecial.Player.mainFish.durability / Time.deltaTime);
                MMEventManager.TriggerEvent(new PlayerInputEvent(PlayerSpecial._pInput.Special, ownerPlayer.playerID - 1, autoThrowFrame.RandomInt()));
            }
        }

    }
}