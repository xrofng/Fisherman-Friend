using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrofng;

namespace OneButton
{
    public class DestroyResponse : CustomResponse
    {
        public List<GameObject> GameObjects = new List<GameObject>();

        public override void PerformResponse()
        {
            base.PerformResponse();

            foreach(GameObject GameObject in GameObjects)
            {
                Destroy(GameObject);
            }
        }
    }

}

