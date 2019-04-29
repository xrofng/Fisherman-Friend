using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Goal_KillEnemy", menuName = "Goal/KillEnemy", order = 50)]
    public class Goal_KillEnemy : Goal
    {
        protected GameObject _targetEnemy;
        public GameObject TargetEnemy
        {
            get { return _targetEnemy; }
            set { _targetEnemy = value; }
        }

        public override bool IsValid()
        {
            return base.IsValid() && TargetEnemy;
        }
    }

}

