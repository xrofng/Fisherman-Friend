using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Agent_Fisherman_Planner : AgentPlanner, MMEventListener<PlayerSpawnedEvent>
    {
        public GameObject targetEnemy;

        protected override void Initialize()
        {
            base.Initialize();
            this.MMEventStartListening<PlayerSpawnedEvent>();
            //GetGoal<Goal_KillEnemy>().TargetEnemy = targetEnemy;
            //GetAction<Action_GoToTarget>().SetProperties(targetEnemy, meleeRangeRadius, moveSpeed);
        }

        public void OnMMEvent(PlayerSpawnedEvent eventType)
        {
            startPlanning = true;
        }

        protected override void PlanPreparation()
        {
            base.PlanPreparation();
            if (Time.frameCount % 10 == 0 )
            {
                targetEnemy = agent.Fuzzy.Cast<Agent_Fisherman_Fuzzy>().GetAttackTarget();
            }
        }

    }

}

