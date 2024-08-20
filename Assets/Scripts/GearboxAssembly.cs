using System.Collections.Generic;
using UnityEngine;

public class GearboxAssembly : MonoBehaviour
{
    public List<Transform> assemblySteps; 
    public UIController uiController; 
    private int currentStep = 0;

    public int CurrentStep
    {
        get { return currentStep; }
        set { currentStep = value; }
    }

    void Start()
    {
        StartAssembly();
    
    }

    public void StartAssembly()
    {
        currentStep = 0;
        UpdateCurrentStep();
    }

    public void OnDetailSnapped()
    {
        currentStep++;
        Debug.Log(currentStep);
        if (currentStep < assemblySteps.Count)
        {
            UpdateCurrentStep();
            
        }
        else
        {
            AssemblyComplete();
        }
    }

    void UpdateCurrentStep()
    {
        // Логика активации/деактивации текущей детали
        Transform currentDetail = assemblySteps[currentStep];

        // Обновляем инструкцию на UI
        uiController.UpdateInstructionText(currentStep, false); // Второй аргумент - false, т.к. сборка не завершена
    }

    void AssemblyComplete()
    {
        uiController.UpdateInstructionText(currentStep, true); // Второй аргумент - true, т.к. сборка завершена
    }
}
