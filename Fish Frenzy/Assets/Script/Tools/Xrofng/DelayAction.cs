using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Xrofng
{
    [Serializable]
    public class DelayAction
    {
        public float DurationSecond;
        public Action beforeAction;
        public Action afterAction;

        public IEnumerator coAction()
        {
            if(beforeAction != null)
            {
                beforeAction();
            }
            yield return new WaitForSeconds(DurationSecond);
            if (afterAction != null)
            {
                afterAction();
            }
        }
    }

}