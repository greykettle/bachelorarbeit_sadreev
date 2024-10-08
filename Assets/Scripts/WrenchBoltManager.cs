using System.Collections.Generic;
using UnityEngine;

public class WrenchBoltManager : MonoBehaviour
{
    public List<GameObject> details;
    private HashSet<GameObject> snappedDetails = new HashSet<GameObject>(); 
    private GearboxAssembly gearboxAssembly;

    void Start()
    {
        if (!enabled) return; 

        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        foreach (GameObject detail in details)
        {
            WrenchBolts trigger = detail.GetComponent<WrenchBolts>();
            if (trigger != null)
            {
                trigger.manager = this;  
            }
            else
            {
                Debug.LogError("no Wrenchbolts script");
            }
        }
    }

    void OnEnable()
    {
        Start(); 
    }

    public void DetailSnapped(GameObject detail)
    {
        if (!snappedDetails.Contains(detail))
        {
            snappedDetails.Add(detail);
        }

        if (snappedDetails.Count == details.Count)
        {
            Debug.Log("Next level.");
            if (gearboxAssembly != null)
            {
                gearboxAssembly.OnDetailSnapped();  
            }
        }
    }
}
