using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_Shark : StageInteraction
{
    [Header("Path Edit")]
    public float reachDistance = 2.0f;
    
    public List<Transform> path_objs = new List<Transform>();
    Transform[] theArray;
    public Transform PathSet;

    [Header("Bloodthirst")]
    public bool randomStartWayPoint = true;
    public bool randomClockWise = true;
    public SoundEffect sfx_Bite;
    protected int CurrentWayPointID;
    protected int NearestWayPointID;
    public float BloodThirstSpeed;
    public float NormalSwimSpeed;
    public int BiteFrame = 10;
    private bool stopMove = false;
    public float speed
    {
        get
        {
            if (DetectedPlayer)
            {
                return BloodThirstSpeed;
            }
            return NormalSwimSpeed;
        }
    }
    public float rotationSpeed = 5.0f;
    public int cClockwise = 1;

    List<GameObject> detects = new List<GameObject>();
    List<int> wayIDs = new List<int>();
    public bool DetectedPlayer = false;

    [Header("Debug")]
    public bool showRay = true;
    public Color rayColor = Color.red;


    // Use this for initialization
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        hitBox.FreezeFramesOnHit = BiteFrame;

        if (randomStartWayPoint)
        {

            CurrentWayPointID = Random.Range(0, path_objs.Count);
            transform.position = path_objs[CurrentWayPointID].position;
        }
        if (randomClockWise)
        {
            cClockwise = Random.Range(0, 2); 
            if(cClockwise == 0) { cClockwise = -1; }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        Move();
        PlayerDetection();
    }

    public override void OnPlayerCollide(Player _player)
    {
        StartCoroutine(OneHitKill(_player));
    }

    IEnumerator OneHitKill(Player _player)
    {
        int frameCount = 0;
        stopMove = true;
        _player.AddAbilityInputIntercepter(this);
        
        // play animation feedback
        Animation.ChangeAnimState(1, true, 0);
        _player.Animation.ChangeAnimState((int)PlayerAnimation.Anim.Eaten, _player._cPlayerState.eatenFrameDuration, true , (int)_player._cPlayerAnimator.GetIdleAnimation());

        SoundManager.Instance.PlaySound(sfx_Bite, this.transform.position);
        while (frameCount < BiteFrame)
        {
            yield return new WaitForEndOfFrame();        frameCount++;
        }
        stopMove = false;
        MatchResult.Instance.StoreAttacker(_player.playerID, this.gameObject);
        _player.KillPlayer();
        _player.RemoveAbilityInputIntercepter(this);
    }

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
            if (!showRay) { return; }
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
    
    void Move()
    {
        if (stopMove)
        {
            return;
        }
        Vector3 targetPos = path_objs[CurrentWayPointID].position;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        Quaternion rotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        float distance = Vector3.Distance(targetPos, transform.position);
        //transform.LookAt(targetPos, Vector3.up);
        if (distance <= reachDistance)
        {
            ChangeCurrentWayPoint(cClockwise);
        }
      
    }

    void ChangeCurrentWayPoint(int changeValue)
    {
        CurrentWayPointID += cClockwise;
        if (CurrentWayPointID >= path_objs.Count)
        {
            CurrentWayPointID = 0;
        }
        if (CurrentWayPointID < 0)
        {
            CurrentWayPointID = path_objs.Count - 1;
        }
    }
    
    int NextWayPointID(int current)
    {
        int next = current + 1;
        if (next >= path_objs.Count)
        {
            next = 0;
        }
        return next;
    }
    void PlayerDetection()
    {
        detects.Clear();
        wayIDs.Clear();

        for (int i = 0; i < path_objs.Count; i++)
        {
            RaycastHit hit;
            Vector3 begin = path_objs[i].position;
            Vector3 target = path_objs[NextWayPointID(i)].position;

            float distance = Vector3.Distance( target , begin);
            Vector3 direction = Vector3.Normalize( target - begin);

            Vector3 side1 = path_objs[i].TransformDirection(Vector3.up);
            Vector3 side2 = target - begin;
            Vector3 perp  = Vector3.Normalize( Vector3.Cross(side1, side2));

            // Does the ray intersect any objects excluding the player layer
            int frequent = 16;
            int h = 20;
            Vector3 perpOffset = ( perp*h) / frequent;
            
            for (int j = -frequent / 4; j < frequent - (frequent/4); j++)
            {
                Vector3 rayStart = begin + (perpOffset * j);
                if (Physics.Raycast(rayStart, direction, out hit, distance))
                {
                   
                    if (hit.collider.gameObject.GetComponent<Player>())
                    {
                        DebugRay(rayStart, direction * distance, Color.red);
                        if (!detects.Contains(hit.collider.gameObject))
                        {
                            detects.Add(hit.collider.gameObject);
                            wayIDs.Add(i);
                        }
                    }else
                    {
                        DebugRay(rayStart, direction * distance, Color.blue);
                    }
                }
                else
                {
                    DebugRay(rayStart, direction * distance, Color.blue);
                }
            }
        }
        
        if (detects.Count > 0)
        {
            HeadToNearestPlayer(detects, wayIDs);
            //if (DetectedPlayer == false)
            //{
            //    ChangeCurrentWayPoint(cClockwise);
            //}
            DetectedPlayer = true;
        }

        else
        {
            DetectedPlayer = false;
        }
    }

    void HeadToNearestPlayer(List<GameObject> players, List<int> wayIDs)
    {
        float nearestDistance = float.MaxValue;
        NearestWayPointID = int.MaxValue;
        for(int i = 0; i < players.Count; i++)
        {
            float distance = (CurrentWayPointID - wayIDs[i])/2.0f;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                NearestWayPointID = wayIDs[i];
            }
        }
        
        int MeasureDistance = CurrentWayPointID;
        int StepRound = int.MaxValue;
        for (int i =0; i < path_objs.Count; i++)
        {
            if (MeasureDistance == NearestWayPointID)
            {
                StepRound = i;
                break;
            }
            MeasureDistance += cClockwise;
        }
        
        if (StepRound > path_objs.Count / 2.0f)
        {
            cClockwise *= -1;
        }
    }

    void DebugRay(Vector3 start, Vector3 dir, Color col)
    {
        if (!showRay) { return; }
        Debug.DrawRay(start, dir, col);
    }
}
