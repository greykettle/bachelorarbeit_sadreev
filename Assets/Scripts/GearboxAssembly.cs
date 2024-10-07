using HighlightPlus;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GearboxAssembly : MonoBehaviour
{
    public List<Transform> assemblySteps;
    public UIController uiController;
    private int currentStep = 0;
    [SerializeField] float fixTime = 1.0f;
    public SpeechController speechController;
    [SerializeField] GameObject endMenu;

    public int CurrentStep
    {
        get { return currentStep; }
        set { currentStep = value; }
    }

    void Start()
    {
        StartAssembly();
        Invoke("PrepareDetails", fixTime);
    }

    private void PrepareDetails()
    {
        for (int i = 0; i < assemblySteps.Count; i++)
        {
            SwitchDetailInteraction(assemblySteps[i].gameObject, false);
        }

        SwitchDetailInteraction(assemblySteps[0].gameObject, true);
    }

    public void StartAssembly()
    {
        currentStep = 0;
        UpdateCurrentStep();
    }

    public void OnDetailSnapped()
    {
        SwitchDetailInteraction(assemblySteps[currentStep].gameObject, false);
        currentStep++;
        if (currentStep < assemblySteps.Count)
        {
            SwitchDetailInteraction(assemblySteps[currentStep].gameObject, true);
            UpdateCurrentStep();
        }
        else
        {
            AssemblyComplete();
        }
    }

    void UpdateCurrentStep()
    {
        Transform currentDetail = assemblySteps[currentStep];
        uiController.UpdateInstructionText(currentStep, false);
        speechController.PlaySpeech(currentStep);
    }

    void AssemblyComplete()
    {
        uiController.UpdateInstructionText(currentStep, true);
        endMenu.SetActive(true);

    }

    private void SwitchDetailInteraction(GameObject detail, bool value)
    {
   
        if (detail.TryGetComponent(out HighlightEffect highlight))
        {
            highlight.SetHighlighted(value);
        }
        else
        {
            Debug.LogError("Cannot find highlight " + detail.name);
        }

    
        bool hasSpecialComponents = detail.TryGetComponent<MatchRelativeTransforms>(out _) ||
                                    detail.TryGetComponent<SnapToPositionAndRotation>(out _) ||
                                    detail.TryGetComponent<BoltsManager>(out _);

       
        WrenchBoltManager wrenchManager = null;
        if (detail.TryGetComponent(out wrenchManager))
        {
            hasSpecialComponents = true;
        }

    
        if (!hasSpecialComponents)
        {
            return;
        }

      
        if (wrenchManager == null || value) 
        {
            detail.GetComponent<XRGrabInteractable>().enabled = value;
            detail.GetComponent<Rigidbody>().isKinematic = !value;

            var colliders = detail.GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = value;
            }
        }


        if (detail.TryGetComponent(out MatchRelativeTransforms component))
        {
            component.previousDetail.GetComponent<HighlightEffect>().highlighted = value;
            component.enabled = value;
            if (value == true && component.enablePreviousDetailGrab)
            {
                SwitchDetailInteraction(component.previousDetail.gameObject, value);
            }
        }

  
        if (detail.TryGetComponent(out SnapToPositionAndRotation fix))
        {
            fix.enabled = value;
        }

        if (detail.TryGetComponent(out BoltsManager manager))
        {
            manager.previousDetail.GetComponent<HighlightEffect>().highlighted = value;

 
            if (value == true && manager.enablePreviousDetailGrab)
            {
                var previousDetail = manager.previousDetail.gameObject;

                previousDetail.GetComponent<XRGrabInteractable>().enabled = true;
                previousDetail.GetComponent<Rigidbody>().isKinematic = false;
                previousDetail.GetComponent<Rigidbody>().useGravity = true;

                var previousColliders = previousDetail.GetComponents<Collider>();
                for (int i = 0; i < previousColliders.Length; i++)
                {
                    previousColliders[i].enabled = true;
                }

                
            }

            
            for (int i = 0; i < manager.bolts.Count; i++)
            {
                manager.bolts[i].GetComponent<HighlightEffect>().highlighted = value;
                manager.bolts[i].GetComponent<XRGrabInteractable>().enabled = value;
                manager.bolts[i].GetComponent<Rigidbody>().isKinematic = !value;

                if (manager.bolts[i].TryGetComponent(out MatchRelativeTransforms comp))
                {
                    comp.enabled = value;
                }
                else if (manager.bolts[i].TryGetComponent(out SnapToPositionAndRotation boltFix))
                {
                    boltFix.enabled = value;
                }

                var boltColliders = manager.bolts[i].GetComponents<Collider>();
                for (int j = 0; j < boltColliders.Length; j++)
                {
                    boltColliders[j].enabled = value;
                }
            }
        }


        if (wrenchManager != null)
        {

            wrenchManager.enabled = value;

            detail.GetComponent<XRGrabInteractable>().enabled = true;
            detail.GetComponent<Rigidbody>().isKinematic = false;

            foreach (var wrenchDetail in wrenchManager.details)
            {
                var cols = wrenchDetail.GetComponents<Collider>();
                foreach (var collider in cols)
                {
                    collider.enabled = value;
                }
            }
        }
    }
}
