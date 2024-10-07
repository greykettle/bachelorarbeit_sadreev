using Assets.Scripts.Demontage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetPoint : MonoBehaviour
{
    public GameObject TargetDetail;
    public Vector3 endLocalPos;
    public Vector3 endLocalRot;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TargetDetail)
        {
           
            other.gameObject.GetComponent<Detail>().OnSort();
            other.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.localPosition = endLocalPos;
            other.transform.localEulerAngles = endLocalRot;
        }
    }

}
