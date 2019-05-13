using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_Fishing", menuName = "Action/Fisherman/Fishing", order = 52)]
    public class Action_Fishing : Action_Fisherman
    {
        private PlayerFishing _cFishing;
        public PlayerFishing PlayerFishing
        {
            get
            {
                if (!_cFishing)
                {
                    _cFishing = ownerPlayer._cPlayerFishing;
                }
                return _cFishing;
            }
        }

        /// TODO use fuzzy calculate
        private float mashingDelay = 0.25f;
        private float mashingDelayCount;

        public override void OnActionStart()
        {
            base.OnActionStart();
            mashingDelayCount = mashingDelay;
        }

        public override void OnActionTick()
        {
            base.OnActionTick();

            mashingDelayCount -= Time.deltaTime;
            if(mashingDelayCount <= 0)
            {
                mashingDelayCount = mashingDelay;
                MMEventManager.TriggerEvent(new PlayerInputDownEvent(PlayerFishing._pInput.Fishing, ownerPlayer.playerID - 1));
            }

            if (ownerPlayer.holdingFish)
            {
                OnActionDone();
            }
        }

        protected override bool ActionCancelationCondition()
        {
            return !ownerPlayer.holdingFish;
        }

    }

}

