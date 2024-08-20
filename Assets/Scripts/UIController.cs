using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text instructionText; 
    public List<string> stepInstructions; 

    public void UpdateInstructionText(int stepIndex, bool isComplete = false)
    {
        if (isComplete)
        {
            instructionText.text = "Сборка завершена!";
        }
        else if (stepIndex < stepInstructions.Count)
        {
            instructionText.text = stepInstructions[stepIndex];
        }
        else
        {
            instructionText.text = "Неизвестный шаг.";
        }
    }
}
