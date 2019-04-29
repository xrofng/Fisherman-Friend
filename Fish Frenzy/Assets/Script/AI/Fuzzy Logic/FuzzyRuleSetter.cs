using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FuzzyRuleSetter
{
    [SerializeField]
    public List<FuzzyRuleCondition> conditions = new List<FuzzyRuleCondition>();

    public enum desirabilityType
    {
        notSet,
        unDesire,
        desire,
        veryDesire
    }
    [SerializeField]
    public desirabilityType desirability;

    public FuzzyRule GetRule(Dictionary<FuzzyVariableIdentifier, FuzzyVariable> fzVarList, FuzzyVariable desireVar)
    {
        FuzzySet desire = desireVar.GetMember(new FuzzySetIdentifier("unDesire"));
        switch (desirability)
        {
            case desirabilityType.desire: desire = desireVar.GetMember(new FuzzySetIdentifier("desire"));  break;
            case desirabilityType.veryDesire: desire = desireVar.GetMember(new FuzzySetIdentifier("veryDesire")); break;
        }

        List<FuzzySet> fuzzyConditons = new List<FuzzySet>();
        foreach(FuzzyRuleCondition condition in conditions)
        {
            fuzzyConditons.Add(fzVarList[condition.varId].GetMember(condition.setId));
        }
        return new FuzzyRule(fuzzyConditons, desire);
    }
}

[Serializable]
public class FuzzyRuleCondition
{
    [SerializeField]
    public FuzzyVariableIdentifier varId;
    [SerializeField]
    public FuzzySetIdentifier setId;
}