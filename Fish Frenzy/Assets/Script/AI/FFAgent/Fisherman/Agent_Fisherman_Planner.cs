using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Agent_Fisherman_Planner : AgentPlanner, MMEventListener<PlayerSpawnedEvent>
    {
        protected Player _ownerPlayer;
        public Player OwnerPlayer
        {
            get
            {
                if (!_ownerPlayer)
                {
                    _ownerPlayer = GetComponent<Player>();
                }
                return _ownerPlayer;
            }
        }

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

        protected override void PreparePlanner()
        {
            base.PreparePlanner();
            targetEnemy = agent.Fuzzy.Cast<Agent_Fisherman_Fuzzy>().GetAttackTarget();
        }

        protected override void PrepareAction(Action action)
        {
            base.PrepareAction(action);

            if (action.GetType() == typeof(Action_FaceTarget))
            {
                GetAction<Action_FaceTarget>().target = targetEnemy;
            }

            if (action.GetType() == typeof(Action_FaceCoast))
            {
                GetAction<Action_FaceCoast>().target = OwnerPlayer._cPlayerEnvironmentInteraction.GetNearestCoast();
            }
        }

        protected override void PrepareGoal(Goal goal)
        {
            base.PrepareGoal(goal);

            if (goal.GetType() == typeof(Goal_KillEnemy))
            {
                GetGoal<Goal_KillEnemy>().TargetEnemy = targetEnemy;
            }
        }



    }

}

