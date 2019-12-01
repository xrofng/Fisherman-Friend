using System;
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
    public Animator LockUI;
    public Vector3 LockUISpawnOffset;
    public GameObject TridentEdge;
    public GameObject AreaIndicator;
    private GameObject _areaIndicator;
    protected GameObject areaIndicator
    {
        get
        {
            if(_areaIndicator == null)
            {
                _areaIndicator = Instantiate(AreaIndicator, transform);
                //_areaIndicator.transform.position
            }
            return _areaIndicator;
        }
    }


    protected HitBoxMelee detection;

    protected List<GameObject> finalTargets;

    /// <summary>
    /// key is object ignored from detection
    /// value is time left for detection allowance
    /// </summary>
    protected Dictionary<GameObject,float> ignoredTargets;

    protected Dictionary<GameObject, Animator> targetLockUIs;


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

        areaIndicator.transform.eulerAngles = new Vector3(0.0f, areaIndicator.transform.eulerAngles.y, areaIndicator.transform.eulerAngles.z);
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
                if (!ignoredTargets.ContainsKey(hitObject))
                {
                    ToggleLockUI(hitObject);
                    finalTargets.Add(hitObject);
                    ignoredTargets.Add(hitObject, IgnoredDuration);
                }
            }
        }
    }

    private void ToggleLockUI(GameObject player)
    {
        if (!targetLockUIs.ContainsKey(player))
        {
            targetLockUIs.Add(player, Instantiate(LockUI,player.transform.position,Quaternion.identity));
            targetLockUIs[player].transform.parent = player.transform;
            targetLockUIs[player].transform.position += LockUISpawnOffset;
        }
        else
        {
            targetLockUIs[player].SetTrigger("relock");
        }
    }

    private void EvaluateIgnoredTarget()
    {
        List<GameObject> _ignoredTarget = new List<GameObject>(ignoredTargets.Keys);
        foreach (GameObject ignored in _ignoredTarget)
        {
            ignoredTargets[ignored] -= Time.deltaTime;
            if (ignoredTargets[ignored] <= 0)
            {
                ignoredTargets.Remove(ignored);
            }
        }
    }

    protected override void PerformSpecialDown()
    {
        base.PerformSpecialDown();
        Player.Animation.Animator.SetTrigger("s_holdtrident");
        finalTargets = new List<GameObject>();
        ignoredTargets = new Dictionary<GameObject, float>();
        targetLockUIs = new Dictionary<GameObject, Animator>();
        ShowAreaIndicator(true);
    }

    protected override void PerformSpecialUp()
    {
        base.PerformSpecialUp();
        ShowAreaIndicator(false);
        ClearLockUI();
    }

    protected override void OnThrowStart()
    {
        fish.SnapToHold();
        for (int i = 0; i < finalTargets.Count; i++)
        {
            float angle = (360 / finalTargets.Count);
            float z = angle * (i + 1);
            Vector3 cam = Camera.main.transform.eulerAngles;
            MovingObject movingObj = SpawnMovingObject(movingObjects, TridentEdge.transform.position, Quaternion.Euler(cam.x, cam.y, z));
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
        Gizmos.DrawWireSphere(this.transform.position, Radius);
        Gizmos.DrawWireSphere(this.transform.position + transform.TransformDirection(Vector3.forward) * Range, Radius);
    }

    protected void SetDetection(bool allow)
    {
        detection.enabled = allow;
    }

    public override void OnPlayerDeath()
    {
        base.OnPlayerDeath();
        ShowAreaIndicator(false);
        ClearLockUI();
    }

    public override void OnDehydrate()
    {
        base.OnDehydrate();
        ShowAreaIndicator(false);
        ClearLockUI();
    }

    private void ClearLockUI()
    {
        if (targetLockUIs ==null)
        {
            return;
        }
        foreach (GameObject playr in targetLockUIs.Keys)
        {
            Destroy(targetLockUIs[playr].gameObject);
        }
        targetLockUIs.Clear();
    }

    private void ShowAreaIndicator(bool show)
    {
        areaIndicator.SetActive(show);
    }
}
