using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] HighlightEffect GearsContainer, ToolsContainer, DetailsContainer;


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

            DisableHighlightDetailContainer();
            DisableHighlightGearContainer();
            DisableHighlightToolContainer();

            HighlightContainers();

            canvasController.UpdateInstructionText(indexCurrentStep);
            speechController.PlaySpeech(indexCurrentStep);
            //Enable text on ui, play audio,
        }

        private void HighlightContainers()
        {
            for (int i = 0; i < steps[indexCurrentStep].detailsToCompleteStep.Count; i++)
            {
                if (steps[indexCurrentStep].detailsToCompleteStep[i].DetailType == DetailType.Gear && GearsContainer.highlighted == false)
                {
                    GearsContainer.SetHighlighted(true);
                }
                else if (steps[indexCurrentStep].detailsToCompleteStep[i].DetailType == DetailType.Detail && DetailsContainer.highlighted == false)
                {
                    DetailsContainer.SetHighlighted(true);
                }
                else if (steps[indexCurrentStep].isToolStep && steps[indexCurrentStep].detailsToCompleteStep[i].DetailType == DetailType.Tool && ToolsContainer.highlighted == false)
                {
                    ToolsContainer.SetHighlighted(true);
                }
            }
        }

        private void Update()
        {
            if (indexCurrentStep < steps.Count)
            {
                TryLaunchNextStep();
            }

            var gears = steps[indexCurrentStep].detailsToCompleteStep.Where(detail => detail.DetailType == DetailType.Gear);
            if (gears.All(detail => detail.isDemontagedAndSorted))
            {
                DisableHighlightGearContainer();
            }

            var details = steps[indexCurrentStep].detailsToCompleteStep.Where(detail => detail.DetailType == DetailType.Detail);
            if (details.All(detail => detail.isDemontagedAndSorted))
            {
                DisableHighlightDetailContainer();
            }

            if (steps[indexCurrentStep].isToolStep)
            {
                var tool = steps[indexCurrentStep].detailsToCompleteStep.Where(detail => detail.DetailType == DetailType.Tool);
                if (tool.All(detail => detail.isDemontagedAndSorted))
                {
                    DisableHighlightToolContainer();
                }
            }


        }

        private void DisableHighlightGearContainer()
        {
            GearsContainer.SetHighlighted(false);
        }
        private void DisableHighlightToolContainer()
        {
            ToolsContainer.SetHighlighted(false);
        }
        private void DisableHighlightDetailContainer()
        {
            DetailsContainer.SetHighlighted(false);
        }
    }
}