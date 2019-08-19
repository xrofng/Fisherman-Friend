using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    public enum DurationTimeUnit
    {
        Second,
        Frame
    }

    [System.Serializable]
    public class Duration
    {
        public DurationTimeUnit durationTimeUnit;
        public float duration;
    }
}

