﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class AgentPlanner : MonoBehaviour
    {
        public bool planAtStart;
        /// <summary>
        /// a flag prevent the plan process
        /// </summary>
        protected bool startPlanning;

        public Agent agent;
        public T GetAgent<T>() where T : Agent
        {
            return agent as T;
        }

        [SerializeField]
        public List<Goal> Goals = new List<Goal>();
        public Dictionary<Type,Goal> GoalsDict = new Dictionary<Type, Goal>();
        protected Goal desireGoal;

        [SerializeField]
        public List<Action> Actions = new List<Action>();
        public Dictionary<Type, Action> ActionsDict = new Dictionary<Type, Action>();
        /// <summary>
        /// Value is action that needed to done before key
        /// Key is action really want to perform
        /// </summary>
        protected Dictionary<Action, Action> ActionPlan = new Dictionary<Action, Action>();
        protected Action goalAction;
        protected Action processingAction;
        public Action ProcessingAction
        {
            get { return processingAction; }
            set { processingAction = value; }
        }

        private FFList<WorldState> allRequiredWorldState = new FFList<WorldState>();
        private FFList<WorldState> satisfiableWorldState = new FFList<WorldState>();

        [SerializeField]
        public List<WorldState> currentWorldStates = new List<WorldState>();

        protected bool isPlanReady;

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            if (!startPlanning)
            {
                return;
            }

            if (Time.frameCount % 10 == 0)
            {
                PlanPreparation();
            }

            ProcessPlan();
            PerformActionAsPlan();
            CheckActionCancel();

        }

        protected void PlanPreparation()
        {
            PreparePlanner();

            foreach (Goal goal in Goals)
            {
                PrepareGoal(goal);
            }

            foreach (Action action in Actions)
            {
                PrepareAction(action);
            }
        }

        protected virtual void StartPlan()
        {
            startPlanning = true;
            PlanPreparation();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void PreparePlanner()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="goal"></param>
        protected virtual void PrepareAction(Action action) { }
        protected virtual void PrepareGoal(Goal goal) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="goal"></param>
        protected virtual void InitAction(Action action) { }
        protected virtual void InitGoal(Goal goal) { }

        protected virtual void Initialize()
        {
            foreach (Goal goal in Goals)
            {
                goal.Planner = this;
                GoalsDict.Add(goal.GetType(), goal);
                InitGoal(goal);
            }
            foreach (Action action in Actions)
            {
                action.Planner = this;
                ActionsDict.Add(action.GetType(), action);
                InitAction(action);
                action.OnActionInit();
            }

            startPlanning = planAtStart;

        }

        /// <summary>
        /// Perform current action
        /// </summary>
        private void PerformActionAsPlan()
        {
            if (processingAction)
            {
                processingAction.OnActionTick();
                Debug.Log("Really Do " + processingAction);
            }
        }

        /// <summary>
        /// Check for valid goal and use astar to find action satisfy
        /// </summary>
        protected void ProcessPlan()
        {
            foreach(Goal goal in Goals)
            {
                ActionPlan.Clear();
                if (goal.IsValid())
                {
                    A_STAR(goal.desiredWorldState);
                }
                if (isPlanReady)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// See whether action conditon is not success, remove it's satisfy world state
        /// </summary>
        private void CheckActionCancel()
        {
            foreach (Action action in Actions)
            {
                if (action.IsValid())
                {
                    action.OnActionCancel();
                }
            }
        }

        /// <summary>
        /// Use Astar to find what action to be done
        /// </summary>
        /// <param name="goalDesire"></param>
        void A_STAR(WorldState goalDesire)
        {
            allRequiredWorldState.List.Clear();
            satisfiableWorldState.List.Clear();

            List<Action> openSet = new List<Action>();
            openSet.AddRange(FindActionSatisfy(goalDesire));

            List<Action> closedSet = new List<Action>();

            Dictionary<Action, float> gScore = new Dictionary<Action, float>();
            foreach (Action action in Actions)
            {
                gScore.Add(action, float.MaxValue);
            }
            foreach(Action satisfyAction in openSet)
            {
                gScore[satisfyAction] = 0 + GetHeuristic(satisfyAction);
            }

            while (openSet.Count > 0)
            {
                Action current = GetLowestFscoreAction(openSet, gScore);
                satisfiableWorldState.AddRange(current.satisfiesWorldState);

                if (GetHeuristic(current) <= 0 && allRequiredWorldState.Count <= satisfiableWorldState.Count)
                {
                    //allRequiredWorldState.PrintItems();
                    //satisfiableWorldState.PrintItems();
                    if (processingAction != current)
                    {
                        processingAction = current;
                        processingAction.OnActionStart();
                    }
                    UpdateActionPlan(current, null);
                    isPlanReady = true;
                    return;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                A_STAR_NeighborCheck(current, closedSet, openSet, gScore);
            }
            isPlanReady = false;
            return;
        }

        private Action GetLowestFscoreAction(List<Action> openSet, Dictionary<Action, float> gScore)
        {
            float lowest = float.MaxValue;
            Action lowestF_Action = openSet[0];
            foreach (Action action in openSet)
            {
                if (gScore[action] + GetHeuristic(action) < lowest)
                {
                    lowestF_Action = action;
                    lowest = gScore[action];
                }
            }
            return lowestF_Action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="closedSet"></param>
        /// <param name="openSet"></param>
        /// <param name="gScore"></param>
        void A_STAR_NeighborCheck(Action current, List<Action> closedSet, List<Action> openSet, Dictionary<Action,float> gScore)
        {
            foreach (WorldState requireState in current.requiresWorldState)
            {
                if (currentWorldStates.Contains(requireState))
                {
                    continue;
                }
                foreach (Action satisfyAction in FindActionSatisfy(requireState))
                {
                    if (!closedSet.Contains(satisfyAction))
                    {
                        float tentative_gScore = gScore[current] + 1;

                        if (!openSet.Contains(satisfyAction))
                        {
                            openSet.Add(satisfyAction);
                        }
                        if (tentative_gScore < gScore[satisfyAction])
                        {
                            UpdateActionPlan( current , satisfyAction);
                            gScore[satisfyAction] = tentative_gScore;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requireState"></param>
        /// <returns></returns>
        private List<Action> FindActionSatisfy(WorldState requireState)
        {
            allRequiredWorldState.Add(requireState);
            
            List<Action> satisfyActions = new List<Action>();
            foreach(Action action in Actions)
            {
                if (action.satisfiesWorldState.Contains(requireState))
                {
                    satisfyActions.Add(action);
                }
            }
            return satisfyActions;
        }

        /// <summary>
        /// Get number of world-state that still need to be satisfied
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private int GetHeuristic(Action action)
        {
            int hScore = 0;
            foreach(WorldState requireWorldState in action.requiresWorldState)
            {
                if (!currentWorldStates.Contains(requireWorldState))
                {
                    hScore += 1;
                }
            }
            return hScore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reachToNode"></param>
        /// <param name="reachByNode"></param>
        private void UpdateActionPlan(Action reachTo, Action reachBy)
        {
            // If it's the first action in plan. assign it to goalAction
            if (ActionPlan.Count == 0)
            {
                goalAction = reachTo;
            }
            // add action to plan(as key) and beforehand required action(as value)
            if (!ActionPlan.ContainsKey(reachTo))
            {
                ActionPlan.Add(reachTo, reachBy);
            }
            else
            {
                ActionPlan[reachTo] = reachBy;
            }
        }

        public void AddCurrentWorldState(WorldState state)
        {
            if (!currentWorldStates.Contains(state))
            {
                currentWorldStates.Add(state);
            }
        }

        public void RemoveCurrentWorldState(WorldState state)
        {
            if (currentWorldStates.Contains(state))
            {
                currentWorldStates.Remove(state);
            }
        }

        public void RemoveCurrentWorldState(Action action)
        {
            foreach (WorldState satisfyState in action.satisfiesWorldState)
            {
                RemoveCurrentWorldState(satisfyState);
            }
        }

        public T GetGoal<T>() where T : Goal
        {
            if (GoalsDict.ContainsKey(typeof(T)))
            {
                return (T)GoalsDict[typeof(T)];
            }
            return null;
        }

        public T GetAction<T>() where T : Action
        {
            if (ActionsDict.ContainsKey(typeof(T)))
            {
                return (T)ActionsDict[typeof(T)];
            }
            return null;
        }
    }



}

