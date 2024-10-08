using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoltsManager : MonoBehaviour
{
    public Transform previousDetail;
    public List<Transform> bolts;
    public float snapDistance = 0.1f;
    public bool disableColliderOnSnap = true;
    public bool enforceOrder = true;
    public bool useTriggers = true;
    private int boltsSnapped = 0;
    private bool allBoltsSnapped = false;
    private Transform assembledParent;
    private GearboxAssembly gearboxAssembly;
    public bool enablePreviousDetailGrab = true;
    public Vector3 localPositiononSnapError;

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
            if (useTriggers)
            {
                var trigger = bolt.gameObject.AddComponent<BoltTrigger>();
                trigger.Initialize(this, bolt);
            }
        }


        HighlightCurrentBolt();
    }

    void Update()
    {
        if (allBoltsSnapped || useTriggers) return;

        if (enforceOrder)
        {
            if (!boltSnapStatus[bolts[boltsSnapped]])
            {
                SnapBoltIfNeeded(bolts[boltsSnapped]);
            }
        }
        else
        {
            foreach (var bolt in bolts)
            {
                if (!boltSnapStatus[bolt])
                {
                    SnapBoltIfNeeded(bolt);
                }
            }
        }

        if (boltsSnapped == bolts.Count)
        {
            allBoltsSnapped = true;
            gearboxAssembly.OnDetailSnapped();
            Debug.Log("All bolts inserted! Proceeding...");

            if (!enablePreviousDetailGrab)
            {
                DisablePreviousDetailGrab();
            }
        }
        else
        {

            HighlightCurrentBolt();
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
            SnapBolt(bolt);
        }
    }

    public void OnBoltTriggered(Transform bolt)
    {
        if (allBoltsSnapped) return;

        if (enforceOrder && bolts[boltsSnapped] != bolt)
        {
            return;
        }

        SnapBolt(bolt);

        if (boltsSnapped == bolts.Count)
        {
            allBoltsSnapped = true;
            gearboxAssembly.OnDetailSnapped();
            Debug.Log("All bolts inserted! Proceeding...");

            if (!enablePreviousDetailGrab)
            {
                DisablePreviousDetailGrab();
            }
        }
        else
        {
     
            HighlightCurrentBolt();
        }
    }

    private void SnapBolt(Transform bolt)
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

        bolt.SetParent(previousDetail);

        if (localPositiononSnapError != Vector3.zero)
        {
            bolt.localPosition = bolt.localPosition + localPositiononSnapError;
        }



      
        if (bolt.TryGetComponent(out HighlightPlus.HighlightEffect highlightEffect))
        {
            highlightEffect.SetHighlighted(false);
        }

        if (disableColliderOnSnap)
        {
            DisableBoltColliders(bolt);
        }

        boltSnapStatus[bolt] = true;
        boltsSnapped++;
        Debug.Log($"Bolt {bolt.name} snapped into place. Total snapped: {boltsSnapped}");
    }

    private void HighlightCurrentBolt()
    {
        if (!enforceOrder) return;

       
        foreach (var bolt in bolts)
        {
            if (bolt.TryGetComponent(out HighlightPlus.HighlightEffect highlightEffect))
            {
                highlightEffect.SetHighlighted(false);
            }
        }

      
        if (boltsSnapped < bolts.Count)
        {
            Transform currentBolt = bolts[boltsSnapped];
            if (currentBolt.TryGetComponent(out HighlightPlus.HighlightEffect currentHighlight))
            {
                currentHighlight.SetHighlighted(true);
            }
        }
    }

    private void DisableBoltColliders(Transform bolt)
    {
        Collider col = bolt.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
            Debug.Log($"Collider of bolt {bolt.name} has been disabled.");
        }
    }

    private void DisablePreviousDetailGrab()
    {
        XRGrabInteractable grabInteractable = previousDetail.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
            Debug.Log("Grabbing of previous detail has been disabled.");
        }
    }
}

public class BoltTrigger : MonoBehaviour
{
    private BoltsManager boltsManager;
    private Transform bolt;

    public void Initialize(BoltsManager manager, Transform boltTransform)
    {
        boltsManager = manager;
        bolt = boltTransform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == boltsManager.previousDetail)
        {
            boltsManager.OnBoltTriggered(bolt);
            Debug.Log("Collision happened");
        }
    }
}
