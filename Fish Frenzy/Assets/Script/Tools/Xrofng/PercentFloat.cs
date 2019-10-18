using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    [System.Serializable]
    public class PercentFloat
    {
        public float Value = 0;
        public float MaxVal = 1;

        public enum ExceedPercentType
        {
            Clamp,
            Loop
        }
        public ExceedPercentType exceedPercentType = ExceedPercentType.Clamp;

        public float Ratio
        {
            get
            {
                return Value / MaxVal;
            }
        }

        public PercentFloat(float startVal = 0, float maxVal = 100, ExceedPercentType exceedPercent = ExceedPercentType.Clamp)
        {
            Value = startVal;
            MaxVal = maxVal;
            exceedPercentType = exceedPercent;
        }

        public void AddValue(float increment)
        {
            Value += increment;
            OnExceedLimit();
        }

        public void SetValue(float val)
        {
            Value = val;
            OnExceedLimit();
        }

        public void SetValue01(float per)
        {
            Value = per*MaxVal;
            OnExceedLimit();
        }

        private void OnExceedLimit()
        {
            if (exceedPercentType == ExceedPercentType.Clamp)
            {
                Value = Mathf.Clamp(Value, 0, MaxVal);
            }

            if (exceedPercentType == ExceedPercentType.Loop)
            {
                Value = Value % MaxVal;
            }
        }

        public bool IsAtMax
        {
            get
            {
                return Ratio >= 1;
            }
        }
    }
}


