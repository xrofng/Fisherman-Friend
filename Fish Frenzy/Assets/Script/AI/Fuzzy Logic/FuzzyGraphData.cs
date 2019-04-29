using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FuzzyGraphData
{
    [SerializeField]
    public FuzzySetIdentifier setIdentifier;

    [SerializeField]
    public float leftValue;

    [SerializeField]
    public float midValue;

    [SerializeField]
    public float rightValue;

    public enum FuzzySetType
    {
        LeftShoulder,
        Triangle,
        RightShoulder,
    }
    [SerializeField]
    public FuzzySetType setType;

    public FuzzyGraphData(FuzzySetType type)
    {
        setType = type;
        if(type == FuzzySetType.LeftShoulder)
        {
            leftValue = 0;
            midValue = 25;
            rightValue = 50;
        }
        if (type == FuzzySetType.Triangle)
        {
            leftValue = 25;
            midValue = 50;
            rightValue = 75;
        }
        if (type == FuzzySetType.RightShoulder)
        {
            leftValue = 50;
            midValue = 75;
            rightValue = 100;
        }
    }

}
