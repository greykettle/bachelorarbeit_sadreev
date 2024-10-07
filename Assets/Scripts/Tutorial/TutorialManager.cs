using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private int currentStep;
    [SerializeField] private TMP_Text tutorialText;
    public List<string> instructions;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private GameObject firstCircle;
    [SerializeField] private GameObject secondCircle;
    [SerializeField] private GameObject objectOnTable;
    [SerializeField] private GameObject targetPoint;
    void Start()
    {
        ShowCurrentInstruction();
        tutorialButton.onClick.AddListener(OnButtonClick);
    }

    void ShowCurrentInstruction()
    {
        if (currentStep < instructions.Count)
        {
            tutorialText.text = instructions[currentStep];
        }
    }
    public void CompleteObjective()
    {
        currentStep++;
        if (currentStep < instructions.Count)
        {
            ShowCurrentInstruction();
            ExecuteStepLogic();
        }
        else
        {
            tutorialText.text = "Тutorial wurde abgeschlossen.";
        }
    }
    void ExecuteStepLogic()
    {
        switch (currentStep)
        {
            case 0:
                tutorialButton.gameObject.SetActive(true);
                break;

            case 1:
                tutorialButton.gameObject.SetActive(false);
                firstCircle.SetActive(true);
                secondCircle.SetActive(false);
                break;

            case 2:
                firstCircle.SetActive(false);
                secondCircle.SetActive(true);
                break;

            case 3:
                firstCircle.SetActive(false);
                secondCircle.SetActive(false);
                objectOnTable.SetActive(true);
                targetPoint.SetActive(true);
                break;

            default:
                tutorialText.text = "Unknown step!";
                break;
        }
    }

    public void OnButtonClick()
    {
        if (currentStep == 0)
        {
            CompleteObjective();
        }
    }
    public void OnFirstCircleEntered()
    {
        if (currentStep == 1)
        {
            firstCircle.SetActive(false);
            secondCircle.SetActive(true);
            CompleteObjective();
        }
    }
    public void OnSecondCircleEntered()
    {
        if (currentStep == 2)
        {
            CompleteObjective();
        }
    }
    public void OnObjectMovedToTarget()
    {
        if (currentStep == 3)
        {
            CompleteObjective(); 
        }
    }
}
