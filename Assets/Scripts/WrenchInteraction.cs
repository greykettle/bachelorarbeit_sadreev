using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WrenchInteraction : MonoBehaviour
{
    public Transform previousDetail; 
    public Transform testParent;
    public float snapDistance = 0.1f; 
    public float customYPosition = 0.0f; 
    public GameObject targetObject; 
    public float rotationDuration = 1.0f; 

    private bool isSnapped = false;
    private Rigidbody currentDetailRigidbody;
    private MeshCollider currentDetailMeshCollider;

    private Transform currentDetail;
    private Transform assembledParent;
    private GearboxAssembly gearboxAssembly;

    public GearboxAssembly GearboxAssembly
    {
        get { return gearboxAssembly; }
        set { gearboxAssembly = value; }
    }

    void Start()
    {
        currentDetail = this.transform;
        assembledParent = GameObject.Find("Assembled").transform;
        currentDetailRigidbody = currentDetail.GetComponent<Rigidbody>();
        currentDetailMeshCollider = this.GetComponent<MeshCollider>();

        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        if (gearboxAssembly == null)
        {
            Debug.LogError("GearboxAssembly not found");
        }
    }

    void FixedUpdate()
    {
        if (!isSnapped && currentDetail != null && previousDetail != null)
        {
            float distance = Vector3.Distance(previousDetail.position, currentDetail.position);

            if (distance < snapDistance)
            {
                SnapToPositionWithYOffset();
            }
        }
    }

    void SnapToPositionWithYOffset()
    {
        Transform assembledPreviousDetail = assembledParent.Find(previousDetail.name);
        Transform assembledCurrentDetail = assembledParent.Find(currentDetail.name);

        if (assembledPreviousDetail == null || assembledCurrentDetail == null)
        {
            Debug.LogError("could not find in assembled.");
            return;
        }

        Vector3 positionOffset = assembledCurrentDetail.position - assembledPreviousDetail.position;

        positionOffset.y = customYPosition;

        Quaternion rotationOffset = assembledCurrentDetail.rotation * Quaternion.Inverse(assembledPreviousDetail.rotation);

        currentDetail.position = previousDetail.position + positionOffset;
        currentDetail.rotation = rotationOffset * previousDetail.rotation;

        currentDetail.GetComponent<XRGrabInteractable>().enabled = false;

        if (currentDetailRigidbody != null)
        {
            currentDetailRigidbody.useGravity = false;
            currentDetailRigidbody.isKinematic = true;
        }

        isSnapped = true;
        currentDetail.parent = testParent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            Debug.Log("Collied");
            StartCoroutine(SnapToFinalPosition());
        }
    }

    IEnumerator SnapToFinalPosition()
    {
        Transform assembledPreviousDetail = assembledParent.Find(previousDetail.name);
        Transform assembledCurrentDetail = assembledParent.Find(currentDetail.name);

        if (assembledPreviousDetail == null || assembledCurrentDetail == null)
        {
            Debug.LogError("Не удалось найти соответствующую деталь в assembled.");
            yield break;
        }

        Vector3 targetPosition = previousDetail.position + (assembledCurrentDetail.position - assembledPreviousDetail.position);
        Quaternion targetRotation = assembledCurrentDetail.rotation * Quaternion.Inverse(assembledPreviousDetail.rotation) * previousDetail.rotation;


        Vector3 startPosition = currentDetail.position;
        Quaternion startRotation = currentDetail.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {

            currentDetail.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / rotationDuration);
            currentDetail.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }


        currentDetail.position = targetPosition;
        currentDetail.rotation = targetRotation;


        if (currentDetailRigidbody != null)
        {
            currentDetailRigidbody.useGravity = false;
            currentDetailRigidbody.isKinematic = true;
        }

        currentDetailMeshCollider.enabled = false;
        isSnapped = true;


        if (gearboxAssembly != null)
        {
            gearboxAssembly.OnDetailSnapped();
        }
    }
}
