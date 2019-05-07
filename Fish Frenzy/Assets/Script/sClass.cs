using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadOnlyAttribute : PropertyAttribute { }

public enum VectorComponent
{
    x = 0,
    y,
    z,
}

public static class sClass {

    // Use this for initialization
    public static int getSign(float f, float zero)
    {
        if (f > zero)
        {
            return 1;
        }
        else if (f < -zero)
        {
            return -1;
        }
        return 0;
    }
    public static bool intervalCheck(float f, float inte, float rval, bool outOf)
    {
        if (outOf)
        {
            if (f < inte || f > rval)
            {
                return true;
            }

        }
        else
        {
            if (f > inte && f < rval)
            {
                return true;
            }
        }
        return false;
    }

    public static int findMinOfArray(float[] arr)
    {
        int index =0;
        float min = float.MaxValue;
        for(int i = 0; i < arr.Length; i++)
        {
            if(arr[i] < min)
            {
                min = arr[i];
                index = i;
            }
        }
        return index;
    }


    
    public static Vector3 SetVector3(Vector3 original , VectorComponent vc, float value)
    {
        Vector3 changed = original;
        changed[(int)vc] = value;
        return changed;
    }
    public static Vector3 GetVector3_IgnoreElement(Vector3 original, VectorComponent vc)
    {
        return SetVector3(original, vc, 0);
    }
    public static float DistanceIgnored(VectorComponent vc, Vector3 start, Vector3 end)
    {
        Vector3 endIgnored_Pos = SetVector3(end, VectorComponent.y, 0);
        Vector3 startIgnored_Pos = SetVector3(start, VectorComponent.y, 0);
        return Vector3.Distance(endIgnored_Pos, startIgnored_Pos);
    }

    public enum quaternionComponent
    {
        x = 0,
        y,
        z,
        w,
    }
    public static Quaternion setQuaternion(Quaternion original, quaternionComponent qc, float value)
    {
        Quaternion changed = original;
        changed[(int)qc] = value;
        return changed;
    }

    public static Color ChangeColorAlpha(Color original , float value)
    {
        Color changed = original;
        changed[3] = value;
        changed[3] = Mathf.Clamp01(changed[3]);
        return changed;
    }

    public static void PrintListContent<T>(List<T> list, string listTitle)
    {
        foreach (T element in list)
        {
            listTitle += element + ",";
        }
        Debug.Log(listTitle);
    }
}

