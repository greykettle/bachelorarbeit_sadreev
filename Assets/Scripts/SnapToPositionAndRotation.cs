using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapToPositionAndRotation : MonoBehaviour
{
    public Transform snapPointA;
    public Transform targetPointB;
    public Vector3 snapLocalPosition;
    public Vector3 snapLocalRotation;
    public float snapDistance = 0.05f;
    public Transform parent;
    private GearboxAssembly gearboxAssembly;
    private bool isSnapped = false;

    void Start()
    {
        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        if (gearboxAssembly == null)
        {
            Debug.LogError("GearboxAssembly не найден в сцене!");
        }
    }

    void Update()
    {
        if (!isSnapped && PointsAreClose())
        {
            SnapObject();
            isSnapped = true;

            if (gearboxAssembly != null)
            {
                Debug.Log("Вызов OnDetailSnapped() в GearboxAssembly");
                gearboxAssembly.OnDetailSnapped();
            }
            else
            {
                Debug.LogError("GearboxAssembly не установлен!");
            }
        }
    }

    bool PointsAreClose()
    {
        float distance = Vector3.Distance(snapPointA.position, targetPointB.position);
        return distance < snapDistance;
    }

    void SnapObject()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        if (parent != null)
        {
            transform.SetParent(parent);
        }

        transform.localPosition = snapLocalPosition;
        transform.localRotation = Quaternion.Euler(snapLocalRotation);

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log("Object snapped to the fixed local position and rotation!");
    }
}
