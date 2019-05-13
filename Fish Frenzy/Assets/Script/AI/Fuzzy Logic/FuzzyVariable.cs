using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyVariable : MonoBehaviour {

	private Dictionary<string, FuzzySet> MemberSets;
	private double minRange, maxRange;

	public FuzzyVariable()
    {
		minRange = 0.0;
		maxRange = 0.0;
		MemberSets = new Dictionary<string, FuzzySet> ();
	}

	public FuzzySet AddLeftShoulderSet( string name, double minBound, 
										double peak, double maxBound){
		FuzzySet_LeftShoulder leftSet = new FuzzySet_LeftShoulder ( peak, 
																	peak - minBound, 
																	maxBound - peak);
		MemberSets.Add (name, leftSet);
		return leftSet;
	}

	public FuzzySet AddRightShoulderSet( string name, double minBound, 
											double peak, double maxBound){
		FuzzySet_RightShoulder rightSet = new FuzzySet_RightShoulder ( peak, 
			peak - minBound, 
			maxBound - peak);
		MemberSets.Add (name, rightSet);
		return rightSet;
	}

	public FuzzySet AddTriangularSet( string name, double minBound, double peak, double maxBound)
    {
		FuzzySet_Triangle triangleSet = new FuzzySet_Triangle ( peak, 
			peak - minBound, 
			maxBound - peak);
		MemberSets.Add (name, triangleSet);
		return triangleSet;
	}

	public void Fuzzify( double val )
    {
		foreach (FuzzySet fuzzySet in MemberSets.Values)
        {
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
        sum_confidence = sum_confidence == 0 ? 1 : sum_confidence;
        return sum_repTimeConfidence / sum_confidence;
	}

	public double DeFuzzifyCentroid( int NumSamples )
    {
		return 0.0;
	}
}
