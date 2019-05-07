using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class AgentPlanner_Shark : AgentPlanner
{
    public GameObject targetPlayer;

    public GameObject NearestPatrol;

    public SI_Shark shark;

    protected override void Initialize()
    {
        base.Initialize();
       // GetGoal<Goal_KillEnemy>().TargetEnemy = targetEnemy;
       // GetAction<Action_GoToTarget>().SetProperties(targetEnemy, meleeRangeRadius, moveSpeed);

    }

}
