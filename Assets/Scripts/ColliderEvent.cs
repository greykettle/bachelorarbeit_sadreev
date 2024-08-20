using UnityEngine;
using UnityEngine.Events;

public class ColliderEvent : MonoBehaviour
{
    private GameObject targetObject;
    void OnCollisionEnter(Collision collision)
    {
        checkTargetObject();
        if (collision.gameObject == targetObject)
        {
            this.gameObject.transform.parent.GetComponent<AssemblyManager>().assemblingModel();
            Debug.Log("Check" + collision.gameObject.name);
        }

    }

       void checkTargetObject()
    {
       this.gameObject.transform.parent.GetComponent<AssemblyManager>().setTargetObject();
        targetObject = this.gameObject.transform.parent.GetComponent<AssemblyManager>().targetObject;
        Debug.Log("Check");
    }
}