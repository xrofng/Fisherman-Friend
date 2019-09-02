using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFishingTrident : ActivateOnFishing
{
    public override void OnFishingAction()
    {
        base.OnFishingAction();
        FFGameManager.Instance.PortRoyal.FishPool.RemoveFish(Fish);
    }
}
