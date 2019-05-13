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
            StartPlan();
        }

        protected override void PreparePlanner()
        {
            base.PreparePlanner();
            targetEnemy = agent.Fuzzy.Cast<Agent_Fisherman_Fuzzy>().GetAttackTarget();
        }

        protected override void PrepareAction(Action action)
        {
            base.PrepareAction(action);
            if (action.CastAction<Action_FaceTarget>() )
            {
                action.CastAction<Action_FaceTarget>().target = targetEnemy;
            }

            if (action.CastAction<Action_GoToTarget>())
            {
                action.CastAction<Action_GoToTarget>().target = targetEnemy;
            }

            if (action.CastAction<Action_GoToCoast>())
            {
                action.CastAction<Action_GoToCoast>().target = OwnerPlayer._cPlayerEnvironmentInteraction.GetNearestCoast().gameObject;
            }

            if (action.CastAction<Action_FaceCoast>())
            {
                Coast coast = OwnerPlayer._cPlayerEnvironmentInteraction.GetNearestCoast();
                action.CastAction<Action_FaceCoast>().target = coast.FaceSea;
            }

            if (action.CastAction<Action_HeadTarget>())
            {
                action.CastAction<Action_HeadTarget>().target = targetEnemy;
            }

            if (action.CastAction<Action_HeadTarget_Coast>())
            {
                action.CastAction<Action_HeadTarget_Coast>().target = OwnerPlayer._cPlayerEnvironmentInteraction.GetNearestCoast().gameObject;
            }
        }

        protected override void PrepareGoal(Goal goal)
        {
            base.PrepareGoal(goal);

            if (goal.CastGoal<Goal_KillEnemy>())
            {
                goal.CastGoal<Goal_KillEnemy>().TargetEnemy = targetEnemy;
            }
        }

        protected override void InitGoal(Goal goal)
        {
            base.InitGoal(goal);
            if (goal.CastGoal<Goal_Fisherman>())
            {
                goal.CastGoal<Goal_Fisherman>().ownerPlayer = OwnerPlayer;
            }
        }


    }

}

