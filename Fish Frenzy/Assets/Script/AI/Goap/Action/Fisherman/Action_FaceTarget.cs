using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_FaceTarget", menuName = "Action/FaceTarget", order = 52)]
    public class Action_FaceTarget : Action
    {
        [Header("Properties")]
        public float rotationSpeed = 5.0f;
        public float faceAngle = 10.0f;

        private Vector3 targetNonY_Pos;
        private Vector3 plannerNonY_Pos;
        private Vector3 plannerNonY_Forward;

        public GameObject target;

        public override void OnActionStart()
        {
            base.OnActionStart();
            //Planner.AgentAnimator.ChangeState(1);
        }

        public override void OnActionTick()
        {
            base.OnActionTick();
            Player fisherman = Planner.GetAgent<Agent_Fisherman>().OwnerPlayer;

            // ignore y-axis position
            targetNonY_Pos = sClass.SetVector3(target.transform.position, VectorComponent.y, 0);
            plannerNonY_Pos = sClass.SetVector3(Planner.transform.position, VectorComponent.y, 0);

            Vector3 dir = (targetNonY_Pos - plannerNonY_Pos).normalized;

            Vector3 dirr = Vector3.Slerp(fisherman.PlayerForward, dir, rotationSpeed);

            fisherman._cPlayerMovement.ChangeDirection(dirr.x, dirr.z);

            float dot = Vector3.Dot(dirr, fisherman.PlayerForward);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angle <= faceAngle)
            {
                OnActionDone();
            }
        }

        public override void OnActionCancel()
        {
            if (Vector3.Angle(targetNonY_Pos, plannerNonY_Forward) > faceAngle)
            {
                Planner.RemoveCurrentWorldState(this);
            }
        }

        public override bool IsValid()
        {
            return target;
        }

    }

}

