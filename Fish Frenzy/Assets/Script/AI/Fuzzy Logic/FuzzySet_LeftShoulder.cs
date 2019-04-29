using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzySet_LeftShoulder : FuzzySet {

	private double peakPoint, leftOffset, rightOffset;

	public FuzzySet_LeftShoulder( double peak, double left, double right ):
									base ( (peak + (peak - left))/2){
		peakPoint = peak;
		leftOffset = left;
		rightOffset = right;
	}

	public override double CalculateDOM (double val)
	{
		if ( rightOffset == 0.0 && val == peakPoint) {
			return 1.0;
		}
		// if the value is on the left side
		if ( val <= peakPoint && val >= leftOffset) {
			return 1.0;
		}
		// if the value is on the right side
		else if ( val > peakPoint && val < (peakPoint + rightOffset) ) {
			return (val - peakPoint) / -rightOffset + 1.0;
		} else {
			return 0.0;
		}
	}
}
