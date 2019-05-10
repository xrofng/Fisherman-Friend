using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_FishSpecial", menuName = "Action/Fisherman/FishSpecial", order = 52)]
    public class Action_FishSpecial : Action_Fisherman
    {
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

        public override void OnActionStart()
        {
            base.OnActionStart();
            MMEventManager.TriggerEvent(new PlayerInputDownEvent(PlayerSpecial._pInput.Special, ownerPlayer.playerID - 1));
        }

    }
}