using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneButton
{
    public class ResponseOnStart : ResponseCaller
    {
        void OnEnable()
        {
            PerformResponse();
        }
    }

}
