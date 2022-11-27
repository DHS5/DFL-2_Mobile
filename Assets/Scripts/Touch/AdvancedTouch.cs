using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTouch
{
    public int Number { get; private set; }

    public Touch CurrentTouch { get; private set; }
    public Touch LastTouch { get; private set; }


    public TouchStatus Status { get; private set; }
    public TouchPhase CurrentPhase { get { return CurrentTouch.phase; } }

    public TouchMovement CurrentMovement { get; private set; }

    public Vector2 CurrentPosition { get { return CurrentTouch.position; } }
    private TouchHorizontalPosition hPosition;
    private float preciseHPos;
    public TouchHorizontalPosition HPosition { get { return hPosition; } }
    public float PreciseHPos { get { return preciseHPos; } }

    private TouchVerticalPosition vPosition;
    private float preciseVPos;
    public TouchVerticalPosition VPosition { get { return vPosition; } }
    public float PreciseVPos { get { return preciseVPos; } }

    [Tooltip("Initial position of the touch")]
    private Vector2 initialPosition;
    [Tooltip("Time at the start of the touch")]
    private float startTime;

    [Tooltip("End time of the previous touch")]
    private float endTime = -0.1f;
    [Tooltip("End position of the previous touch")]
    private Vector2 endPosition;

    public float Duration { get { return (endTime > 0 ? endTime : Time.time) - startTime; } }



    private TouchManager touchManager;

    private float movementThreshold;

    public AdvancedTouch(int _number, TouchManager _touchManager)
    {
        Number = _number;
        touchManager = _touchManager;

        Status = TouchStatus.INACTIVE;
        movementThreshold = Screen.width / 30;
    }

    public void Update()
    {
        if (Input.touchCount > Number)
        {
            CurrentTouch = Input.GetTouch(Number);
            touchManager.GetTouchPosition(CurrentTouch, ref hPosition, ref preciseHPos, ref vPosition, ref preciseVPos);

            if (CurrentPhase == TouchPhase.Began)
            {
                Begin(CurrentTouch);
            }
            if (CurrentPhase == TouchPhase.Moved || CurrentPhase == TouchPhase.Stationary)
            {
                AnalyseMove();
            }
            if (CurrentPhase == TouchPhase.Ended)
            {
                End();
            }
        }
        else if (Status != TouchStatus.INACTIVE)
        {
            Status = TouchStatus.INACTIVE;
        }
    }


    private void Begin(Touch touch)
    {
        if (Number == 0 && touchManager.JoystickMode == JoystickMode.MOVABLE) touchManager.ReferencePosition = touch.position;

        LastTouch = touch;

        initialPosition = touch.position;
        startTime = Time.time;
        endTime = -0.1f;

        if (Number > 0 && initialPosition.x > Screen.width / 2)
            Status = TouchStatus.TAP;
        else
            Status = TouchStatus.ACTIVE;
        //Status = TouchStatus.ACTIVE;
        CurrentMovement = TouchMovement.STATIONARY;
    }

    private void AnalyseMove()
    {
        if (Mathf.Max(Mathf.Abs(CurrentPosition.x - initialPosition.x), Mathf.Abs(CurrentPosition.y - initialPosition.y)) > movementThreshold)
        {
            Status = TouchStatus.ACTIVE;
            if (Mathf.Abs(CurrentPosition.x - initialPosition.x) > Mathf.Abs(CurrentPosition.y - initialPosition.y))
            {
                CurrentMovement = CurrentPosition.x - initialPosition.x > 0 ? TouchMovement.RIGHT : TouchMovement.LEFT;
            }
            else
            {
                CurrentMovement = CurrentPosition.y - initialPosition.y > 0 ? TouchMovement.UP : TouchMovement.DOWN;
            }
            initialPosition = CurrentPosition;
        }
        else
        {
            CurrentMovement = TouchMovement.STATIONARY;

            if (Time.time - startTime > touchManager.jumpMaxInputDuration)
            {
                Status = TouchStatus.ACTIVE;
            }
        }
    }

    private void End()
    {
        endTime = Time.time;
        Status = TouchStatus.QUIT;

        endPosition = LastTouch.position;
    }
}