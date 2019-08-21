using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFgui : MonoBehaviour
{
    private GUIRecolorer _guiRecolorer;
    public GUIRecolorer GUIRecolorer
    {
        get
        {
            if (!_guiRecolorer)
            {
                _guiRecolorer = GetComponent<GUIRecolorer>();
            }
            return _guiRecolorer;
        }
    }

    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
}
