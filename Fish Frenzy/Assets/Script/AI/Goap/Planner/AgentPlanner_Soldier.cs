using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class AgentPlanner_Soldier : AgentPlanner
    {
        public GameObject targetEnemy;
        public float moveSpeed = 4.0f;
        public float meleeRangeRadius = 4.0f;

        protected override void Initialize()
        {
            base.Initialize();
            GetGoal<Goal_KillEnemy>().TargetEnemy = targetEnemy;
            GetAction<Action_GoToTarget>().SetProperties(targetEnemy, meleeRangeRadius, moveSpeed);

        }
    }

}

