﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class AgentPlanner : MonoBehaviour
    {
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

        [SerializeField]
        public List<WorldState> currentWorldStates = new List<WorldState>();

        protected bool isPlanReady;

        //protected SM_Agent _smAgent;
        //public SM_Agent SMAgent
        //{
        //    get
        //    {
        //        if(_smAgent == null)
        //        {
        //            _smAgent = GetComponent<SM_Agent>();
        //        }
        //        return _smAgent;
        //    }
        //}

        protected AgentAnimator _agentAnimator;
        public AgentAnimator AgentAnimator
        {
            get
            {
                if (_agentAnimator == null)
                {
                    _agentAnimator = GetComponent<AgentAnimator>();
                }
                return _agentAnimator;
            }
        }

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            StartPlan();
            PerformActionAsPlan();
            CheckActionCancel();
        }

        protected virtual void Initialize()
        {
            Debug.Log("base Initialize");
            foreach (Goal goal in Goals)
            {
                goal.Planner = this;
                GoalsDict.Add(goal.GetType(), goal);
            }
            foreach (Action action in Actions)
            {
                action.Planner = this;
                ActionsDict.Add(action.GetType(), action);
            }
        }


        private void PerformActionAsPlan()
        {
            //Action prevAction = null;
            //Action currentAction = goalAction;
            //while(currentAction != null)
            //{
            //    Debug.Log("Try Do " + currentAction);
            //    prevAction = currentAction;
            //    currentAction = ActionPlan[currentAction];
            //}
            //currentAction = prevAction;
            //currentAction.OnActionTick();
            processingAction.OnActionTick();
            Debug.Log("Really Do " + processingAction);
        }

        protected void StartPlan()
        {
            foreach(Goal goal in Goals)
            {
                ActionPlan.Clear();
                Debug.Log("CheckValid: "+goal.name+" "+goal.IsValid());
                if (goal.IsValid())
                {
                    Debug.Log("AStar on " + goal.name + " witd desired " + goal.desiredWorldState);
                    A_STAR(goal.desiredWorldState);
                }
                if (isPlanReady)
                {

                    return;
                }
            }
        }

        private void CheckActionCancel()
        {
            foreach (Action action in Actions)
            {
                action.OnActionCancel();
            }
        }

        void A_STAR(WorldState goalDesire)
        {
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
                //Debug.Log(current.name + " gscore:" + gScore[current]);
                if (GetHeuristic(current) <= 0)
                {
                    Debug.Log("Plan Ready last action is: " + current.name);
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
                foreach (Action satisfyAction in FindActionSatisfy( requireState))
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

