using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Xrofng
{
    [System.Serializable]
    public class XGraphic
    {
        public bool showing = true;
        public List<MaskableGraphic> maskableGraphics = new List<MaskableGraphic>();

        public void Show(bool show)
        {
            showing = show;
            foreach (MaskableGraphic maskableGraphic in maskableGraphics)
            {
                maskableGraphic.enabled = show;
            }

        }
    }

}

