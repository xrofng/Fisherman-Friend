using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desirable_TargetPlayer : Desirable
{
    public Player targetPlayer;
    public FuzzyVariableIdentifier distanceVar;

    public override bool IsValid()
    {
        return targetPlayer;
    }

    public override void EvalDesirability()
    {
        float distanceToTarget = Vector3.Distance(this.transform.position, targetPlayer.transform.position);
        Fuzzificate(distanceVar, distanceToTarget);

    }
}
