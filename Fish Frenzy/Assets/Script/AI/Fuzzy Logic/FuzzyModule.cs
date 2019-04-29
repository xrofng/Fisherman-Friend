using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyModule {

	public enum DefuzzifyType
	{
		max_avg, centroid
	};

	public int NumSamplesToUseForCentroid = 15;

	private Dictionary<string, FuzzyVariable> FuzzyVariables;
	private List<FuzzyRule> FuzzyRules;

	public FuzzyModule(){
		FuzzyVariables = new Dictionary<string, FuzzyVariable> ();
		FuzzyRules = new List<FuzzyRule> ();
	}

	public FuzzyVariable CreateFLV(string varName){
		FuzzyVariable FLV = new FuzzyVariable ();
		FuzzyVariables.Add (varName, FLV);
		return FLV;
	}

	public void AddRule(FuzzyRule rule)
    {
        this.FuzzyRules.Add(rule);
	}

	public void Fuzzification(string nameFLV, double value)
    {
        this.FuzzyVariables[nameFLV].Fuzzify(value);
	}

    public void ApplyRule()
    {
        foreach(FuzzyRule rule in FuzzyRules)
        {
            rule.CalculateConsequenceWithAND();
        }
    }

	public double Defuzzification(string nameFLV, DefuzzifyType method){
        double desirableValue = 0.0;
        switch (method)
        {
            case DefuzzifyType.centroid:
                desirableValue = FuzzyVariables[nameFLV].DeFuzzifyCentroid(NumSamplesToUseForCentroid);
                break;
            case DefuzzifyType.max_avg:
                desirableValue = FuzzyVariables[nameFLV].DeFuzzifyMaxAvg();
                break;
            default: break;
        }
        return desirableValue;
	}
}
