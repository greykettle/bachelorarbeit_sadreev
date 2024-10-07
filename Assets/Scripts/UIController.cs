using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    public TMP_Text instructionText; 

    public List<TextInstruction> stepInstructions;
    public Localizator localizator;


    public void UpdateInstructionText(int stepIndex, bool isComplete = false)
    {
        if (isComplete)
        {
            instructionText.text = localizator.currentLanguage == "eng" ? stepInstructions[stepIndex].eng : stepInstructions[stepIndex].ger;// "Сборка завершена!";
        }
        else if (stepIndex < stepInstructions.Count)
        {
            instructionText.text = localizator.currentLanguage == "eng" ? stepInstructions[stepIndex].eng : stepInstructions[stepIndex].ger;
        }
        else
        {
            instructionText.text = localizator.currentLanguage == "eng" ? stepInstructions[stepIndex].eng : stepInstructions[stepIndex].ger;// "Неизвестный шаг.";
        }
    }
}

[Serializable]
public class TextInstruction
{ 
   public string eng,ger;
}