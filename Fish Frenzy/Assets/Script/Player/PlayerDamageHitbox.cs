using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHitbox : PlayerAbility
{
    public Transform HitBoxCollections;
    private Dictionary<string, HitBoxMelee> hitboxes = new Dictionary<string, HitBoxMelee>();

    public HitBoxMelee GetHitBox(string hitBoxName)
    {
        if (!hitboxes.ContainsKey(hitBoxName))
        {
            if (FindHitBox(hitBoxName))
            {
                return hitboxes[hitBoxName];
            }
        }
        else
        {
            return hitboxes[hitBoxName];
        }

        return null;
    }

    private HitBoxMelee FindHitBox(string hitBoxName)
    {
        foreach(Transform hitBox in HitBoxCollections)
        {
            if(hitBox.gameObject.name == hitBoxName)
            {
                HitBoxMelee onHit = hitBox.GetComponent<HitBoxMelee>();
                onHit.gameObject.layer = LayerMask.NameToLayer("Fish" + Player.playerID);
                if (onHit)
                {
                    hitboxes.Add(hitBoxName, onHit);
                    return onHit;
                }
            }
        }
        return null;
    }
}
