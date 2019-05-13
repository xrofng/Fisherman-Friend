using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof( FFgui))]
public class GUIRecolorer : MonoBehaviour
{
    /// Grayscale Image that will tint when call Recolor
    public List<Image> recolorImages = new List<Image>();
    public List<Image> staticImages = new List<Image>();

    public void Recolor(Color color)
    {
        foreach(Image recolor in recolorImages)
        {
            recolor.color = color;
        }
    }

    public void ResetColor()
    {
        foreach (Image recolor in recolorImages)
        {
            recolor.color = Color.white;
        }
    }
}
