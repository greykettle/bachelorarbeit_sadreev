using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DetailSnap : MonoBehaviour
{

    public bool getSnapped
    {
        get { return isSnapped; }
        set { isSnapped = value; }
    }

    public Transform targetDetail; 
    private GearboxAssembly gearboxAssembly; 
    public float snapDistance = 0.1f; 

    private bool isSnapped = false;
    private Rigidbody detailRigidbody;
    private MeshCollider detailMeshCollider;

    void Start()
    {
        detailRigidbody = GetComponent<Rigidbody>();
        detailMeshCollider = GetComponent<MeshCollider>();
        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        if (gearboxAssembly == null)
        {
            Debug.LogError("GearboxAssembly не найден в сцене. Убедитесь, что он присутствует и активен.");
        }
    }


    void FixedUpdate()
    {
        if (isSnapped || targetDetail == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, targetDetail.position);

        if (distance < snapDistance)
        {
            SnapToTarget();
        }
    }

    void SnapToTarget()
    {
        transform.position = targetDetail.position;
        transform.rotation = targetDetail.rotation;

        XRGrabInteractable interactable = GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.enabled = false;
        }

        if (detailRigidbody != null)
        {
            detailRigidbody.useGravity = false;
            detailRigidbody.isKinematic = true;
        }

        if (detailMeshCollider != null)
        {
            detailMeshCollider.enabled = false;
        }


        transform.parent = targetDetail;
        isSnapped = true;
        

        gearboxAssembly.OnDetailSnapped();
    }
}
