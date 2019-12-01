using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public enum EffectSpawnOn
    {
        WORLD,
        CAMERA
    }
    public VisualEffect HitPlayerEffect;
    public EffectSpawnOn HitEffectOn;
    public Vector3 HitEffectSpawnOffset;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player)
        {
            OnCollidePlayer(player);
        }
    }

    private void OnCollidePlayer(Player player)
    {
        if (player.Death)
        {
            return;
        }


        if (HitEffectOn == EffectSpawnOn.WORLD)
        {
            EffectManager.Instance.PlayEffect(HitPlayerEffect, player.transform.position + HitEffectSpawnOffset);
        }
        else
        {
            EffectManager.Instance.PlayEffectOnCamera(HitPlayerEffect, player.transform.position + HitEffectSpawnOffset);
        }
        player.KillPlayer();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + HitEffectSpawnOffset,1);
    }
}
