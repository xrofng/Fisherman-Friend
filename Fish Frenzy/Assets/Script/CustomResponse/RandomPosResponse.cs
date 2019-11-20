using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrofng;

namespace OneButton
{
    public class RandomPosResponse : CustomResponse
    {
        public Transform target;

        public RandomFloat radius = new RandomFloat(0, 20);
        public RandomFloat circulatPos = new RandomFloat(0, 360);
        public RandomFloat height = new RandomFloat(0, 10);

        public Vector3 offset = Vector3.zero;

        public override void PerformResponse()
        {
            base.PerformResponse();

            float rad = Mathf.Deg2Rad * circulatPos.Value;
            radius.InitRandom();
            float posX = Mathf.Cos(rad) * radius.Value;
            radius.InitRandom();
            float posZ = Mathf.Sin(rad) * radius.Value;
            target.position = offset + Vector3.right * posX + Vector3.up * height.Value + Vector3.forward * posZ;
        }

        private void OnDrawGizmosSelected()
        {
            if (!target)
            {
                return;
            }
            Vector3 v = target.position + offset;
            Gizmos.DrawWireSphere(v, radius.maxRandom);
            Gizmos.DrawWireSphere(v, radius.maxRandom);
            Vector3 boxsize = new Vector3(radius.maxRandom, height.maxRandom, radius.maxRandom);
            Gizmos.DrawWireCube(v + Vector3.up* height.maxRandom/2.0f, boxsize);
        }
    }

}

