using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Xrofng
{
    public class XButton : Button
    {
        private bool pointerPoint = false;

        public Action OnHighlightAction;
        public Action OnEnterAction;
        public Action OnExitAction;
        public Action OnClickAction;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            InvokeActon(OnClickAction);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            InvokeActon(OnEnterAction);
            pointerPoint = true;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            InvokeActon(OnExitAction);
            pointerPoint = false;
        }

        private void Update()
        {
            if (pointerPoint)
            {
                InvokeActon(OnHighlightAction);
            }
        }

        protected bool InvokeActon(Action action)
        {
            if (action != null)
            {
                action();
                return true;
            }
            return false;
        }

    }

}

