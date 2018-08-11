using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static   bool intervalCheck(float f, float inte, float rval, bool outOf)
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
}

