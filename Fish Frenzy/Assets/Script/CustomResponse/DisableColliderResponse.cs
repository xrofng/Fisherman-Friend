using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrofng;

namespace OneButton
{
    public class DisableColliderResponse : CustomResponse
    {
        public List<Collider> Colliders = new List<Collider>();

        public override void PerformResponse()
        {
            base.PerformResponse();

            foreach(Collider collider in Colliders)
            {
                collider.enabled = false;
            }
        }
    }

}

