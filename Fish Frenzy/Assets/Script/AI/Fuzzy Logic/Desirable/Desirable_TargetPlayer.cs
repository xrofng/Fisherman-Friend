using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desirable_TargetPlayer : Desirable
{
    public Player targetPlayer;
    private BotPlayer botPlayer;
    public BotPlayer BotPlayer
    {
        get
        {
            if(botPlayer == null)
            {
                botPlayer = targetPlayer.gameObject.GetComponent<BotPlayer>();
            }
            return botPlayer;
        }
    }

    protected enum FLV_NAME
    {
        // input
        BRAIN, HOLDFISH, DAMAGE,
        // output
        DESIRABILITY
    }

    protected enum FZ_SET_BRAIN
    {
        DUMB, AVERAGE, CLEVER
    }

    protected enum FZ_SET_HOLDFISH
    {
        NO, YES
    }

    protected enum FZ_SET_DAMAGE
    {
        LOW, MID, HIGH
    }

    protected override bool IsValid()
    {
        return targetPlayer && BotPlayer;
    }

    public override void InitFuzzy()
    {
        base.InitFuzzy();

        // create all flv
        FuzzyVariable FLV_BRAIN = fuzzyModule.CreateFLV(FLV_NAME.BRAIN.ToString("F"));
        FuzzyVariable FLV_HOLDFISH = fuzzyModule.CreateFLV(FLV_NAME.HOLDFISH.ToString("F"));
        FuzzyVariable FLV_DAMAGE = fuzzyModule.CreateFLV(FLV_NAME.DAMAGE.ToString("F"));
        FuzzyVariable FLV_DESIRE = fuzzyModule.CreateFLV(FLV_NAME.DESIRABILITY.ToString("F"));

        // fill fuzzy set

        ///-------- SET_BRAIN
        FuzzySet SET_BRAIN_DUMB = FLV_BRAIN.AddLeftShoulderSet(
                                   FZ_SET_BRAIN.DUMB.ToString("F"),
                                   0, 80, 100);
        FuzzySet SET_BRAIN_AVERAGE = FLV_BRAIN.AddTriangularSet(
                                   FZ_SET_BRAIN.AVERAGE.ToString("F"),
                                   80, 100, 120);
        FuzzySet SET_BRAIN_CLEVER = FLV_BRAIN.AddRightShoulderSet(
                                   FZ_SET_BRAIN.CLEVER.ToString("F"),
                                   100, 120, 200);

        ///-------- SET_HOLDFISH
        FuzzySet SET_HOLDFISH_NO = FLV_HOLDFISH.AddLeftShoulderSet(
                                    FZ_SET_HOLDFISH.NO.ToString("F"),
                                    0, 0.25, 0.50);
        FuzzySet SET_HOLDFISH_YES = FLV_HOLDFISH.AddRightShoulderSet(
                                   FZ_SET_HOLDFISH.YES.ToString("F"),
                                   0.5, 0.75, 1);
            
        ///-------- SET_DAMAGE
        FuzzySet SET_DAMAGE_LOW = FLV_DAMAGE.AddLeftShoulderSet(
                                    FZ_SET_DAMAGE.LOW.ToString("F"),
                                    0, 25, 50);
        FuzzySet SET_DAMAGE_MID = FLV_DAMAGE.AddTriangularSet(
                                   FZ_SET_DAMAGE.MID.ToString("F"),
                                   25, 50, 75);
        FuzzySet SET_DAMAGE_HIGH = FLV_DAMAGE.AddRightShoulderSet(
                                   FZ_SET_DAMAGE.HIGH.ToString("F"),
                                   50, 75, 100);

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

        AddRule(new FuzzySet[] { SET_BRAIN_DUMB, SET_HOLDFISH_NO, SET_DAMAGE_LOW }, SET_DESIRE_VERYDESIRABLE);
        AddRule(new FuzzySet[] { SET_BRAIN_DUMB, SET_HOLDFISH_NO, SET_DAMAGE_MID }, SET_DESIRE_VERYDESIRABLE);
        AddRule(new FuzzySet[] { SET_BRAIN_DUMB, SET_HOLDFISH_NO, SET_DAMAGE_HIGH }, SET_DESIRE_VERYDESIRABLE);

        AddRule(new FuzzySet[] { SET_BRAIN_DUMB, SET_HOLDFISH_YES, SET_DAMAGE_LOW }, SET_DESIRE_UNDESIRE);
        AddRule(new FuzzySet[] { SET_BRAIN_DUMB, SET_HOLDFISH_YES, SET_DAMAGE_MID }, SET_DESIRE_UNDESIRE);
        AddRule(new FuzzySet[] { SET_BRAIN_DUMB, SET_HOLDFISH_YES, SET_DAMAGE_HIGH }, SET_DESIRE_UNDESIRE);

        AddRule(new FuzzySet[] { SET_BRAIN_AVERAGE, SET_HOLDFISH_NO, SET_DAMAGE_LOW }, SET_DESIRE_UNDESIRE);
        AddRule(new FuzzySet[] { SET_BRAIN_AVERAGE, SET_HOLDFISH_NO, SET_DAMAGE_MID }, SET_DESIRE_UNDESIRE);
        AddRule(new FuzzySet[] { SET_BRAIN_AVERAGE, SET_HOLDFISH_NO, SET_DAMAGE_HIGH }, SET_DESIRE_UNDESIRE);

        AddRule(new FuzzySet[] { SET_BRAIN_AVERAGE, SET_HOLDFISH_YES, SET_DAMAGE_LOW }, SET_DESIRE_DESIRE);
        AddRule(new FuzzySet[] { SET_BRAIN_AVERAGE, SET_HOLDFISH_YES, SET_DAMAGE_MID }, SET_DESIRE_DESIRE);
        AddRule(new FuzzySet[] { SET_BRAIN_AVERAGE, SET_HOLDFISH_YES, SET_DAMAGE_HIGH }, SET_DESIRE_VERYDESIRABLE);

        AddRule(new FuzzySet[] { SET_BRAIN_CLEVER, SET_HOLDFISH_NO, SET_DAMAGE_LOW }, SET_DESIRE_UNDESIRE);
        AddRule(new FuzzySet[] { SET_BRAIN_CLEVER, SET_HOLDFISH_NO, SET_DAMAGE_MID }, SET_DESIRE_UNDESIRE);
        AddRule(new FuzzySet[] { SET_BRAIN_CLEVER, SET_HOLDFISH_NO, SET_DAMAGE_HIGH }, SET_DESIRE_UNDESIRE);

        AddRule(new FuzzySet[] { SET_BRAIN_CLEVER, SET_HOLDFISH_YES, SET_DAMAGE_LOW }, SET_DESIRE_DESIRE);
        AddRule(new FuzzySet[] { SET_BRAIN_CLEVER, SET_HOLDFISH_YES, SET_DAMAGE_MID }, SET_DESIRE_VERYDESIRABLE);
        AddRule(new FuzzySet[] { SET_BRAIN_CLEVER, SET_HOLDFISH_YES, SET_DAMAGE_HIGH }, SET_DESIRE_VERYDESIRABLE);

    }

    protected override void Fuzzificate()
    {
        base.Fuzzificate();

        fuzzyModule.Fuzzification(FLV_NAME.BRAIN.ToString("F"), BotPlayer.GetIQ());

        int holdFish = targetPlayer.holdingFish? 1:0;
        fuzzyModule.Fuzzification(FLV_NAME.HOLDFISH.ToString("F"), holdFish);

        fuzzyModule.Fuzzification(FLV_NAME.DAMAGE.ToString("F"), targetPlayer.dPercent);

    }
}
