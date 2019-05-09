using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Goal: ScriptableObject
    {
        protected AgentPlanner _planner;
        public AgentPlanner Planner
        {
            get { return _planner; }
            set { _planner = value; }
        }

        [SerializeField]
        public WorldState desiredWorldState;

        public int priority { get; set; }

        public virtual bool IsValid()
        {
            return Planner;
        }

        public T CastGoal<T>() where T : Goal
        {
            return this as T;
        }
    }
}


