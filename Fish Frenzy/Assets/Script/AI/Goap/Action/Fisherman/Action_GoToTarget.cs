using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_GoToTarget", menuName = "Action/Fisherman/GoToTarget", order = 52)]
    public class Action_GoToTarget : Action
    {
        [Header("Properties")]
        public float nearRadius = 4.0f;

        public GameObject target;

        private Vector3 targetNonY_Pos;
        private Vector3 plannerNonY_Pos;

        public override void OnActionStart()
        {
            base.OnActionStart();
        }

        public override void OnActionTick()
        {
            base.OnActionTick();
            Player fisherman = Planner.agent.CastAgent<Agent_Fisherman>().OwnerPlayer;

            // ignore y-axis position
            targetNonY_Pos = sClass.SetVector3(target.transform.position, VectorComponent.y, 0);
            plannerNonY_Pos = sClass.SetVector3(Planner.transform.position, VectorComponent.y, 0);

            Vector3 dir = (targetNonY_Pos - plannerNonY_Pos).normalized;

            fisherman._cPlayerMovement.Move(dir);

            if(Vector3.Distance(Planner.transform.position,target.transform.position) < nearRadius)
            {
                OnActionDone();
            }
        }

        protected override bool ActionCancelationCondition()
        {
            return Vector3.Distance(Planner.transform.position, target.transform.position) > nearRadius;
        }

        public override bool IsValid()
        {
            return target;
        }

    }

}

