using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFList<T>
{
    private List<T> list = new List<T>();
    public List<T> List
    {
        get { return list; }
    }

    public int Count
    {
        get { return list.Count; }
    }

    public void Add(T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }

    public void AddRange(List<T> items)
    {
        foreach(T item in items)
        {
            Add(item);
        }
    }


    public void Remove(T item)
    {
        if (list.Contains(item))
        {
            list.Remove(item);
        }
    }

    public void PrintItems()
    {
        string s = "";
        foreach(T item in List)
        {
            s += item.ToString() + '\n';
        }
        Debug.Log(s);
    }
}
