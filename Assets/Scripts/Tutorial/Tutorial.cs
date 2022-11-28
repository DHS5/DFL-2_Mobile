using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial prerequisites")]
    public GameData gameData;
    public bool goalPostActive;


    [SerializeField] private TutorialPopup[] tutos;
    [SerializeField] private Animator controllerAnimator;
    [SerializeField] private Animator fingerAnimator;
    [SerializeField] private GameObject jumpButton;


    private void Awake()
    {
        InitTutosArray();
    }

    private void Start()
    {
        StartCoroutine(TutorialCR());
    }

    private void InitTutosArray()
    {
        foreach (TutorialPopup tuto in tutos)
        {
            tuto.gameObject.SetActive(false);
            tuto.controllerAnimator = controllerAnimator;
            tuto.fingerAnimator = fingerAnimator;
            tuto.jumpButton = jumpButton;
        }
    }

    private IEnumerator TutorialCR()
    {
        for (int i = 0; i < tutos.Length; i++)
        {
            yield return new WaitForSeconds(tutos[i].timeBeforeShowingUp);

            tutos[i].PreCondition();

            yield return new WaitUntil(() => tutos[i].CanPass);

            Destroy(tutos[i].gameObject);
        }
    }
}
