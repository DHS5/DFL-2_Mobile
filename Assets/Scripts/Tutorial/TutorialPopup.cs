using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPopup : MonoBehaviour
{
    protected MainManager main;

    [Header("Content")]
    [SerializeField] [TextArea] private string contentText;
    [Tooltip("Time between the previous popup and this one")]
    public float timeBeforeShowingUp;
    [SerializeField] private bool showOkButton;
    [Space, Space]
    [SerializeField] private string controllerTrigger;
    [SerializeField] private string fingerTrigger;
    [SerializeField] private bool showController;
    [SerializeField] private bool showFinger;
    [SerializeField] private bool showJumpButton;


    // UI Components 
    [Header("UI components")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button okButton;

    public Animator controllerAnimator;
    public Animator fingerAnimator;
    public GameObject jumpButton;

    protected bool active = false;

    public bool CanPass { get; protected set; }

    private void Awake()
    {
        main = FindObjectOfType<MainManager>();
    }

    private void Start()
    {
        text.text = contentText;
        okButton.gameObject.SetActive(showOkButton);

        controllerAnimator.gameObject.SetActive(showController);
        if (showController) controllerAnimator.SetTrigger(controllerTrigger);
        fingerAnimator.gameObject.SetActive(showFinger);
        if (showFinger) fingerAnimator.SetTrigger(fingerTrigger);
        jumpButton.SetActive(showJumpButton);
        main.touchManager.HideControllers(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(text.transform as RectTransform);

        CanPass = false;
    }

    public virtual void PreCondition()
    {
        active = true;
        gameObject.SetActive(true);

        Time.timeScale = 0;
    }

    public void Ok()
    {
        CanPass = true;

        Time.timeScale = 1;
        controllerAnimator.gameObject.SetActive(false);
        fingerAnimator.gameObject.SetActive(false);
        jumpButton.SetActive(false);
        main.touchManager.HideControllers(false);
    }
}
