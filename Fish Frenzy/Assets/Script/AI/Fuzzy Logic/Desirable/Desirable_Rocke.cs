using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desirable_Rocke : Desirable
{
    public int ammo = 8;
    public float dis = 200;

    protected enum FLV_NAME
    {
        // input
        DISTANCE_TO_TARGET, AMMO,
        // output
        DESIRABILITY
    }

    protected enum FZ_SET_DISTANCE
    {
        CLOSE, MEDIUM, FAR
    }
    protected enum FZ_SET_AMMO
    {
        LOW, OKAY, LOADS
    }

    protected override bool IsValid()
    {
        return base.IsValid();
    }

    protected override void Fuzzificate()
    {
        base.Fuzzificate();
        fuzzyModule.Fuzzification(FLV_NAME.DISTANCE_TO_TARGET.ToString("F"), dis);
        fuzzyModule.Fuzzification(FLV_NAME.AMMO.ToString("F"), ammo);
    }

    public override void InitFuzzy()
    {
        base.InitFuzzy();

        // create all flv
        FuzzyVariable FLV_Distance = fuzzyModule.CreateFLV(FLV_NAME.DISTANCE_TO_TARGET.ToString("F"));
        FuzzyVariable FLV_Ammo = fuzzyModule.CreateFLV(FLV_NAME.AMMO.ToString("F"));
        FuzzyVariable FLV_Desire = fuzzyModule.CreateFLV(FLV_NAME.DESIRABILITY.ToString("F"));

        // fill fuzzy set
        FuzzySet distance_close = FLV_Distance.AddLeftShoulderSet(
                                   FZ_SET_DISTANCE.CLOSE.ToString("F"),
                                   0, 25, 150);
        FuzzySet distance_med = FLV_Distance.AddTriangularSet(
                                   FZ_SET_DISTANCE.MEDIUM.ToString("F"),
                                   25, 150, 300);
        FuzzySet distance_far = FLV_Distance.AddRightShoulderSet(
                                   FZ_SET_DISTANCE.FAR.ToString("F"),
                                   150, 300, 500);

        FuzzySet ammo_low = FLV_Ammo.AddLeftShoulderSet(
                                   FZ_SET_AMMO.LOW.ToString("F"),
                                   0, 0, 10);
        FuzzySet ammo_ok = FLV_Ammo.AddTriangularSet(
                                   FZ_SET_AMMO.OKAY.ToString("F"),
                                   0, 10, 30);
        FuzzySet ammo_loads = FLV_Ammo.AddRightShoulderSet(
                                   FZ_SET_AMMO.LOADS.ToString("F"),
                                   10, 30, 40);

        FuzzySet unDesire = FLV_Desire.AddLeftShoulderSet(
                                    FZ_SET_DESIRE.UNDESIRABLE.ToString("F"),
                                    0, 25, 50);
        FuzzySet desire = FLV_Desire.AddTriangularSet(
                                   FZ_SET_DESIRE.DESIRABLE.ToString("F"),
                                   25, 50, 75);
        FuzzySet veryDesire = FLV_Desire.AddRightShoulderSet(
                                   FZ_SET_DESIRE.VERYDESIRABLE.ToString("F"),
                                   50, 75, 100);

        // rule
        AddRule(new FuzzySet[] { distance_far, ammo_low }, desire);
        AddRule(new FuzzySet[] { distance_far, ammo_ok }, unDesire);
        AddRule(new FuzzySet[] { distance_far, ammo_loads }, unDesire);

        AddRule(new FuzzySet[] { distance_med, ammo_low }, veryDesire);
        AddRule(new FuzzySet[] { distance_med, ammo_ok }, veryDesire);
        AddRule(new FuzzySet[] { distance_med, ammo_loads }, desire);

        AddRule(new FuzzySet[] { distance_close, ammo_low }, desire);
        AddRule(new FuzzySet[] { distance_close, ammo_ok }, unDesire);
        AddRule(new FuzzySet[] { distance_close, ammo_loads }, unDesire);

    }

}
