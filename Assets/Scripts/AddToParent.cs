using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToParent : MonoBehaviour
{
    public Transform testParent;
    private Transform currentDetail;
    private SnapAndFix snapStatus;
    // Start is called before the first frame update
    void Start()
    {
        snapStatus = GetComponent<SnapAndFix>();
        currentDetail = this.transform; 
    }

    // Update is called once per frame
    void Update()
    {
        if (snapStatus.getSnapped == true)
        {
            currentDetail.parent = testParent;
        }
    }
}
