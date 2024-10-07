using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MatchRelativeTransforms : MonoBehaviour
{
    public Transform previousDetail;
    public Transform testParent;
    public bool enablePreviousDetailGrab;
    public Vector3 localPositiononSnapError;

    public float snapDistance = 0.1f;
    private bool isSnapped = false;
    private Rigidbody currentDetailRigidbody;
    [SerializeField] private List<Collider> currentDetailColliders;

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
        currentDetailColliders = new List<Collider>(GetComponents<Collider>());

        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        if (gearboxAssembly == null)
        {
            Debug.LogError("GearboxAssembly not found.");
        }


    }

    void FixedUpdate()
    {
        GameObject assembledObject = GameObject.Find("Assembled");
        if (assembledObject != null)
        {
            assembledParent = assembledObject.transform;
        }
        else
        {
            Debug.LogError("Assembled not found.");
            return; 
        }
        if (!isSnapped && currentDetail != null && previousDetail != null)
        {
            Transform assembledPreviousDetail = assembledParent.Find(previousDetail.name);
            Transform assembledCurrentDetail = assembledParent.Find(currentDetail.name);


            if (assembledPreviousDetail == null)
            {
                Debug.LogError("Assembled detail with name " + previousDetail.name + " not found in assembled.");
                return;
            }

            if (assembledCurrentDetail == null)
            {
                Debug.LogError("Assembled detail with name " + currentDetail.name + " not found in assembled.");
                return;
            }

            float distance = Vector3.Distance(previousDetail.position, currentDetail.position);

            if (distance < snapDistance)
            {
                Vector3 positionOffset = assembledCurrentDetail.position - assembledPreviousDetail.position;
                Quaternion rotationOffset = assembledCurrentDetail.rotation * Quaternion.Inverse(assembledPreviousDetail.rotation);

                // currentDetail.position = previousDetail.position + positionOffset;
                currentDetail.position = previousDetail.position;
                Debug.Log($"Previous Detail Position {previousDetail.position}");
                currentDetail.rotation = rotationOffset * previousDetail.rotation;
                currentDetail.GetComponent<XRGrabInteractable>().enabled = false;

                if (currentDetailRigidbody != null)
                {
                    currentDetailRigidbody.useGravity = false;
                    currentDetailRigidbody.isKinematic = true;
                }

                isSnapped = true;
                currentDetail.parent = testParent;
                if (localPositiononSnapError != Vector3.zero)
                {
                    currentDetail.localPosition = localPositiononSnapError;
                }

                DisableAllColliders();

                gearboxAssembly.OnDetailSnapped();
            }
        }
    }

    private void DisableAllColliders()
    {
        for (int i = 0; i < currentDetailColliders.Count; i++)
        {
            currentDetailColliders[i].enabled = false;
        }
    }
}
