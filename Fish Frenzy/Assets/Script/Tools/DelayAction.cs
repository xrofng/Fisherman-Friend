using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Xrofng
{
    [Serializable]
    public class DelayAction
    {
        public Duration Duration;
        public Action beforeAction;
        public Action afterAction;

        public IEnumerator coAction()
        {
            if(beforeAction != null)
            {
                beforeAction();
            }
            if (Duration.durationTimeUnit == DurationTimeUnit.Frame)
            {
                int frameCount = 0;
                while (frameCount < Duration.duration)
                {
                    yield return new WaitForEndOfFrame();
                    frameCount += 1;
                }
            }
            if(Duration.durationTimeUnit == DurationTimeUnit.Second)
            {
                yield return new WaitForSeconds(Duration.duration);
            }
            if (afterAction != null)
            {
                afterAction();
            }
        }
    }

}