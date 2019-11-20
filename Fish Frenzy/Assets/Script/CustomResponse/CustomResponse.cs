using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneButton
{
    public class CustomResponse: MonoBehaviour
    {
        private void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {

        }

        public virtual void PerformResponse()
        {

        }

        public virtual bool CheckEndResponse()
        {
            return true;
        }
    }
}