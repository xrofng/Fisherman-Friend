using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneButton
{
    [RequireComponent(typeof(BoxCollider))]
    public class ResponseOnHit : ResponseCaller
    {
        public LayerMask hitLayer;
        public string hitTag;

        public enum ColldeType
        {
            BOTH,
            TRIGGER,
            COLLISION
        };
        public ColldeType colldeType = ColldeType.BOTH;

        private void OnTriggerEnter(Collider other)
        {
            if (colldeType == ColldeType.BOTH || colldeType == ColldeType.TRIGGER)
            {
                CheckHit(other.gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (colldeType == ColldeType.BOTH || colldeType == ColldeType.COLLISION)
            {
                CheckHit(collision.gameObject);
            }
        }

        private void CheckHit(GameObject other)
        {
            if (hitLayer == (hitLayer | (1 << other.gameObject.layer))
                && other.tag == hitTag)
            {
                PerformResponse();
            }
        }
    }
}
