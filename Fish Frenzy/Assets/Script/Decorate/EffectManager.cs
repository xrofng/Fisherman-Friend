using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VisualEffect
{
    public GameObject effect;
    public float destroyEffectDelay;
}

/// <summary>
/// This persistent singleton handles effect playing
/// </summary>
[AddComponentMenu("Corgi Engine/Managers/Effect Manager")]
public class EffectManager : PersistentSingleton<EffectManager>
{
    private RectTransform _effectCanvas;
    public RectTransform EffectCanvas
    {
        get
        {
            if(_effectCanvas == null)
            {
                _effectCanvas = GameObject.Find("EffectCanvas").GetComponent<RectTransform>();
            }
            return _effectCanvas;
        }
    }

    private Camera _mainCam;
    public Camera MainCam
    {
        get
        {
            if (_mainCam == null)
            {
                _mainCam = FindObjectOfType<MultiPlayerCamera>().GetComponent<Camera>();
            }
            return _mainCam;
        }
    }

    /// <summary>
    /// Play particle on 
    /// </summary>
    /// <param name="vFX"></param>
    /// <returns></returns>
    public virtual GameObject PlayEffect(VisualEffect vFX, Vector3 location)
    {
        GameObject temporaryEffectHost = Instantiate(vFX.effect, location, vFX.effect.transform.rotation);

        Destroy(temporaryEffectHost.gameObject, vFX.destroyEffectDelay);

        return temporaryEffectHost;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vFX"></param>
    /// <param name="location"></param>
    public virtual GameObject PlayEffectOnCamera(VisualEffect vFX, Vector3 location)
    {
        Vector3 dir = location - FFGameManager.Instance.PortRoyal.StageCenterCeil.transform.position;
        float y = dir.z;
        if(dir.y*dir.y > dir.z * dir.z)
        {
            y = dir.y;
        }
        Vector2 topViewDir = new Vector2(dir.x, y).normalized;

        RectTransform temporaryEffectHost = Instantiate(vFX.effect, EffectCanvas.transform).GetComponent<RectTransform>();

        Vector2 halfSize = EffectCanvas.sizeDelta / 2.0f;

        Vector2 spawnPos = new Vector2(topViewDir.x * halfSize.x, topViewDir.y* halfSize.y);
        temporaryEffectHost.anchoredPosition = spawnPos;
        Destroy(temporaryEffectHost.gameObject, vFX.destroyEffectDelay);

        return temporaryEffectHost.gameObject;
    }

    private float CeilFloor(float va)
    {
        if (va > 0)
        {
            return Mathf.Ceil(va);
        }
        else if(va<0)
        {
            return Mathf.Floor(va);
        }
        return 0;
    }

    protected Vector3 KeepInsideCamera(Vector2 screenPos)
    {
        screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width - 0);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height - 0);
        return screenPos;
    }

    /*
      
        Vector3 targetViewPos = MainCam.WorldToViewportPoint(location);
        Vector2 targetScreenPos = MainCam.ViewportToScreenPoint(targetViewPos);

        Vector2 effectPos = KeepInsideCamera(targetScreenPos);
        RectTransform temporaryEffectHost = Instantiate(vFX.effect, EffectCanvas.transform).GetComponent<RectTransform>();

        Debug.Log("effectPos" + targetViewPos);
        temporaryEffectHost.anchoredPosition = targetViewPos;

        Vector3 targetDirection = EffectCanvas.position - location;
        float angle = Vector3.Angle(targetDirection, temporaryEffectHost.transform.up);
        temporaryEffectHost.Rotate(Vector3.forward * angle*Mathf.Deg2Rad);

        Destroy(temporaryEffectHost.gameObject, vFX.destroyEffectDelay);

        return temporaryEffectHost.gameObject;*/
}
