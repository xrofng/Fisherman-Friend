using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrofng;

public class SlidePlatform : MonoBehaviour
{
    public enum MovementMode
    {
        Linear,
        EaseInOut
    }

    public enum FinishAction
    {
        Stop,
        Destroy,
        Loop,
    }

    public enum NextAuthorization
    {
        StartOver,
        BackAndForth
    }

    public MovementMode movementMode = MovementMode.Linear;

    public FinishAction finishAction;

    public NextAuthorization nextAuthorization;

    public float MovementSpeed = 3;

    public Vector3 destinationPosition;

    protected Vector3 _initialPosition;
    protected Vector3 _finalPosition;
    protected float _distanceToNextPoint;
    protected bool _endReached = false;
    protected bool _scriptActivatedAuthorization = false;
    public float MinDistanceToGoal = .1f;

    public DelayAction NextRunDelayAction;

    protected float _totalDuration;
    protected float _currentTime;

    public Action onFinishedCallback = delegate { };
    public bool MoveOnStart;

    [Header("Debug")]
    public GizmosShape Gizmo = new GizmosShape();

    /// <summary>
	/// Initialization
	/// </summary>
	protected virtual void Start()
    {
        Initialization();
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Initialization()
    {
        if (!_endReached)
        {
            SetNewFinalPosition(transform.position + destinationPosition);
        }
        if (MoveOnStart)
        {
            AuthorizeMovement();
        }
    }

    /// <summary>
	/// On update we keep moving along the path
	/// </summary>
	protected virtual void Update()
    {
        // if the path is null we exit, if we only go once and have reached the end we exit, if we can't move we exit
        if (_endReached || !_scriptActivatedAuthorization)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        // we move our object
        MoveAlongThePath();

        // we decide if we've reached our next destination or not, if yes, we move our destination to the next point 
        _distanceToNextPoint = (transform.position - _finalPosition).magnitude;
        if (_distanceToNextPoint <= MinDistanceToGoal)
        {
            OnFinishMove();
        }
    }

    /// <summary>
	/// Moves the object along the path according to the specified movement type.
	/// </summary>
	public virtual void MoveAlongThePath()
    {
        if (movementMode == MovementMode.Linear)
        {
            transform.position = Vector3.MoveTowards(transform.position, _finalPosition, Time.deltaTime * MovementSpeed);
        }
        else if (movementMode == MovementMode.EaseInOut)
        {
            _currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_initialPosition, _finalPosition, Mathf.SmoothStep(0f, 1f, _currentTime / _totalDuration));
        }
    }

    protected void OnFinishMove()
    {
        _endReached = true;
        if (finishAction == FinishAction.Stop)
        {
            _scriptActivatedAuthorization = false;
        }
        else if (finishAction == FinishAction.Destroy)
        {
            Destroy(this.gameObject);
        }
        else if (finishAction == FinishAction.Loop)
        {
            NextRunDelayAction.afterAction = () =>
            {
                _endReached = false;
            };
            StartCoroutine(NextRunDelayAction.coAction());
        }

        if(nextAuthorization == NextAuthorization.BackAndForth)
        {
            SetNewFinalPosition(_initialPosition);
        }
        else if(nextAuthorization == NextAuthorization.StartOver)
        {
            transform.position = _initialPosition;
        }

        onFinishedCallback();
    }

    public virtual void SetNewFinalPosition(Vector3 finalPos)
    {
        _finalPosition = finalPos;
        _initialPosition = transform.position;
        CalculateDuration();
    }

    public virtual void AuthorizeMovement()
    {
        _scriptActivatedAuthorization = true;
        _endReached = false;
    }

    public virtual void InstantMoveToDestination()
    {
        _initialPosition = transform.position;
        transform.position += destinationPosition;
        OnFinishMove();
    }

    protected virtual void CalculateDuration()
    {
        _totalDuration = Vector3.Distance(_initialPosition, _finalPosition) / MovementSpeed;
        _currentTime = 0f;
    }

    /// <summary>
    /// On DrawGizmos, we draw lines to show the path the object will follow
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        if (Gizmo.DrawGizmo)
        {
            Gizmos.color = Gizmo.Color;
            Gizmos.DrawWireSphere(transform.position + destinationPosition, 0.2f);
            Gizmos.DrawLine(transform.position, transform.position + destinationPosition);
        }
    }

}
