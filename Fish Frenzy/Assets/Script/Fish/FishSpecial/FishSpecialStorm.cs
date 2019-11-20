﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpecialStorm : FishSpecialThrow
{
    [Header("Storm")]
    public float Range;
    public float Radius;
    public LayerMask LayerMask;
    public float IgnoredDuration;
    public GameObject TridentEdge;

    protected HitBoxMelee detection;

    protected List<GameObject> finalTargets;

    /// <summary>
    /// key is object ignored from detection
    /// value is time left for detection allowance
    /// </summary>
    protected Dictionary<GameObject,float> ignoredTarget;

    [Header("SoundEffect")]
    public SoundEffect sfx_attach;
    public SoundEffect sfx_swing;

    public override void OnPlayerHold()
    {
        base.OnPlayerHold();
    }

    public override void OnSpecialActivated()
    {
        base.OnSpecialActivated();
    }

    protected override void PerformSpecialHold()
    {
        base.PerformSpecialHold();

        DetectTarget();

        EvaluateIgnoredTarget();
    }

    private void DetectTarget()
    {
        RaycastHit[] hits;

        Vector3 p1 = Player.transform.position;
        Vector3 p2 = Player.transform.position + Player.PlayerForward * Range;

        hits = Physics.CapsuleCastAll(p1, p2, Radius, Player.PlayerForward, Range, LayerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject != Player.gameObject)
            {
                if (!ignoredTarget.ContainsKey(hitObject))
                {
                    finalTargets.Add(hitObject);
                    ignoredTarget.Add(hitObject, IgnoredDuration);
                }
            }
        }
    }

    private void EvaluateIgnoredTarget()
    {
        List<GameObject> _ignoredTarget = new List<GameObject>(ignoredTarget.Keys);
        foreach (GameObject ignored in _ignoredTarget)
        {
            ignoredTarget[ignored] -= Time.deltaTime;
            if (ignoredTarget[ignored] <= 0)
            {
                ignoredTarget.Remove(ignored);
            }
        }
    }

    protected override void PerformSpecialDown()
    {
        base.PerformSpecialDown();
        Player.Animation.Animator.SetTrigger("s_holdtrident");
        finalTargets = new List<GameObject>();
        ignoredTarget = new Dictionary<GameObject, float>();
        Debug.Log(finalTargets.Count);
    }

    protected override void PerformSpecialUp()
    {
        base.PerformSpecialUp();
    }

    protected override void OnThrowStart()
    {
        fish.SnapToHold();
        for (int i = 0; i < finalTargets.Count; i++)
        {
            float angle = (360 / finalTargets.Count);
            float z = angle * (i + 1);
            Vector3 cam = Camera.main.transform.eulerAngles;
            MovingObject movingObj = Instantiate(movingObjects, TridentEdge.transform.position, Quaternion.Euler(cam.x, cam.y, z));
            movingObj.HitBox.Owner = Player.gameObject;
            MovingObjToTarget movingObjToTarget = movingObj.GetComponent<MovingObjToTarget>();

            if (movingObjToTarget)
            {
                movingObjToTarget.target = finalTargets[i].transform;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Player.transform.position, Radius);
        Gizmos.DrawWireSphere(Player.transform.position + Player.PlayerForward * Range, Radius);
    }

    protected void SetDetection(bool allow)
    {
        detection.enabled = allow;
    }

    public override void OnDehydrate()
    {
        base.OnDehydrate();
    }

}
