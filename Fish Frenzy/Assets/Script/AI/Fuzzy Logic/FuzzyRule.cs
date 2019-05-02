using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyRule {

    private List<FuzzySet> conditions;
    private FuzzySet consequence;

    public FuzzyRule(FuzzySet condition1, FuzzySet condition2, FuzzySet consequence)
    {
        conditions = new List<FuzzySet>();
        AddCondition(condition1);
        AddCondition(condition2);
        SetConsequence(consequence);
    }

    public FuzzyRule(FuzzySet[] addConditions, FuzzySet consequence)
    {
        conditions = new List<FuzzySet>();

        foreach (FuzzySet condition in addConditions)
        {
            AddCondition(condition);
        }
        if (conditions.Count > 1)
        {
        }
        SetConsequence(consequence);
    }


    private void AddCondition(FuzzySet fz)
    {
        conditions.Add(fz);
    }

    private void SetConsequence(FuzzySet fz)
    {
        this.consequence = fz;
    }

    public void CalculateConsequenceWithAND()
    {
        double confidence = double.MaxValue;
        foreach(FuzzySet condition in conditions)
        {
            if(confidence > condition.DOM)
            {
                confidence = condition.DOM;
            }
        }
        consequence.SetConfidenceWithOR(confidence);
    }

}
