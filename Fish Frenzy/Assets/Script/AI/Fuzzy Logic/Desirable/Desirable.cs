using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desirable : MonoBehaviour
{
    protected FuzzyModule fuzzyModule;

    /// <summary>
    /// Fuzzy Variable in this desirable
    /// </summary>
    [SerializeField]
    protected List<FuzzyVariable> fuzzyVariables = new List<FuzzyVariable>();

    /// <summary>
    /// Cached Fuzzy Sets
    /// Key is name of Variable
    /// </summary>
    protected Dictionary<FuzzyVariableIdentifier, FuzzyVariable> fuzzyVariableSets = new Dictionary<FuzzyVariableIdentifier, FuzzyVariable>();

    [SerializeField]
    protected FuzzyVariable desireVariable;

    [SerializeField]
    protected List<FuzzyRuleSetter> fuzzyRuleSetters;

    public virtual void InitFuzzy()
    {
        fuzzyModule = new FuzzyModule();

        foreach (FuzzyVariable fuzzyVariable in fuzzyVariables)
        {
            // cache
            fuzzyVariableSets.Add(fuzzyVariable.variableId, fuzzyVariable);

            // create all flv
            fuzzyModule.CreateFLV(fuzzyVariable.variableId.name);

            // fill fuzzy set
            fuzzyVariable.ConstructSets();
        }

        fuzzyModule.CreateFLV(desireVariable.name);
        desireVariable.ConstructSets();

        // rule
        foreach(FuzzyRuleSetter setter in fuzzyRuleSetters)
        {
            fuzzyModule.AddRule(setter.GetRule(fuzzyVariableSets, desireVariable));
        }
    }

    public virtual double GetDesirability()
    {
        if (!IsValid())
        {
            return -double.MaxValue;
        }

        EvalDesirability();

        fuzzyModule.ApplyRule();
        return fuzzyModule.Defuzzification(desireVariable.name, FuzzyModule.DefuzzifyType.max_avg);
    }

    public virtual bool IsValid()
    {
        return true;
    }

    public virtual void EvalDesirability()
    {
        //foreach (string fzVarName in fuzzyVariableSets.Keys)
        //{
        //    fuzzyModule.Fuzzification(fuzzyVariableSets[fzVarName],);

        //    //fuzzyModule.Fuzzification(FLV_NAME.DISTANCE_TO_TARGET.ToString("F"), disToTarget);
        //    //fuzzyModule.Fuzzification(FLV_NAME.AMMO.ToString("F"), Ammo);
        //}
    }

    protected void Fuzzificate(FuzzyVariableIdentifier variableId, double value)
    {
        if (fuzzyVariableSets.ContainsKey(variableId))
        {
            fuzzyModule.Fuzzification(variableId.name,value);
        }
    }
}
