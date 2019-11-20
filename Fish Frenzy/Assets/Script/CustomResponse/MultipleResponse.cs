using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneButton
{ 
    public class MultipleResponse : CustomResponse
    {
        public List<CustomResponse> customResponses;

        public override void PerformResponse()
        {
            base.PerformResponse();
            foreach(CustomResponse customResponse in customResponses)
            {
                customResponse.PerformResponse();
            }
        }
    }
}