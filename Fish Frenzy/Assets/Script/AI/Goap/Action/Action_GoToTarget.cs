using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Action_GoToTarget", menuName = "Action/GoToTarget", order = 52)]
    public class Action_GoToTarget : Action
    {
        [Header("Properties")]
        public float nearRadius = 4.0f;
        public float moveSpeed = 4.0f;

        protected GameObject _targetEnemy;
        public GameObject TargetEnemy
        {
            get { return _targetEnemy; }
            set { _targetEnemy = value; }
        }

        public void SetProperties(GameObject target, float radius = 4.0f, float speed = 4.0f)
        {
            moveSpeed = speed;
            nearRadius = radius;
            TargetEnemy = target;
        }

        public override void OnActionStart()
        {
            base.OnActionStart();
            Planner.AgentAnimator.ChangeState(1);
        }

        public override void OnActionTick()
        {
            base.OnActionTick();
            Vector3 direction = (TargetEnemy.transform.position - Planner.transform.position).normalized;
            Planner.transform.Translate(direction * Time.deltaTime * moveSpeed);
            if(Vector3.Distance(Planner.transform.position,TargetEnemy.transform.position) < nearRadius)
            {
                OnActionDone();
            }
        }

        public override void OnActionCancel()
        {
            if (Vector3.Distance(Planner.transform.position, TargetEnemy.transform.position) > nearRadius)
            {
                Planner.RemoveCurrentWorldState(this);
            }
        }

        public override bool IsValid()
        {
            return TargetEnemy;
        }

    }

}

