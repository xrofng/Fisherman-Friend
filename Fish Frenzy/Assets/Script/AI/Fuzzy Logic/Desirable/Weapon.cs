using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected FuzzyModule fuzzyModule;
    protected int Ammo;

    protected enum FLV_NAME
    {
        // input
        DISTANCE_TO_TARGET, AMMO,
        // output
        DESIRABILITY
    }

    protected enum FZ_SET_DISTANCE
    {
        CLOSE,MEDIUM,FAR
    }
    protected enum FZ_SET_AMMO
    {
        LOW, OKAY, LOADS
    }
    protected enum FZ_SET_DESIRE
    {
        UNDESIRABLE, DESIRABLE, VERY_DESIRABLE
    }

    public abstract void InitFuzzy();

    public double GetDesirability(double disToTarget)
    {
        fuzzyModule.Fuzzification(FLV_NAME.DISTANCE_TO_TARGET.ToString("F"), disToTarget);
        fuzzyModule.Fuzzification(FLV_NAME.AMMO.ToString("F"), Ammo);

        fuzzyModule.ApplyRule();
        return fuzzyModule.Defuzzification(FLV_NAME.DESIRABILITY.ToString("F"), FuzzyModule.DefuzzifyType.max_avg);
    }
}
