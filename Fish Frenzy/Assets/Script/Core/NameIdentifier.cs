using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NameIdentifier", menuName = "NameIdentifier/NameIdentifier", order = 52)]
public class NameIdentifier : ScriptableObject
{
   
}
[CreateAssetMenu(fileName = "FLVid_", menuName = "NameIdentifier/FLV_Identifier", order = 52)]
public class FuzzyVariableIdentifier : NameIdentifier{}

[CreateAssetMenu(fileName = "FLSid_", menuName = "NameIdentifier/FLS_Identifier", order = 52)]
public class FuzzySetIdentifier : NameIdentifier
{
    public FuzzySetIdentifier(string idName)
    {
        this.name = idName;
    }
    public FuzzySetIdentifier()
    {
    }
}

