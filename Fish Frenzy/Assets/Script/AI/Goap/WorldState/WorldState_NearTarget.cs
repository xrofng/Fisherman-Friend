using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "WorldState_NearTarget_", menuName = "WorldState/WorldState_NearTarget", order = 51)]
    public class WorldState_NearTarget: WorldState
    {
        public GameObject target;
    }

}

