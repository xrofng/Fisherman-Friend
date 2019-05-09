using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Goal_Fisherman : Goal
    {
        public Player ownerPlayer;

        public override bool IsValid()
        {
            return ownerPlayer;
        }
    }
}