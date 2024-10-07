using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColliderLevelChange : MonoBehaviour
{
    public int activationLevel;
    public int deactivationLevel;

    public bool adjustPhysicsWithCollider = true;
    public bool enableGravityOnActivation = true;
    public bool disableGravityOnDeactivation = true;
    public bool enableXRInteractable = true;

    private Collider collider;
    private XRGrabInteractable xrGrabInteractable;
    private GearboxAssembly gearboxAssembly;
    private Rigidbody rb;

    void Start()
    {
        collider = GetComponent<Collider>();
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        if (gearboxAssembly == null)
        {
            Debug.LogError("GearboxAssembly not found in the scene.");
        }

      
    }

    void Update()
    {
        if (gearboxAssembly != null)
        {
            int currentLevel = gearboxAssembly.CurrentStep;

            if (currentLevel == activationLevel)
            {
                ActivateComponents();
            }
            else if (currentLevel == deactivationLevel)
            {
                DeactivateComponents();
            }
        }
    }

    private void ActivateComponents()
    {
        if (collider != null) collider.enabled = true;

        if (enableXRInteractable && xrGrabInteractable != null)
        {
            xrGrabInteractable.enabled = true;
        }

        if (adjustPhysicsWithCollider && rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = enableGravityOnActivation;
        }
    }

    private void DeactivateComponents()
    {
        if (collider != null) collider.enabled = false;

        if (enableXRInteractable && xrGrabInteractable != null)
        {
            xrGrabInteractable.enabled = false;
        }

        if (adjustPhysicsWithCollider && rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = !disableGravityOnDeactivation;
        }
    }
}
