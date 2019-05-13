using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FuzzySet {

	public double RepresentativeValue { get; set; }

	// Degree of Membership
	public double DOM {	get; set;}

	public FuzzySet(double repVal){
		DOM = 0.0; RepresentativeValue = repVal;
	}

	public abstract double CalculateDOM( double val );

	public void ClearDOM() { DOM = 0.0;	}

    public void SetConfidenceWithOR(double val)
    {
        if (DOM < val)
        {
            DOM = val;
        }
    }
}
