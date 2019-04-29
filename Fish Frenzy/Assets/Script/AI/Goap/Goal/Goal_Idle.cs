using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "Goal_Idle", menuName = "Goal/Idle", order = 50)]
    public class Goal_Idle : Goal
    {
        public override bool IsValid()
        {
            return base.IsValid();
        }
    }

}

