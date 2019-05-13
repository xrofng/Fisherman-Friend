using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentFuzzy : MonoBehaviour
{
    public Agent agent;

    private GameObject desireKeeper;
    public GameObject DesireKeeper
    {
        get
        {
            if (!desireKeeper)
            {
                desireKeeper = Instantiate(new GameObject(),this.transform);
            }
            return desireKeeper;
        }
    }

    public bool initOnStart;

    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        if (initOnStart)
        {
            InitFuzzy();
        }

    }

    protected T AddDesirable<T>() where T : Desirable
    {
        return DesireKeeper.AddComponent<T>();
    }

    protected T[] GetDesirables<T>() where T : Desirable
    {
        return DesireKeeper.GetComponents<T>();
    }

    public T Cast<T>() where T: AgentFuzzy
    {
        return this as T;
    }

    protected void InitFuzzy()
    {
        foreach (Desirable desire in DesireKeeper.GetComponents<Desirable>())
        {
            desire.InitFuzzy();
        }
    }


}
