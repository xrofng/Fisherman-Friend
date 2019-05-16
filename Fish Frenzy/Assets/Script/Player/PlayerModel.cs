using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public GameObject Hat
    {
        get
        {
            Transform[] child;
            child = GetComponentsInChildren<Transform>();
            GameObject hat = null;
            if (child != null)
            {
                hat = FindHat(child);
            }
            else
            {
                // Try again, looking for inactive GameObjects
                Transform[] childInactive = GetComponentsInChildren<Transform>(true);
                hat = FindHat(child);
            }
            return hat;
        }
    }

    protected GameObject FindHat(Transform[] arr)
    {
        foreach(Transform a in arr)
        {
            if(a.name == "Hat")
            {
                return a.gameObject;
            }
        }
        return null;
    }
}
