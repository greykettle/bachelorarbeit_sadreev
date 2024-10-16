using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapAndFix : MonoBehaviour
{
    public bool getSnapped
    {
        get { return isSnapped; }
        set { isSnapped = value; }
    }

    public Transform testParent;
    public Transform currentDetail;
    public Transform snapPoint1A;
    public Transform snapPoint1B;
    public Transform snapPoint2A;
    public Transform snapPoint2B;
    public float snapDistance = 0.05f;

    public float customEulerRotationX = -90.0f; 
    public float customYPosition = 0.81f; 

    private MeshCollider detailMeshCollider;
    private Rigidbody detailRigidbody;
    private GearboxAssembly gearboxAssembly;

    private bool isSnapped = false;

    void Start()
    {
        detailRigidbody = currentDetail.GetComponent<Rigidbody>();
        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        if (gearboxAssembly == null)
        {
            Debug.LogError("GearboxAssembly error");
        }
    }

    void Update()
    {
        detailMeshCollider = currentDetail.GetComponent<MeshCollider>();

        if (!isSnapped)
        {
            float distanceA = Vector3.Distance(snapPoint1A.position, snapPoint2A.position);
            float distanceB = Vector3.Distance(snapPoint1B.position, snapPoint2B.position);

            if (distanceA < snapDistance && distanceB < snapDistance)
            {
                SnapToPosition();
                currentDetail.parent = testParent;
                isSnapped = true;
                gearboxAssembly.OnDetailSnapped();
            }
        }
    }

    void SnapToPosition()
    {
        Vector3 offset = snapPoint2A.position - snapPoint1A.position;
        this.transform.position += offset;

        Quaternion targetRotation = Quaternion.FromToRotation(snapPoint1B.position - snapPoint1A.position,
                                                              snapPoint2B.position - snapPoint2A.position);
        this.transform.rotation = targetRotation * this.transform.rotation;

        this.GetComponent<XRGrabInteractable>().enabled = false;

        // Apply custom Euler rotation X
        Vector3 eulerRotation = currentDetail.transform.rotation.eulerAngles;
        eulerRotation.x = customEulerRotationX;
        this.transform.rotation = Quaternion.Euler(eulerRotation);

        // Apply custom Y position
        Vector3 newPosition = this.transform.position;
        newPosition.y = customYPosition;
        this.transform.position = newPosition;

        Rigidbody rb = currentDetail.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        if (detailMeshCollider != null)
        {
            detailMeshCollider.enabled = false;
        }

        Debug.Log("detail fixed");
    }
}
