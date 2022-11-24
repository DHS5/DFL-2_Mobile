using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum TouchHorizontalPosition { CENTER = 0, LEFT = -1, RIGHT = 1 }
public enum TouchVerticalPosition { CENTER = 0, UP = 1, DOWN = -1 }
public enum TouchMovement { STATIONARY, LEFT, RIGHT, UP, DOWN }
public enum TouchStatus { INACTIVE, ACTIVE, QUIT, TAP }


public class TouchManager : MonoBehaviour
{
    [Header("UI objects")]
    [SerializeField] private Canvas rootCanvas;
    [Space]
    [SerializeField] private RectTransform joystick;
    [SerializeField] private RectTransform touchReferenceCenter;
    [SerializeField] private RectTransform touchReferenceRange;


    private AdvancedTouch[] touches = new AdvancedTouch[3];

    private Vector2 refPosition;
    private Vector2 touchPos;
    private Vector2 joystickPos;
    private Vector2 touchDirection;

    public readonly float jumpMaxInputDuration = 0.1f;
    public readonly float swipeMaxInputDuration = 0.5f;

    public float CenterSize
    {
        get { return touchReferenceCenter.rect.width / 2; }
        set
        {
            touchReferenceCenter.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value * 2);
            touchReferenceCenter.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value * 2);

            Canvas.ForceUpdateCanvases();
        }
    }
    public float Range
    {
        get { return touchReferenceRange.rect.width / 2 - CenterSize; }
        set
        {
            touchReferenceRange.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (CenterSize + value) * 2);
            touchReferenceRange.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (CenterSize + value) * 2);

            Canvas.ForceUpdateCanvases();
        }
    }

    public float Side
    {
        get
        {
            if (touches[0].Status != TouchStatus.ACTIVE)
            {
                return 0;
            }
            return touches[0].PreciseHPos;
        }
    }

    public int RawSide
    {
        get
        {
            if (touches[0].Status != TouchStatus.ACTIVE)
            {
                return 0;
            }
            return (int)touches[0].HPosition;
        }
    }
    public float Acc
    {
        get
        {
            if (touches[0].Status != TouchStatus.ACTIVE)
            {
                return 0;
            }
            return touches[0].PreciseVPos;
        }
    }
    public int RawAcc
    {
        get
        {
            if (touches[0].Status != TouchStatus.ACTIVE)
            {
                return 0;
            }
            return (int)touches[0].VPosition;
        }
    }

    public bool Jump
    {
        get
        {
            for (int i = 0; i < touches.Length; i++)
            {
                if (touches[i].Status == TouchStatus.QUIT && touches[i].Duration <= jumpMaxInputDuration
                    && touches[i].CurrentMovement == TouchMovement.STATIONARY)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public TouchMovement Swipe
    {
        get
        {
            for (int i = 1; i < touches.Length; i++)
            {
                if (touches[i].Status == TouchStatus.QUIT && touches[i].Duration <= swipeMaxInputDuration
                    && touches[i].Duration > jumpMaxInputDuration)
                {
                    return touches[i].CurrentMovement;
                }
            }
            return TouchMovement.STATIONARY;
        }
    }

    private void Awake()
    {
        CenterSize = 50;
        Range = 150;

        for (int i = 0; i < touches.Length; i++)
            touches[i] = new AdvancedTouch(i, this);
    }

    private void Start()
    {
        refPosition = new Vector2(touchReferenceCenter.position.x,
            touchReferenceCenter.position.y);
    }

    private void Update()
    {
        for (int i = 0; i < touches.Length; i++)
            touches[i].Update();
        if (Swipe != TouchMovement.STATIONARY)
            Debug.Log(Swipe);

        MoveJoystick();
    }



    public void GetTouchPosition(Touch touch, ref TouchHorizontalPosition hPos, ref float preciseH,
        ref TouchVerticalPosition vPos, ref float preciseV)
    {
        touchPos = touch.position;

        // Horizontal
        hPos = touchPos.x > refPosition.x + CenterSize ? TouchHorizontalPosition.RIGHT
            : touchPos.x < refPosition.x - CenterSize ? TouchHorizontalPosition.LEFT
            : TouchHorizontalPosition.CENTER;
        preciseH = Mathf.Clamp((Mathf.Abs(touchPos.x - refPosition.x) - CenterSize) / Range, 0, 1) * (float)hPos;
        // Vertical
        vPos = touchPos.y > refPosition.y + CenterSize ? TouchVerticalPosition.UP
            : touchPos.y < refPosition.y - CenterSize ? TouchVerticalPosition.DOWN
            : TouchVerticalPosition.CENTER;
        preciseV = Mathf.Clamp((Mathf.Abs(touchPos.y - refPosition.y) - CenterSize) / Range, 0, 1) * (float)vPos;
    }

    private void MoveJoystick()
    {
        if (touches[0].Status != TouchStatus.ACTIVE) joystickPos = refPosition;
        else
        {
            touchDirection = touches[0].CurrentPosition - refPosition;
            joystickPos = refPosition + touchDirection.normalized * Mathf.Clamp(Mathf.Sqrt(touchDirection.sqrMagnitude),
                -CenterSize - Range, CenterSize + Range);
        }
        joystick.position = joystickPos;
    }
}
