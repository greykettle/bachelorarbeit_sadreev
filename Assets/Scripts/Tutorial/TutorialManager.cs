using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private int currentStep;
    [SerializeField] private TMP_Text tutorialText;
    public List<Instruction> instructions; 
    [SerializeField] private Button tutorialButton;
    [SerializeField] private GameObject firstCircle;
    [SerializeField] private GameObject secondCircle;
    [SerializeField] private GameObject objectOnTable;
    [SerializeField] private GameObject targetPoint;
    [SerializeField] private Localizator localizator; 
    [SerializeField] private GameObject backToTheMenuCanvas; 

    void Start()
    {
        ShowCurrentInstruction();
        tutorialButton.onClick.AddListener(OnButtonClick);
        backToTheMenuCanvas.SetActive(false); 
    }

    void ShowCurrentInstruction()
    {
        if (currentStep < instructions.Count)
        {
            string language = localizator.currentLanguage;
            tutorialText.text = instructions[currentStep].GetText(language);
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

            backToTheMenuCanvas.SetActive(true);
            tutorialText.text = string.Empty; 
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
