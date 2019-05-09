using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_FaceTarget", menuName = "Action/Fisherman/FaceTarget", order = 52)]
    public class Action_Fisherman : Action
    {
        protected Player ownerPlayer;

        public override void OnActionInit()
        {
            base.OnActionInit();
            ownerPlayer = Planner.agent.CastAgent<Agent_Fisherman>().OwnerPlayer;
        }

        public override bool IsValid()
        {
            return ownerPlayer;
        }
    }

}

