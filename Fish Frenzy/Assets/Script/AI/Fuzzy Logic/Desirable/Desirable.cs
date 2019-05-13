using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desirable : MonoBehaviour
{
    protected FuzzyModule fuzzyModule;

    protected enum FLV_NAME
    {
        // input
        VAR1, VAR2,
        // output
        DESIRABILITY
    }

    protected enum FZ_SET_VAR1
    {
        LEFT, TRI, RIGHT
    }

    protected enum FZ_SET_VAR2
    {
        LEFT, TRI, RIGHT
    }

    protected enum FZ_SET_DESIRE
    {
        UNDESIRABLE, DESIRABLE, VERYDESIRABLE
    }

    public virtual void InitFuzzy()
    {
        fuzzyModule = new FuzzyModule();
    }

    protected virtual void Fuzzificate()
    {
        //fuzzyModule.Fuzzification(FLV_NAME.VAR1.ToString("F"), 0);
        //fuzzyModule.Fuzzification(FLV_NAME.VAR2.ToString("F"), 0);
    }

    public virtual double GetDesirability()
    {
        if (!IsValid())
        {
            return -double.MaxValue;
        }

        Fuzzificate();

        fuzzyModule.ApplyRule();
        return fuzzyModule.Defuzzification(FLV_NAME.DESIRABILITY.ToString("F"), FuzzyModule.DefuzzifyType.max_avg);

    }

    protected void AddRule(FuzzySet[] conditions,FuzzySet desire)
    {
        //fuzzyModule.AddRule(new FuzzyRule(conditions, desire));
        fuzzyModule.AddRule(new FuzzyRule(conditions[0], conditions[1], desire));
    }

    protected virtual bool IsValid()
    {
        return true;
    }

    
}
