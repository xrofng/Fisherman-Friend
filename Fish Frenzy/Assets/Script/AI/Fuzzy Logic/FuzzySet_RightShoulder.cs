﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzySet_RightShoulder : FuzzySet {

	private double peakPoint, leftOffset, rightOffset;

	public FuzzySet_RightShoulder( double peak, double left, double right ):
									base ( (peak + (peak + right) )/2 ){
		peakPoint = peak;
		leftOffset = left;
		rightOffset = right;
	}
	public override double CalculateDOM (double val)
	{
		if ( leftOffset == 0.0 && val == peakPoint) {
			return 1.0;
		}
		//if the value is on the left side
		if (val < peakPoint && val >= (peakPoint - leftOffset)) {
			return (val - (peakPoint - leftOffset)) / leftOffset;
		}
		//if the value is on the right side
		else if (val >= peakPoint && val <= rightOffset) {
			return 1.0;
		} 
		else {
			return 0.0;
		}
	}
}