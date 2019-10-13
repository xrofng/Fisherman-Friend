using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    private GameObject _hat;
    public GameObject Hat
    {
        get
        {
            if (_hat != null)
            {
                return _hat;
            }
            Transform[] child;
            child = GetComponentsInChildren<Transform>();
            _hat = null;
            if (child != null)
            {
                _hat = FindPart(child, "Hat");
            }
            else
            {
                // Try again, looking for inactive GameObjects
                Transform[] childInactive = GetComponentsInChildren<Transform>(true);
                _hat = FindPart(childInactive, "Hat");
            }
            return _hat;
        }
    }

    protected GameObject _body;
    public GameObject Body
    {
        get
        {
            if (_body != null)
            {
                return _body;
            }
            Transform[] child;
            child = GetComponentsInChildren<Transform>();
            _body = null;
            if (child != null)
            {
                _body = FindPart(child,"Body");
            }
            else
            {
                // Try again, looking for inactive GameObjects
                Transform[] childInactive = GetComponentsInChildren<Transform>(true);
                _body = FindPart(childInactive, "Body");
            }
            return _body;
        }
    }

    protected GameObject FindPart(Transform[] arr,string partName)
    {
        foreach(Transform a in arr)
        {
            if(a.name == partName)
            {
                return a.gameObject;
            }
        }
        return null;
    }

    public Vector3 ModelForward
    {
        get
        {
            return Body.transform.TransformDirection(Vector3.forward);
        }
    }

    public Vector3 ModelRight
    {
        get
        {
            return Body.transform.TransformDirection(Vector3.right);
        }
    }

    public Vector3 ModelDirection(Vector3 dir)
    {
        return Body.transform.TransformDirection(dir);
    }
}
