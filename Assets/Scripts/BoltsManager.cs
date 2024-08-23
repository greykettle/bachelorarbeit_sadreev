using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoltsManager : MonoBehaviour
{
    public Transform previousDetail;
    public List<Transform> bolts;
    public float snapDistance = 0.1f;
    private int boltsSnapped = 0;
    private bool allBoltsSnapped = false;
    private Transform assembledParent;
    private GearboxAssembly gearboxAssembly;

    private Dictionary<Transform, bool> boltSnapStatus = new Dictionary<Transform, bool>();

    void Start()
    {
        assembledParent = GameObject.Find("Assembled").transform;

        if (assembledParent == null)
        {
            Debug.LogError("Assembled object not found in the scene. Ensure it is present and active.");
            return;
        }

        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        if (gearboxAssembly == null)
        {
            Debug.LogError("GearboxAssembly not found in the scene. Ensure it is present and active.");
            return;
        }

        foreach (var bolt in bolts)
        {
            boltSnapStatus[bolt] = false;
        }
    }

    void Update()
    {
        if (allBoltsSnapped) return;

        foreach (var bolt in bolts)
        {
            if (!boltSnapStatus[bolt])
            {
                SnapBoltIfNeeded(bolt);
            }
        }

        if (boltsSnapped == bolts.Count)
        {
            allBoltsSnapped = true;
            gearboxAssembly.OnDetailSnapped();
            Debug.Log("All bolts inserted! Proceeding...");
        }
    }

    void SnapBoltIfNeeded(Transform bolt)
    {
        Transform assembledPreviousDetail = assembledParent.Find(previousDetail.name);
        Transform assembledCurrentBolt = assembledParent.Find(bolt.name);

        if (assembledPreviousDetail == null)
        {
            Debug.LogError("Assembled detail with name " + previousDetail.name + " not found in assembled.");
            return;
        }

        if (assembledCurrentBolt == null)
        {
            Debug.LogError("Assembled detail with name " + bolt.name + " not found in assembled.");
            return;
        }

        float distance = Vector3.Distance(previousDetail.position, bolt.position);

        if (distance < snapDistance)
        {
            Vector3 positionOffset = assembledCurrentBolt.position - assembledPreviousDetail.position;
            Quaternion rotationOffset = assembledCurrentBolt.rotation * Quaternion.Inverse(assembledPreviousDetail.rotation);

            bolt.position = previousDetail.position + positionOffset;
            bolt.rotation = rotationOffset * previousDetail.rotation;

            XRGrabInteractable grabInteractable = bolt.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = false;
            }

            Rigidbody rb = bolt.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            // Устанавливаем previousDetail родителем болта
            bolt.SetParent(previousDetail);

            boltSnapStatus[bolt] = true;
            boltsSnapped++;
            Debug.Log($"Bolt {bolt.name} snapped into place. Total snapped: {boltsSnapped}");
        }
    }
}
