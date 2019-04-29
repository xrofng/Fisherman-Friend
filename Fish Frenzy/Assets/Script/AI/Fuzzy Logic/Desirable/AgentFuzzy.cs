using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentFuzzy : MonoBehaviour
{
    public GameObject desireKeeper;

    void Start()
    {
        foreach(Desirable desire in desireKeeper.GetComponents<Desirable>())
        {
            desire.InitFuzzy();
            Debug.Log(desire.GetDesirability());
        }
    }
}
