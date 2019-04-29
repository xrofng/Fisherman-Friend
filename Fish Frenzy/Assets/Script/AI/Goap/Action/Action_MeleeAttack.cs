using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_MeleeAttack", menuName = "Action/MeleeAttack", order = 52)]
    public class Action_MeleeAttack : Action
    {
        public override void OnActionStart()
        {
            base.OnActionStart();
            Planner.AgentAnimator.ChangeStateOnce(2,10);
        }

    }

}