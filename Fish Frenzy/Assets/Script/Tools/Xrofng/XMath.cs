﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    public static class Xmath
    {
        /// <summary>
        /// Low Down linear-interpolation
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="t"></param>
        /// <param name="low"></param>
        /// <param name="down"></param>
        /// <returns></returns>
        public static float Ldlerp(float min, float max, float t, float low, float down)
        {
            float up = down + low;
            float fx = ((t * t) * (up - (low * t * t))) / down;
            if(min == 0.0f && max == 1.0f)
            {
                return fx;
            }
            return Mathf.Lerp(min, max, fx);
        }

        /// <summary>
        /// Linear interpolation between Color a and b by t
        /// </summary>
        /// <param name="cA"></param>
        /// <param name="cB"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Color LerpColor(Color cA, Color cB, float t)
        {
            Color c = Color.white;
            c.r = Mathf.Lerp(cA.r, cB.r, t);
            c.g = Mathf.Lerp(cA.g, cB.g, t);
            c.b = Mathf.Lerp(cA.b, cB.b, t);
            c.a = Mathf.Lerp(cA.a, cB.a, t);
            return c;
        }

        public static Quaternion RandomQuaternion()
        {
            float x, y, z, u, v, w, s;
            do { x = Random.Range(-1.0f, 1.0f); y = Random.Range(-1.0f, 1.0f); z = x * x + y * y; } while (z > 1.0f);
            do { u = Random.Range(-1.0f, 1.0f); v = Random.Range(-1.0f, 1.0f); w = u * u + v * v; } while (w > 1.0f);
            s = Mathf.Sqrt((1 - z) / w);
            return new Quaternion(x, y, s * u, s * v);
        }
    }
}


