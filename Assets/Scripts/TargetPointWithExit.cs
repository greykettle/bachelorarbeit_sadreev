using Assets.Scripts.Demontage;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetPointWithExit : MonoBehaviour
{
    private bool isReady;
    public GameObject TargetDetail;
    public Vector3 endLocalPos;
    public Vector3 endLocalRot;


    private void OnTriggerEnter(Collider other)
    {
        if (isReady && other.gameObject == TargetDetail)
        {

            other.gameObject.GetComponent<Detail>().OnSort();
            other.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.localPosition = endLocalPos;
            other.transform.localEulerAngles = endLocalRot;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TargetDetail)
        {
            isReady = true;
        }
    }

}

