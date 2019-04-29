using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [CreateAssetMenu(fileName = "WorldState_Equip_", menuName = "WorldState/WorldState_EquipSpecifyWeapon", order = 51)]
    public class WorldState_EquipSpecifyWeapon: WorldState
    {
        public List<string> validWeapon = new List<string>();

    }

}

