using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    public static class XDebug
    {
        public static void PrintListItems<T>(List<T> list)
        {
            string s = "";
            foreach (T item in list)
            {
                s += item.ToString() + '\n';
            }
            Debug.Log(s);
        }

        public static void PrintDictKeys<T,R>(Dictionary<T,R> dict)
        {
            string s = "";
            foreach (T item in dict.Keys)
            {
                s += item.ToString() + '\n';
            }
            Debug.Log(s);
        }

        public static void PrintDictItems<T, R>(Dictionary<T, R> dict)
        {
            string s = "";
            foreach (R item in dict.Values)
            {
                s += item.ToString() + '\n';
            }
            Debug.Log(s);
        }
    }
}
