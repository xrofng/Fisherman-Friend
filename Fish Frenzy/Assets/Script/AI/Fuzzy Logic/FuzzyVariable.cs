using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FuzzyVariable", menuName = "FuzzyVariable", order = 52)]
public class FuzzyVariable : ScriptableObject
{
    /// <summary>
    /// name of fuzzy variable
    /// </summary>
    [SerializeField]
    public FuzzyVariableIdentifier variableId;

    /// <summary>
    /// Fuzzy Sets in this variable
    /// </summary>
    [SerializeField]
    public List<FuzzyGraphData> Members = new List<FuzzyGraphData>();

    /// <summary>
    /// Cached Fuzzy Sets
    /// Key is name of Set
    /// </summary>
	private Dictionary<FuzzySetIdentifier, FuzzySet> MemberSets;

	private double minRange, maxRange;

	public FuzzyVariable(){
		minRange = 0.0;
		maxRange = 0.0;
		MemberSets = new Dictionary<FuzzySetIdentifier, FuzzySet> ();

        Members.Add(new FuzzyGraphData(FuzzyGraphData.FuzzySetType.LeftShoulder));
        Members.Add(new FuzzyGraphData(FuzzyGraphData.FuzzySetType.Triangle));
        Members.Add(new FuzzyGraphData(FuzzyGraphData.FuzzySetType.RightShoulder));
    }

    public void ConstructSets()
    {
        foreach(FuzzyGraphData graphData in Members)
        {
            if(graphData.setType == FuzzyGraphData.FuzzySetType.LeftShoulder)
            {
                AddLeftShoulderSet(graphData.setIdentifier, graphData.leftValue, graphData.midValue, graphData.rightValue);
            }
            if (graphData.setType == FuzzyGraphData.FuzzySetType.Triangle)
            {
                AddTriangularSet(graphData.setIdentifier, graphData.leftValue, graphData.midValue, graphData.rightValue);
            }
            if (graphData.setType == FuzzyGraphData.FuzzySetType.RightShoulder)
            {
                AddRightShoulderSet(graphData.setIdentifier, graphData.leftValue, graphData.midValue, graphData.rightValue);
            }
        }
    }

    public FuzzySet AddLeftShoulderSet(FuzzySetIdentifier name, double minBound, 
										double peak, double maxBound){
		FuzzySet_LeftShoulder leftSet = new FuzzySet_LeftShoulder ( peak, 
																	peak - minBound, 
																	maxBound - peak);
		MemberSets.Add (name, leftSet);
		return leftSet;
	}

    public FuzzySet AddRightShoulderSet(FuzzySetIdentifier name, double minBound, 
											double peak, double maxBound){
		FuzzySet_RightShoulder rightSet = new FuzzySet_RightShoulder ( peak, 
			peak - minBound, 
			maxBound - peak);
		MemberSets.Add (name, rightSet);
		return rightSet;
	}

	public FuzzySet AddTriangularSet( FuzzySetIdentifier name, double minBound, 
									double peak, double maxBound){
		FuzzySet_Triangle triangleSet = new FuzzySet_Triangle ( peak, 
			peak - minBound, 
			maxBound - peak);
		MemberSets.Add (name, triangleSet);
		return triangleSet;
	}

	public void Fuzzify( double val ){
		foreach (FuzzySet fuzzySet in MemberSets.Values) {
			fuzzySet.DOM = fuzzySet.CalculateDOM (val);
		}
	}
	public double DeFuzzifyMaxAvg()
    {
        double sum_repTimeConfidence = 0.0f;
        double sum_confidence = 0.0f;
        foreach(FuzzySet fz in MemberSets.Values)
        {
            sum_repTimeConfidence += fz.DOM * fz.RepresentativeValue;
            sum_confidence += fz.DOM;
        }
        return sum_repTimeConfidence / sum_confidence;
	}

	public double DeFuzzifyCentroid( int NumSamples )
    {
		return 0.0;
	}

    public FuzzySet GetMember(FuzzySetIdentifier memberSetId)
    {
        if (MemberSets.ContainsKey(memberSetId))
        {
            return MemberSets[memberSetId];
        }
        return null;
    }

}
