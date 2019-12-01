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
        GameObject temporaryEffectHost = Instantiate(vFX.effect, location,Quaternion.identity);

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
        Vector3 targetScreenPos = MainCam.WorldToScreenPoint(location);
        Vector2 halfSize = EffectCanvas.sizeDelta/2.0f;

        targetScreenPos.x = Mathf.Clamp(targetScreenPos.x, -halfSize.x, halfSize.x);
        targetScreenPos.y = Mathf.Clamp(targetScreenPos.y, -halfSize.y, halfSize.y);
        RectTransform temporaryEffectHost = Instantiate(vFX.effect, EffectCanvas.transform).GetComponent<RectTransform>();

        temporaryEffectHost.anchoredPosition = targetScreenPos;

        Vector3 targetDirection = EffectCanvas.position - location;
        float angle = Vector3.Angle(targetDirection, temporaryEffectHost.transform.up);
        temporaryEffectHost.Rotate(Vector3.forward * angle*Mathf.Deg2Rad);

        Destroy(temporaryEffectHost.gameObject, vFX.destroyEffectDelay);

        return temporaryEffectHost.gameObject;
    }
}
