using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_HeadTarget", menuName = "Action/Fisherman/HeadTarget", order = 52)]
    public class Action_HeadTarget : Action_Fisherman
    {
        [Header("Properties")]
        public float rotationSpeed = 0.025f;
        public float faceAngle = 10.0f;
        public float nearRadius = 2.0f;

        private Vector3 targetNonY_Pos;
        private Vector3 plannerNonY_Pos;
        private Vector3 plannerNonY_Forward;

        public GameObject target;
        public Vector3 faceTo;

        public override void OnActionStart()
        {
            base.OnActionStart();
        }

        public override void OnActionTick()
        {
            base.OnActionTick();

            Vector3 direction = DirectionToTarget().normalized;
            Vector3 smoothDirection = Vector3.Slerp(ownerPlayer.PlayerForward, direction, rotationSpeed);

            ownerPlayer._cPlayerMovement.ChangeDirection(smoothDirection.x, smoothDirection.z);

            ownerPlayer._cPlayerMovement.Move(direction);

            if (AngleToTarget() <= faceAngle
                && Vector3.Distance(Planner.transform.position, target.transform.position) < nearRadius)
            {
                OnActionDone();
            }
        }

        protected override bool ActionCancelationCondition()
        {
            return AngleToTarget() > faceAngle || Vector3.Distance(Planner.transform.position, target.transform.position) >= nearRadius;
        }

        protected Vector3 DirectionToTarget()
        {
            // ignore y-axis position
            targetNonY_Pos = sClass.SetVector3(target.transform.position, VectorComponent.y, 0);
            plannerNonY_Pos = sClass.SetVector3(Planner.transform.position, VectorComponent.y, 0);

            Vector3 direction = (targetNonY_Pos - plannerNonY_Pos).normalized;

            return direction;
        }

        protected float AngleToTarget()
        {
            float dot = Vector3.Dot(plannerNonY_Forward + DirectionToTarget(), ownerPlayer.PlayerForward);
            return Mathf.Acos(dot) * Mathf.Rad2Deg;
        }

        public override bool IsValid()
        {
            return target;
        }

    }

}

