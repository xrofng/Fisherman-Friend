using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    public static class XDebug
    {
        public static void LogList<T>(List<T> list)
        {
            foreach(T ele in list)
            {
                Debug.Log(ele.ToString());
            }
        }
    }
}
