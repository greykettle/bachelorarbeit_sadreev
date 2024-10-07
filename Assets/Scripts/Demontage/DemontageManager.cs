using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Demontage
{
    public enum DetailType
    {
        Gear,
        Detail,
        Tool,
    }


    public class DemontageManager : MonoBehaviour
    {
        public SpeechController speechController;
        public UIController canvasController;
        public List<Step> steps;
        public static int indexCurrentStep { get; private set; } = 0;
        [SerializeField] GameObject finalMenu;


        private void Start()
        {
            LaunchStep();
        }

        private void TryLaunchNextStep()
        {
            if (steps[indexCurrentStep].CheckStepIsCompleted())
            {
                indexCurrentStep++;
                if (indexCurrentStep < steps.Count)
                {
                    LaunchStep();
                }
                else 
                {
                    finalMenu.SetActive(true);
                }
            }
        }

        private void LaunchStep()
        {
            steps[indexCurrentStep].Launch();
            canvasController.UpdateInstructionText(indexCurrentStep);
            speechController.PlaySpeech(indexCurrentStep);
            //Enable text on ui, play audio,
        }

        private void Update()
        {
            if (indexCurrentStep < steps.Count)
            {
                TryLaunchNextStep();
            }
        }
    }
}