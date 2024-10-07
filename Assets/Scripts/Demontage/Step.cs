using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Demontage
{

    [Serializable]
    public class Step
    {
        //description, sounds
        public List<Detail> detailsToCompleteStep;
        public bool isCompleted = false;
        public bool isToolStep = false;
        public bool isStepByStepActivation;

        private int indexOfDetailToActivation=0;
        //Главную деталь считать разобранной после того как мы ее взяли  вруки.
        //Все поддетали нам нужно разделитьс главной деталью  по изолированным слоям чтобы не было лишних  коллизий
        public void Launch()
        {
            if (isStepByStepActivation)
            {
                LaunchDetail();
            }
            else
            {
                for (int i = 0; i < detailsToCompleteStep.Count; i++)
                {
                    detailsToCompleteStep[i].OnLaunchPreparing();
                }
            }

            
        }

        private void LaunchDetail()
        {
            if (indexOfDetailToActivation == detailsToCompleteStep.Count)
            {
                isCompleted = true;
            }
            else
            {
                detailsToCompleteStep[indexOfDetailToActivation].OnLaunchPreparing();
                detailsToCompleteStep[indexOfDetailToActivation].step = this;
                indexOfDetailToActivation++;
            }
        }

        public void OnDetailSotringInStepByStep()
        {
            LaunchDetail();
        }

        public bool CheckStepIsCompleted()
        {
            for (int i = 0; i < detailsToCompleteStep.Count; i++)
            {
                if (detailsToCompleteStep[i].DetailType == DetailType.Tool )
                {
                    if (isToolStep && detailsToCompleteStep[i].isDemontagedAndSorted == false)
                    {
                        return false;
                    }
                }
                else if (detailsToCompleteStep[i].isDemontagedAndSorted == false)
                {
                    return false;
                }
            }

            isCompleted = true;
            return true;
        }
    }
}