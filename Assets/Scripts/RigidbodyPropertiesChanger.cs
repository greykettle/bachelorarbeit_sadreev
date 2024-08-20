using UnityEngine;

public class RigidbodyPropertiesChanger : MonoBehaviour
{
    void Start()
    {
       
        Rigidbody rb = GetComponent<Rigidbody>();

   
        if (rb != null)
        {
         
            rb.useGravity = false;
            rb.isKinematic = true;
            Debug.Log("Rigidbody properties changed in Start");
        }
        else
        {
            Debug.LogError("Rigidbody not found on the object");
        }
    }
}
