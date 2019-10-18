using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    [System.Serializable]
    public class RandomFloat
    {
        public float minRandom = 0.0f;
        public float maxRandom = 100.0f;
        protected float value;

        protected bool inited = false; 

        public float Value
        {
            get
            {
                if (!inited)
                {
                    InitRandom();
                }
                return value;
            }
        }

        public int IntValue
        {
            get
            {
                return (int)value;
            }
        }

        public bool randomNegativity = false;

        public RandomFloat(float minRandom, float maxRandom)
        {
            this.minRandom = minRandom;
            this.maxRandom = maxRandom;
        }

        public void InitRandom()
        {
            value = Random.Range(minRandom, maxRandom);
            if (randomNegativity)
            {
                value = RandomNegativity(value);
            }
            inited = true;
        }

        public float RandomNegativity(float v)
        {
            int n = Random.Range(0, 2);
            if (n == 0)
            {
                return -v;
            }
            else if (n==1)
            {
                return v;
            }
            return v;
        }
    }
}


