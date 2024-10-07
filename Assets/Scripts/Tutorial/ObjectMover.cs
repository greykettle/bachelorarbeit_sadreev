using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectMover : MonoBehaviour
{
    public TutorialManager tutorialManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObjectToMove"))
        {
            tutorialManager.OnObjectMovedToTarget(); 
        }
        other.GetComponent<Rigidbody>().isKinematic = true;
        other.GetComponent<XRGrabInteractable>().enabled = false;
        other.transform.position = new Vector3(1.705f, 0.513f, -0.189f);
        other.transform.rotation = Quaternion.Euler(-90f, 0f, 90f);
    }
}
