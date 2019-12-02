using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Xrofng
{
    [System.Serializable]
    public class XList<T>
    {
        public List<T> Elements = new List<T>();
        private int _currentIndex;
        public T CurrentElement
        {
            get
            {
                return Elements[_currentIndex];
            }
        }

        public void ChangeIndex(int increment)
        {
            int nexIndex = _currentIndex + increment;
            nexIndex = Mathf.Clamp(nexIndex, 0, Elements.Count);

            SetIndex(nexIndex);
        }

        private void SetIndex(int ind)
        {
            _currentIndex = ind;
        }
    }

}