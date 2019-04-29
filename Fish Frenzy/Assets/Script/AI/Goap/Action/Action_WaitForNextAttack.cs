using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_WaitForNextAttack", menuName = "Action/WaitForNextAttack", order = 52)]
    public class Action_WaitForNextAttack : Action
    {
        public override void OnActionStart()
        {
            base.OnActionStart();
            Planner.AgentAnimator.ChangeState(3);
        }

    }

}