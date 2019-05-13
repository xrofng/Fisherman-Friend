using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [Serializable]
    public class Action: ScriptableObject
    {
        protected AgentPlanner _planner;
        public AgentPlanner Planner
        {
            get { return _planner; }
            set { _planner = value; }
        }
        [SerializeField]
        public List<WorldState> satisfiesWorldState = new List<WorldState>();
        [SerializeField]
        public List<WorldState> requiresWorldState = new List<WorldState>();

        public virtual bool IsValid()
        {
            int requirementInPlanner = 0;
            foreach (WorldState require in requiresWorldState)
            {
                if (Planner.currentWorldStates.Contains(require))
                {
                    requirementInPlanner += 1;
                }
               
            }
            return requirementInPlanner == requiresWorldState.Count;
        }

        public virtual void OnActionInit()
        {

        }

        public virtual void OnActionStart()
        {
            
        }

        public virtual void OnActionCancel()
        {
            if (ActionCancelationCondition())
            {
                Planner.RemoveCurrentWorldState(this);
            }
        }

        public virtual void OnActionTick()
        {

        }

        public virtual void OnActionDone()
        {
            Planner.ProcessingAction = null;
            foreach(WorldState satisfyState in satisfiesWorldState)
            {
                Planner.AddCurrentWorldState(satisfyState);
            }
        }

        protected virtual bool ActionCancelationCondition()
        {
            return false;
        }

        public T CastAction<T>() where T : Action
        {
            return this as T;
        }
    }

}

