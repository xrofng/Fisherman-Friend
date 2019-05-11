using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFRandom
{
    public float max;
    public float min;

    public FFRandom(float min,float max)
    {
        this.min = min;
        this.max = max;
    }

    public FFRandom(int min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public FFRandom(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public FFRandom(float min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public float RandomFloat()
    {
        return Random.Range(min, max);
    }

    public int RandomInt()
    {
        return (int)RandomFloat();
    }
}
