using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desirable_TargetPlayer_Fisherman : Desirable
{
    /// <summary>
    /// Encapsulation of this Agent
    /// </summary>
    public Agent_Fisherman agent_Fisherman;

    /// <summary>
    /// 
    /// </summary>
    public Player targetPlayer;

    protected enum FLV_NAME
    {
        // input
        DISTANCE ,BRAIN, DAMAGE,
        // output
        DESIRABILITY
    }

    protected enum FZ_SET_BRAIN
    {
        DUMB, AVERAGE, CLEVER
    }

    protected enum FZ_SET_DISTANCE
    {
        CLOSE, MID, FAR
    }

    protected enum FZ_SET_DAMAGE
    {
        LOW, MID, HIGH
    }

    protected override bool IsValid()
    {
        return targetPlayer && agent_Fisherman;
    }

    public override void InitFuzzy()
    {
        base.InitFuzzy();

        // create all flv
       // FuzzyVariable FLV_BRAIN = fuzzyModule.CreateFLV(FLV_NAME.BRAIN.ToString("F"));
        FuzzyVariable FLV_DISTANCE = fuzzyModule.CreateFLV(FLV_NAME.DISTANCE.ToString("F"));
        FuzzyVariable FLV_DAMAGE = fuzzyModule.CreateFLV(FLV_NAME.DAMAGE.ToString("F"));
        FuzzyVariable FLV_DESIRE = fuzzyModule.CreateFLV(FLV_NAME.DESIRABILITY.ToString("F"));

        // fill fuzzy set

        /*
        ///-------- SET_BRAIN
        FuzzySet SET_BRAIN_DUMB = FLV_BRAIN.AddLeftShoulderSet(
                                  FZ_SET_BRAIN.DUMB.ToString("F"),
                                  0, 0, 150);
        FuzzySet SET_BRAIN_AVERAGE = FLV_BRAIN.AddTriangularSet(
                                   FZ_SET_BRAIN.AVERAGE.ToString("F"),
                                   0, 150, 300);
        FuzzySet SET_BRAIN_CLEVER = FLV_BRAIN.AddRightShoulderSet(
                                   FZ_SET_BRAIN.CLEVER.ToString("F"),
                                   150, 300, 300);
        */

        ///-------- SET_DISTANCE
        FuzzySet SET_DISTANCE_CLOSE = FLV_DISTANCE.AddLeftShoulderSet(
                                  FZ_SET_DISTANCE.CLOSE.ToString("F"),
                                  0, 0, 5);
        FuzzySet SET_DISTANCE_MID = FLV_DISTANCE.AddTriangularSet(
                                   FZ_SET_DISTANCE.MID.ToString("F"),
                                   0, 5, 30);
        FuzzySet SET_DISTANCE_FAR = FLV_DISTANCE.AddRightShoulderSet(
                                   FZ_SET_DISTANCE.FAR.ToString("F"),
                                   5, 30, 30);

        ///-------- SET_DAMAGE
        FuzzySet SET_DAMAGE_LOW = FLV_DAMAGE.AddLeftShoulderSet(
                                   FZ_SET_DAMAGE.LOW.ToString("F"),
                                   0, 0, 100);
        FuzzySet SET_DAMAGE_MID = FLV_DAMAGE.AddTriangularSet(
                                   FZ_SET_DAMAGE.MID.ToString("F"),
                                   0, 100, 999);
        FuzzySet SET_DAMAGE_HIGH = FLV_DAMAGE.AddRightShoulderSet(
                                   FZ_SET_DAMAGE.HIGH.ToString("F"),
                                   100, 999, 999);

        ///-------- SET_DESIRE
        FuzzySet SET_DESIRE_UNDESIRE = FLV_DESIRE.AddLeftShoulderSet(
                                    FZ_SET_DESIRE.UNDESIRABLE.ToString("F"),
                                    0, 25, 50);
        FuzzySet SET_DESIRE_DESIRE = FLV_DESIRE.AddTriangularSet(
                                   FZ_SET_DESIRE.DESIRABLE.ToString("F"),
                                   25, 50, 75);
        FuzzySet SET_DESIRE_VERYDESIRABLE = FLV_DESIRE.AddRightShoulderSet(
                                   FZ_SET_DESIRE.VERYDESIRABLE.ToString("F"),
                                   50, 75, 100);

        
        AddRule(new FuzzySet[] { SET_DISTANCE_CLOSE, SET_DAMAGE_LOW }, SET_DESIRE_DESIRE);
        AddRule(new FuzzySet[] { SET_DISTANCE_CLOSE, SET_DAMAGE_MID }, SET_DESIRE_VERYDESIRABLE);
        AddRule(new FuzzySet[] { SET_DISTANCE_CLOSE, SET_DAMAGE_HIGH }, SET_DESIRE_VERYDESIRABLE);

        AddRule(new FuzzySet[] { SET_DISTANCE_MID,   SET_DAMAGE_LOW }, SET_DESIRE_DESIRE);
        AddRule(new FuzzySet[] { SET_DISTANCE_MID,   SET_DAMAGE_MID }, SET_DESIRE_DESIRE);
        AddRule(new FuzzySet[] { SET_DISTANCE_MID,   SET_DAMAGE_HIGH }, SET_DESIRE_VERYDESIRABLE);
                                                        
        AddRule(new FuzzySet[] { SET_DISTANCE_FAR,   SET_DAMAGE_LOW }, SET_DESIRE_UNDESIRE);
        AddRule(new FuzzySet[] { SET_DISTANCE_FAR,   SET_DAMAGE_MID }, SET_DESIRE_UNDESIRE);
        AddRule(new FuzzySet[] { SET_DISTANCE_FAR,   SET_DAMAGE_HIGH }, SET_DESIRE_DESIRE);

    }

    protected override void Fuzzificate()
    {
        base.Fuzzificate();

        fuzzyModule.Fuzzification(FLV_NAME.DAMAGE.ToString("F"), targetPlayer.damagePercent);

        float distanceToTarget = sClass.DistanceIgnored(VectorComponent.y, transform.position, targetPlayer.transform.position);
        fuzzyModule.Fuzzification(FLV_NAME.DISTANCE.ToString("F"),distanceToTarget);
    }

}
