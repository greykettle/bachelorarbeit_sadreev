using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColliderLevelChange : MonoBehaviour
{
    public int activationLevel; 
    public int deactivationLevel; 

    private BoxCollider boxCollider;
    private XRGrabInteractable xrGrabInteractable;
    private GearboxAssembly gearboxAssembly;

    void Start()
    {

        boxCollider = GetComponent<BoxCollider>();
        xrGrabInteractable = GetComponent<XRGrabInteractable>();

        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        if (gearboxAssembly == null)
        {
            Debug.LogError("GearboxAssembly wurde nicht gefunden");
        }

    
        if (boxCollider != null) boxCollider.enabled = false;
        if (xrGrabInteractable != null) xrGrabInteractable.enabled = false;
    }

    void Update()
    {
     
        if (gearboxAssembly != null)
        {
            int currentLevel = gearboxAssembly.CurrentStep;

            if (currentLevel == activationLevel)
            {
             
                if (boxCollider != null) boxCollider.enabled = true;
                if (xrGrabInteractable != null) xrGrabInteractable.enabled = true;
            }
            else if (currentLevel == deactivationLevel)
            {
            
                if (boxCollider != null) boxCollider.enabled = false;
                if (xrGrabInteractable != null) xrGrabInteractable.enabled = false;
            }
        }
    }
}
