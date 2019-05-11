using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coast : MonoBehaviour
{
    private GameObject _faceSea;
    public GameObject FaceSea
    {
        get
        {
            if (!_faceSea)
            {
                _faceSea = GetComponentInChildren<Transform>().gameObject;
            }
            return _faceSea;
        }
    }
}
