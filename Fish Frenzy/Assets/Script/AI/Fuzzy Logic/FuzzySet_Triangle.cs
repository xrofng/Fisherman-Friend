using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzySet_Triangle : FuzzySet {

	private double peakPoint, leftOffset, rightOffset;

	public FuzzySet_Triangle( double mid, double left, double right ) : base(mid){
		peakPoint = mid;
		leftOffset = left;
		rightOffset = right;
	}

	public override double CalculateDOM (double val)
	{
		if ((rightOffset == 0.0 && peakPoint == val) || 
			(leftOffset == 0.0 && peakPoint == val) ) {
			return 1.0;
		}

		//if the value is on the left side
		if (val <= peakPoint && val >= (peakPoint - leftOffset)) {
			return (val - (peakPoint - leftOffset)) / leftOffset;
		}
		//if the value is on the right side
		else if ( val > peakPoint && val < (peakPoint + rightOffset) ) {
			return (val - peakPoint) / -rightOffset + 1.0;
		} 
		else {
			return 0.0;
		}
	}
}
