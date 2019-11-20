using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneButton
{
    public class ResponseCaller: MonoBehaviour
    {
        public CustomResponse Response;

        protected void PerformResponse()
        {
            Response.PerformResponse();
        }
    }
}