using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_Shark : StageInteraction
{
    public Vector3 bounceForce = new Vector3(0, 5, 0);
    public int BiteFrame = 10;
    // Use this for initialization
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        hitBox.FreezeFramesOnHit = BiteFrame;

        last_posiion = transform.position;

        CurrentWayPointID = Random.Range(0, path_objs.Count);
        transform.position = path_objs[CurrentWayPointID].position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        Move();
    }

    public override void OnPlayerCollide(Player _player)
    {
        // Check player will be ignored from recently collide
        if (IgnorePlayerWithID.Contains(_player.playerID))
        {
            return;
        }
        AddIgnorePlayerID(_player.playerID);
        hitBox.CauseDamage();
    }
    public Color rayColor = Color.red;

    [Header("Path Edit")]
    public float reachDistance = 2.0f;
    public List<Transform> path_objs = new List<Transform>();
    Transform[] theArray;
    public Transform PathSet;
    // Draw Path
    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        theArray = PathSet.GetComponentsInChildren<Transform>();
        path_objs.Clear();

        foreach (Transform pathobj in theArray)
        {
            if (pathobj != this.transform)
            {
                path_objs.Add(pathobj);
            }
        }

        for (int i = 0; i < path_objs.Count; i++)
        {
            Vector3 pos = path_objs[i].position;
            if (i == 0)
            {
                Gizmos.DrawSphere(pos, reachDistance);
                Gizmos.DrawLine(path_objs[path_objs.Count - 1].position, pos);
            }
            if (i > 0)
            {
                Vector3 previous = path_objs[i - 1].position;

                Gizmos.DrawLine(previous, pos);
                Gizmos.DrawSphere(pos, reachDistance);
            }
        }
    }

    [Header("Move")]
    public int CurrentWayPointID;
    public float walkSpeed;
    public float rotationSpeed = 5.0f;
    private bool prevIsGround = false;

    Vector3 last_posiion;
    Vector3 current_posiion;

    // Update is called once per frame
    void Move()
    {
        Vector3 targetPos = path_objs[CurrentWayPointID].position;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);
        Quaternion rotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        float distance = Vector3.Distance(targetPos, transform.position);
        //transform.LookAt(targetPos, Vector3.up);
        if (distance <= reachDistance)
        {
            CurrentWayPointID += 1;
        }
        if (CurrentWayPointID >= path_objs.Count)
        {
            CurrentWayPointID = 0;
        }
    }

    public Vector3 getLowestPlayerPoint()
    {
        return new Vector3(transform.position.x, transform.position.y + _collider.center.y + _collider.height / 2.0f, transform.position.z);
    }
    void PlayBiteAnimation()
    {
        _animation.clip = _animation.GetClip("Bite");
        _animation.Play();
    }
}
