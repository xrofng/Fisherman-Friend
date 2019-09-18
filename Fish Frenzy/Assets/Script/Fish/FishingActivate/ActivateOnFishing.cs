using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnFishing : MonoBehaviour
{
    private Fish m_fish;
    public Fish Fish
    {
        get
        {
            if (m_fish == null)
            {
                m_fish = GetComponent<Fish>();
            }
            return m_fish;
        }
    }

    public virtual void OnFishingAction()
    {

    }
}
