using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CanvasRescaler : MonoBehaviour
{
    [System.Serializable]
    private enum CanvasMode { UNSCALED, SCALED }
    private enum RescaleMode { WHOLE_SCREEN, FIT_IN_SCREEN }

    [Header("UI components")]
    [SerializeField] private RectTransform scalerRect;

    [Header("Default parameters")]
    [SerializeField] private float defaultHeight;
    [SerializeField] private float defaultWidth;
    [Space]
    [SerializeField] private RescaleMode rescaleMode;
    [SerializeField] private CanvasMode canvasMode;

    private float screenHeight;
    private float screenWidth;

    private float heightRatio;
    private float widthRatio;
    private float effectiveRatio;
    private float wholeScreenHRatio;
    private float wholeScreenWRatio;
    private float wholeScreenRatio;

    readonly float ratioMaxRange = 1.1f;

    private void Awake()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        heightRatio = screenHeight / defaultHeight;
        widthRatio = screenWidth / defaultWidth;

        // Get the good ratio to use
        effectiveRatio = Mathf.Min(heightRatio, widthRatio) / heightRatio;

        // Get the whole screen ratio
        wholeScreenWRatio = widthRatio / scalerRect.lossyScale.x;
        wholeScreenHRatio = heightRatio / scalerRect.lossyScale.y;
        wholeScreenRatio = Mathf.Max(wholeScreenWRatio, wholeScreenHRatio);

        if (canvasMode == CanvasMode.UNSCALED && rescaleMode == RescaleMode.FIT_IN_SCREEN)
        {
            widthRatio = widthRatio > heightRatio ? Mathf.Min(widthRatio, heightRatio * ratioMaxRange) : widthRatio;
            heightRatio = heightRatio > widthRatio ? Mathf.Min(heightRatio, widthRatio * ratioMaxRange) : heightRatio;

            scalerRect.localScale = new Vector3(widthRatio, heightRatio, 1);

            Canvas.ForceUpdateCanvases();
        }
        else if (canvasMode == CanvasMode.SCALED && rescaleMode == RescaleMode.FIT_IN_SCREEN && effectiveRatio != 1)
        {
            scalerRect.localScale = new Vector3(effectiveRatio, effectiveRatio, 1);

            Canvas.ForceUpdateCanvases();
        }
        else if (rescaleMode == RescaleMode.WHOLE_SCREEN)
        {
            scalerRect.localScale = new Vector3(wholeScreenRatio, wholeScreenRatio, 1);

            Canvas.ForceUpdateCanvases();
        }
    }
}
