using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectMovement : MonoBehaviour
{
    /// If true, this platform will only moved when commanded to by another script
    public bool ScriptActivated = false;

    public bool isLoop = false;

    private bool startedMove;
    private bool finishedMove;

    public enum MoveFactor
    {
        Time,
        Speed
    }
    public enum Move_Position
    {
        From,
        To
    }
    [Header("MoveFactor")]
    public MoveFactor moveFactor;
    public float moveDuration=0.25f;
    public float moveSpeed=100;

    private Vector3 moveDirection;
    private float movingCount = 0;
    private Vector3 targetPosition;
    private Vector3 originPosition;

    [Header("Move _ Position")]
    public Move_Position move_Position;
    public Vector3 fromPosition;
    public Vector3 toPosition;

    void Start ()
    {
        if(move_Position == Move_Position.From)
        {
            targetPosition = this.transform.position;
            originPosition = fromPosition + transform.position;
            this.transform.position = originPosition;
        }
        if (move_Position == Move_Position.To)
        {
            targetPosition = toPosition + this.transform.position;
            originPosition = this.transform.position;
        }
        moveDirection = (targetPosition - originPosition).normalized;

        if (!ScriptActivated)
        {
            StartMove();
        }
    }

    public void StartMove()
    {
        startedMove = true;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!startedMove)
        {
            return;
        }
        if(moveFactor == MoveFactor.Time)
        {
            movingCount += Time.deltaTime;
            float fracJourney = movingCount / moveDuration;
            transform.position = Vector3.Lerp(originPosition, targetPosition, fracJourney);
            if (fracJourney == 1)
            {
                FinishMove();
            }
        }
        if (moveFactor == MoveFactor.Speed)
        {
            transform.Translate(moveDirection*moveSpeed*Time.deltaTime);
            if (Vector3.Distance(this.transform.position, targetPosition) < 1.0f)
            {
                FinishMove();
            }
        }

    }

    void FinishMove()
    {
        if (!isLoop)
        {
            finishedMove = true;
        }
        else
        {
            transform.position = originPosition;
        }
    }

    void FixedUpdate()
    {

    }

    [Header("Debug")]
    public bool drawGizmo;
    public Color gizmoColor = Color.white;
    public Color gizmoSelectColor = Color.grey;

    void OnDrawGizmos()
    {
        if (!drawGizmo)
        {
            return;
        }
        Gizmos.color = gizmoColor;
        Vector3 offset = fromPosition;
        if (move_Position == Move_Position.To)
        {
            offset = toPosition;
        }
        Gizmos.DrawWireSphere(offset + transform.position, 1);
    }

    void OnDrawGizmosSelected()
    {
        if (!drawGizmo)
        {
            return;
        }
        Gizmos.color = gizmoSelectColor;
        Vector3 offset = fromPosition;
        if (move_Position == Move_Position.To)
        {
            offset = toPosition;
        }
        Gizmos.DrawWireSphere(offset + transform.position, 1);
    }

}
