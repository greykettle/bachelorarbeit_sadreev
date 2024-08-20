using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    public Text instructionText;
    private int currentStep = 1;

    void Start()
    {
        UpdateInstruction(); 
    }

    public void UpdateInstruction()
    {
        switch (currentStep)
        {
            case 1:
                instructionText.text = "Шаг 1: Взять деталь А и установить её.";
                break;
            case 2:
                instructionText.text = "Шаг 2: Взять деталь Б и соединить с деталью А.";
                break;
            
            default:
                instructionText.text = "Сборка завершена!";
                break;
        }
    }

    public void ProceedToNextStep()
    {
        currentStep++;
        UpdateInstruction();
    }
}
